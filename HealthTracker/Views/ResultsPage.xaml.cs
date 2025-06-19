using HealthTracker.ViewModels;

namespace HealthTracker.Views
{
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage(ResultsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
