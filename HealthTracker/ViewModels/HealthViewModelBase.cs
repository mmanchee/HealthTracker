using HealthTracker.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthTracker.ViewModels
{
    public abstract class HealthViewModelBase : BaseViewModel
    {
        protected readonly DatabaseService _databaseService;
        private bool _isSavedForToday;

        public bool IsSavedForToday
        {
            get => _isSavedForToday;
            set
            {
                if (_isSavedForToday == value) return;
                _isSavedForToday = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsFormEnabled)); // Notify that the form's state might change
            }
        }

        public bool IsFormEnabled => !IsSavedForToday;

        public ICommand SaveCommand { get; }

        public HealthViewModelBase(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            SaveCommand = new Command(async () => await SaveEntry(), () => IsFormEnabled);
        }

        // Abstract method to be implemented by each specific health ViewModel
        protected abstract Task SaveEntry();

        // Abstract method to check the saved state
        public abstract Task CheckIfSavedToday();

        public async Task OnAppearing()
        {
            await CheckIfSavedToday();
        }
    }
}
