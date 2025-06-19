using HealthTracker.ViewModels;

namespace HealthTracker.Views;

public partial class HealthResultsPage : ContentPage
{
    private readonly HealthResultsViewModel _viewModel;
    public HealthResultsPage(HealthResultsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}
