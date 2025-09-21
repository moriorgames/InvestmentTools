# Investment Tools — Documento Técnico (v0.1)

URL repositorio de GITHUB: https://github.com/moriorgames/InvestmentTools

> **Objetivo del documento**: Proveer contexto técnico claro y accionable para colaborar con una IA/teammates en la construcción de *Investment Tools*, una solución en **.NET 8** para ingesta, persistencia y visualización de datos financieros, siguiendo **buenas prácticas** (SOLID, Clean Code), **Clean Architecture** (Ports & Adapters), **TDD** y **DDD**.

---

## 1) Visión del proyecto

**Investment Tools** combina programación + finanzas para:
1. **Persistir** datos financieros (activos, precios, indicadores, eventos) de forma consistente y auditable.
2. **Procesar** y **combinar** dichas fuentes para generar señales/algoritmos (más adelante).
3. **Exponer** un **panel frontal** con visualizaciones y métricas, alimentado semanalmente.

> **Fase actual**: foco en **persistencia fiable** y contrato de dominio estable. La lógica algorítmica y el front vendrán después.

**Principios rectores**
- **Calidad primero**: diseño guiado por **TDD**.
- **Evolutivo**: arquitectura preparada para crecer sin deuda innecesaria.
- **Trazabilidad**: cada dato tiene **origen**, **timestamp** y **hash** (si aplica) para auditoría.
- **Automatizado**: build, test y checks en CI desde el día 1.

---

## 2) Entorno de trabajo

- **SO**: macOS (Apple Silicon).
- **IDE**: JetBrains **Rider**.
- **CLI**: Terminal (zsh/bash) + **dotnet CLI**.
- **SCM**: **Git** Trunk Based Development.
- **Base de datos**: **MySQL 8.x** (desarrollo local) + **EF Core (Pomelo Provider)**.
- **Runtime**: **.NET 8**, Microsoft Visual Studio Solution File, Format Version 12.00

---

## 3) Alcance v0 (MVP de persistencia)

- **Modelado mínimo** del dominio necesario para almacenar:
    - **TBD**

---

## 4) Arquitectura (Clean Architecture + Ports & Adapters)

**Capas/Proyectos**
```
InvestmentTools.sln
  └─ code/
      ├─ Desktop/          # Frontal UI con Avalonia .NET 8 (MVVM)
      │      ├─ src/
      ├─ Domain/           # Entidades DDD, VOs, reglas de dominio, eventos
      │      ├─ src/
      │      └─ test/ 
      ├─ Application/      # Casos de uso (UseCases), puertos, DTOs
      │      ├─ src/
      │      └─ test/
      └─ Infrastructure/   # Adapters: EF Core, MySQL, Logs, etc.
             ├─ src/
             └─ test/
```

**Reglas**
- **Domain** no referencia a nadie.
- **Application** depende solo de **Domain**.
- **Infrastructure** implementa los **puertos** definidos en **Domain**.
- **Desktop** es la interacción con el usuario UI.


**Beneficios**
- Aislamiento del dominio.
- Sustituibilidad de infraestructura (p.ej. MySQL → PostgreSQL) sin reescribir la app.
- Testeabilidad: puertos *test doubles* y tests de integración sólo donde aportan valor.

---

## 5) Dominio (DDD ligero)

**Entidades y VOs iniciales** (Ideas):
- `Asset` (Aggregate Root)
    - `AssetId` (VO), `Ticker` (VO), `Name`, `AssetType` (enum: Equity, ETF, Commodity, Crypto, FX, Bond, Index), `Currency` (VO ISO-4217), `IsActive`.
- `PriceSnapshot`
    - `AssetId`, `TimestampUtc` (VO), `Open`, `High`, `Low`, `Close`, `Volume` (nullable), `Source` (VO), `IngestionRunId` (FK opcional).
- `EconomicIndicator`
    - `IndicatorId`, `Name`, `Period` (VO: Year/Month/Week/Date), `Value` (decimal), `Unit` (string), `Source`.
- `IngestionRun`
    - `IngestionRunId`, `Source`, `StartedAtUtc`, `FinishedAtUtc`, `Status` (enum), `Notes`.

**Invariantes y reglas** (ejemplos)
- `PriceSnapshot.TimestampUtc` debe ser único por `(AssetId, TimestampUtc)`.
- No se aceptan `Close` negativas; `High >= Max(Open, Low, Close)`; `Low <= Min(Open, High, Close)`.
- `Asset.Ticker` único por mercado/`Source` (si aplica).

---

## 6) Persistencia (EF Core + MySQL)

**Proveedor**: Pomelo.EntityFrameworkCore.MySql

**Convenciones**
- Tablas `snake_case` (ej.: `price_snapshot`).
- Claves compuestas donde aplique (`PriceSnapshot`: `asset_id` + `timestamp_utc`).
- Índices:
    - `IX_price_snapshot_asset_time` en `(asset_id, timestamp_utc)`.
    - `IX_price_snapshot_time` en `timestamp_utc` para búsquedas por rango.
- Campos decimales con precisión definida (ej. `decimal(18,6)` para precios, `decimal(20,8)` para cripto si se precisa).

**Conexión local (ejemplo)**
```
"ConnectionStrings": {
  "InvestmentToolsDb": "Server=localhost;Port=3306;Database=investment_tools;User Id=it_user;Password=***;TreatTinyAsBoolean=true;"
}
```

### Migraciones


**Migraciones**: `dotnet ef migrations add Init --project src/InvestmentTools.Infrastructure --startup-project src/InvestmentTools.WebApi`

```shell
dotnet ef migrations add Init --project code/Infrastructure/src/Infrastructure.csproj
dotnet ef database update --project code/Infrastructure/src/Infrastructure.csproj
```

---

## 7) Testing (TDD)

**Piramide**
- **Unit** (rápidos): reglas de dominio, mappers, validadores.
- **Integration** (selectivos): Repositorios EF, endpoints críticos con MySQL real.
- **Contract** (futuro): contra fuentes externas cuando se añadan (p.ej. WireMock/MockHttp).

**Stack**
- xUnit + FluentAssertions.
- *Testcontainers for .NET* (opcional) para MySQL ARM64 en local.

**Ejemplos**
- `AssetTests` → unicidad de `Ticker`, normalización.
- `PriceSnapshotTests` → coherencia OHLC, uniqueness por `(AssetId, TimestampUtc)`.
- `PriceRepositoryIntegrationTests` → upsert por lote y rendimiento básico.

---

## 8) Observabilidad y cross‑cutting

- **Logging**: Serilog (Console + File).
- **Métricas/Tracing**: OpenTelemetry (exporter OTLP).
- **Idempotencia**: claves naturales y *upserts* para ingestas.
- **Retried ops**: Polly (exponencial backoff en IO).
- **Seguridad**: Secrets en `dotnet user-secrets`/GitHub Actions Secrets; nunca en repo.
- **Validación de entrada**: FluentValidation + filtros.

---

## 9) Estándares de código

- **C# 12**, `nullable enable`, `implicit usings` activados.
- **Análisis**: .editorconfig + Roslyn Analyzers; *treat warnings as errors* en Domain/Application.
- **Convenciones**
    - Nombres de clases: `PascalCase`; variables: `camelCase`; constantes: `SCREAMING_SNAKE_CASE`.
    - `async`/`await` en I/O; `CancellationToken` obligatorio en puertos y repos.
    - Evitar *anémico*: reglas en el dominio, no en services utilitarios.

**Commits (Conventional Commits)**
```
feat(domain): add PriceSnapshot OHLC invariants
fix(api): return 409 on duplicate price snapshot
chore(ci): enable test coverage report
```

## 10) Frontend — Avalonia Desktop (cross-platform)

Decisión: adoptamos Avalonia UI para un cliente de escritorio que muestre las gráficas e interactúe con la base de datos local/remota. Motivos:

- Multiplataforma real (macOS/Windows/Linux) con un único código .NET.
- Sin despliegue web: ejecución offline, arranque rápido y distribución sencilla (posible single-file/self-contained).
- Ecosistema de gráficas maduro (ScottPlot, LiveCharts2).

```
InvestmentTools.sln
  └─ code/
      └─ Desktop/
         └─ src/   # Avalonia .NET 8 (MVVM)
```

### Creación (Rider)

- Instalar plugin AvaloniaRider.
- Instalar plantillas: dotnet new install Avalonia.Templates.
- Rider → File → New... → New Project → Avalonia .NET MVVM App (.NET 8) → nombre Desktop.


## 11) User Secrets

```shell
dotnet user-secrets init --project code/Desktop/src/Desktop.csproj
dotnet user-secrets init --project code/Infrastructure/src/Infrastructure.csproj
```

Example:
```
dotnet user-secrets set "ConnectionStrings:InvestmentToolsDb" "..." --project code/Desktop/src/Desktop.csproj  
```