using HealthTracker.ViewModels;

namespace HealthTracker.Views;

public partial class TodoItemDetailPage : ContentPage
{
    public TodoItemDetailPage(TodoItemDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
