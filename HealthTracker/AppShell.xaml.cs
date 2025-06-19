using HealthTracker.Views;

namespace HealthTracker;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register routes for pages that will be navigated to
        Routing.RegisterRoute(nameof(PastJournalsPage), typeof(PastJournalsPage));
        Routing.RegisterRoute(nameof(TodoItemDetailPage), typeof(TodoItemDetailPage));
        Routing.RegisterRoute(nameof(HealthResultsPage), typeof(HealthResultsPage));
    }
}
