using HealthTracker.ViewModels;

namespace HealthTracker.Views;

public partial class MentalHealthPage : ContentPage
{
    private readonly MentalHealthViewModel _viewModel;
    public MentalHealthPage(MentalHealthViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearing();
    }
}
