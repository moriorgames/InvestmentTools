using Avalonia;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;

namespace Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Opened += async (_, __) =>
        {
            var sp = ((App)Application.Current!).Services;
            var db = sp.GetRequiredService<EntityFrameworkContext>();

            var canConnect = await db.Database.CanConnectAsync();
            Title = canConnect
                ? "InvestmentTools — DB: connected"
                : "InvestmentTools — DB: not reachable";
        };
    }
}
