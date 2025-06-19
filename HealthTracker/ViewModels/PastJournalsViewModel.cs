using HealthTracker.Models;
using HealthTracker.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthTracker.ViewModels
{
    public class PastJournalsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<JournalEntry> PastEntries { get; } = new ObservableCollection<JournalEntry>();

        public ICommand ViewJournalCommand { get; }

        public PastJournalsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            ViewJournalCommand = new Command<JournalEntry>(async (entry) => await ViewJournalDetails(entry));
        }

        public async Task LoadPastEntries()
        {
            if (IsBusy) return;
            IsBusy = true;
            PastEntries.Clear();
            var entries = await _databaseService.GetAllJournalEntriesAsync();
            foreach (var entry in entries)
            {
                PastEntries.Add(entry);
            }
            IsBusy = false;
        }

        private async Task ViewJournalDetails(JournalEntry entry)
        {
            if (entry == null) return;
            await Shell.Current.DisplayAlert(
                $"Journal - {entry.CreateDate:MMMM dd, yyyy}",
                entry.Content,
                "Close"
            );
        }
    }
}
