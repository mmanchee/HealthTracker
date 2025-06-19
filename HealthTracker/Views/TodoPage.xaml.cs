using HealthTracker.ViewModels;

namespace HealthTracker.Views;

public partial class TodoPage : ContentPage
{
    private readonly TodoViewModel _viewModel;
    public TodoPage(TodoViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadItems();
    }
}
