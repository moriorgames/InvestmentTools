using Avalonia;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Opened += async (_, __) =>
        {
            var sp = ((App)Application.Current!).Services;

            var factory = sp.GetRequiredService<IDbContextFactory<EntityFrameworkContext>>();
            await using var db = await factory.CreateDbContextAsync();

            var ok = await db.Database.CanConnectAsync();
            Title = ok 
                ? "InvestmentTools — DB: connected" 
                : "InvestmentTools — DB: not reachable";
        };
    }
}