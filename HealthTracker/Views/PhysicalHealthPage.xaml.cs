using HealthTracker.ViewModels;

namespace HealthTracker.Views;

public partial class PhysicalHealthPage : ContentPage
{
    private readonly PhysicalHealthViewModel _viewModel;
    public PhysicalHealthPage(PhysicalHealthViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearing();
    }
}
