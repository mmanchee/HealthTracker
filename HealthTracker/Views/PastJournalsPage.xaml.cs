using HealthTracker.ViewModels;

namespace HealthTracker.Views;

public partial class PastJournalsPage : ContentPage
{
    private readonly PastJournalsViewModel _viewModel;
    public PastJournalsPage(PastJournalsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadPastEntries();
    }
}
