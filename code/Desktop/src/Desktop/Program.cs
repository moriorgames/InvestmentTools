using System;
using Avalonia;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Persistence;

namespace Desktop;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((ctx, services) =>
            {
                var cs = ctx.Configuration.GetConnectionString("InvestmentToolsDb")
                         ?? throw new InvalidOperationException("Missing ConnectionStrings:InvestmentToolsDb");

                services.AddDbContext<EntityFrameworkContext>(opt =>
                    opt.UseMySql(cs, ServerVersion.AutoDetect(cs)));
            })
            .Build();

        BuildAvaloniaApp(host.Services).StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp(IServiceProvider sp) =>
        AppBuilder.Configure(() => new App { Services = sp })
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}

