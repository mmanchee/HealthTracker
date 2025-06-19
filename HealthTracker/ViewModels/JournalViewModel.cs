using HealthTracker.Models;
using HealthTracker.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthTracker.Views;
using System;

namespace HealthTracker.ViewModels
{
    public class JournalViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private string _currentEntryContent;
        private JournalEntry _todayEntry;

        public string CurrentEntryContent
        {
            get => _currentEntryContent;
            set
            {
                _currentEntryContent = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand ViewPastJournalsCommand { get; }

        public JournalViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            SaveCommand = new Command(async () => await SaveCurrentEntry());
            ViewPastJournalsCommand = new Command(async () => await NavigateToPastJournals());
        }

        public async Task LoadCurrentEntry()
        {
            IsBusy = true;
            _todayEntry = await _databaseService.GetJournalForDateAsync(DateTime.Today);
            if (_todayEntry != null)
            {
                CurrentEntryContent = _todayEntry.Content;
            }
            else
            {
                CurrentEntryContent = ""; // Reset for new entry
            }
            IsBusy = false;
        }

        private async Task SaveCurrentEntry()
        {
            if (IsBusy) return;
            IsBusy = true;

            var entryToSave = _todayEntry ?? new JournalEntry { CreateDate = DateTime.Today };
            entryToSave.Content = CurrentEntryContent;

            await _databaseService.SaveJournalEntryAsync(entryToSave);

            // Reload the entry to get the ID if it was new
            _todayEntry = await _databaseService.GetJournalForDateAsync(DateTime.Today);
            IsBusy = false;
            await Shell.Current.DisplayAlert("Saved", "Your journal entry has been saved.", "OK");
        }

        private async Task NavigateToPastJournals()
        {
            await Shell.Current.GoToAsync(nameof(PastJournalsPage));
        }
    }
}
