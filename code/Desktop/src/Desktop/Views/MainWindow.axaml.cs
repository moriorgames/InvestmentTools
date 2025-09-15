using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Domain.Entity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScottPlot.Avalonia;

namespace Desktop.Views;

public partial class MainWindow : Window
{
    private readonly IServiceProvider serviceProvider;
    private readonly TextBlock statusText;
    private readonly TextBlock indicatorResultText;
    private readonly AvaPlot indicatorPlot;
    private readonly Button addIndicatorButton;

    public MainWindow()
    {
        InitializeComponent();

        if (Application.Current is not App app)
        {
            throw new InvalidOperationException("Unable to access application services.");
        }

        serviceProvider = app.Services;
        statusText = this.FindControl<TextBlock>("StatusText")
                     ?? throw new InvalidOperationException("StatusText control not found.");
        indicatorResultText = this.FindControl<TextBlock>("IndicatorResultText")
                             ?? throw new InvalidOperationException("IndicatorResultText control not found.");
        indicatorPlot = this.FindControl<AvaPlot>("IndicatorPlot")
                       ?? throw new InvalidOperationException("IndicatorPlot control not found.");
        addIndicatorButton = this.FindControl<Button>("AddIndicatorButton")
                           ?? throw new InvalidOperationException("AddIndicatorButton control not found.");

        Opened += async (_, __) => await RefreshAsync();
        addIndicatorButton.Click += async (_, __) => await AddTestIndicatorAsync();
    }

    private async Task RefreshAsync()
    {
        try
        {
            await using var db = await CreateContextAsync();
            var canConnect = await db.Database.CanConnectAsync();
            UpdateConnectionStatus(canConnect);

            if (!canConnect)
            {
                indicatorResultText.Text = "Database not reachable.";
                ClearPlot("No data to display");
                return;
            }

            await LoadIndicatorsAsync(db);
        }
        catch (Exception ex)
        {
            statusText.Text = "Error checking database";
            Title = "InvestmentTools — DB: error";
            indicatorResultText.Text = $"Unable to contact database: {ex.Message}";
            ClearPlot("No data to display");
        }
    }

    private async Task AddTestIndicatorAsync()
    {
        try
        {
            await using var db = await CreateContextAsync();
            var canConnect = await db.Database.CanConnectAsync();

            if (!canConnect)
            {
                UpdateConnectionStatus(false);
                indicatorResultText.Text = "Cannot insert indicator because the database is not reachable.";
                ClearPlot("No data to display");
                return;
            }

            var indicator = new Indicator(Guid.NewGuid().ToString("N"),
                $"Test Indicator {DateTime.UtcNow:HH:mm:ss}",
                Random.Shared.Next(10, 101),
                DateTime.UtcNow);

            db.Indicators.Add(indicator);
            await db.SaveChangesAsync();

            indicatorResultText.Text = $"Inserted {indicator.Name} with value {indicator.Value}.";
            await LoadIndicatorsAsync(db);
        }
        catch (Exception ex)
        {
            indicatorResultText.Text = $"Failed to insert indicator: {ex.Message}";
        }
    }

    private async Task LoadIndicatorsAsync(EntityFrameworkContext db)
    {
        var indicators = await db.Indicators
            .OrderBy(indicator => indicator.CreatedAt)
            .ToListAsync();

        if (indicators.Count == 0)
        {
            indicatorResultText.Text = "No indicators stored yet.";
            ClearPlot("No indicator data");
            return;
        }

        var latest = indicators[^1];
        indicatorResultText.Text = $"Last indicator: {latest.Name} — Value {latest.Value} at {latest.CreatedAt:u}";

        RenderBarChart(indicators);
    }

    private void RenderBarChart(IReadOnlyList<Indicator> indicators)
    {
        indicatorPlot.Plot.Clear();

        var values = indicators.Select(indicator => (double)indicator.Value).ToArray();
        var labels = indicators.Select(indicator => indicator.Name).ToArray();

        indicatorPlot.Plot.AddBar(values);
        indicatorPlot.Plot.XTicks(labels);
        indicatorPlot.Plot.SetAxisLimits(yMin: 0);
        indicatorPlot.Plot.Title("Indicator values");
        indicatorPlot.Plot.YLabel("Value");
        indicatorPlot.Plot.XLabel("Indicator");

        indicatorPlot.Refresh();
    }

    private void UpdateConnectionStatus(bool canConnect)
    {
        statusText.Text = canConnect ? "Connected to Database" : "Database Not Reachable";
        Title = $"InvestmentTools — DB: {(canConnect ? "connected" : "not reachable")}";
    }

    private void ClearPlot(string title)
    {
        indicatorPlot.Plot.Clear();
        indicatorPlot.Plot.Title(title);
        indicatorPlot.Refresh();
    }

    private async Task<EntityFrameworkContext> CreateContextAsync()
    {
        var factory = serviceProvider.GetRequiredService<IDbContextFactory<EntityFrameworkContext>>();
        return await factory.CreateDbContextAsync();
    }
}
