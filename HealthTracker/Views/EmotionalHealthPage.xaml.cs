using HealthTracker.ViewModels;

namespace HealthTracker.Views;

public partial class EmotionalHealthPage : ContentPage
{
    private readonly EmotionalHealthViewModel _viewModel;
    public EmotionalHealthPage(EmotionalHealthViewModel viewModel)
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
