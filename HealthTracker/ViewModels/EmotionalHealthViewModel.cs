using HealthTracker.Models;
using HealthTracker.Services;
using System.Threading.Tasks;

namespace HealthTracker.ViewModels
{
    public class EmotionalHealthViewModel : HealthViewModelBase
    {
        private int _mood = 3, _stress = 1, _anxiety = 1, _stability = 3, _connection = 3, _gratitude = 3, _optimism = 3, _expression = 3, _compassion = 3, _regulation = 3;

        public int OverallMood { get => _mood; set { _mood = value; OnPropertyChanged(); } }
        public int StressLevel { get => _stress; set { _stress = value; OnPropertyChanged(); } }
        public int Anxiety { get => _anxiety; set { _anxiety = value; OnPropertyChanged(); } }
        public int EmotionalStability { get => _stability; set { _stability = value; OnPropertyChanged(); } }
        public int SocialConnection { get => _connection; set { _connection = value; OnPropertyChanged(); } }
        public int Gratitude { get => _gratitude; set { _gratitude = value; OnPropertyChanged(); } }
        public int Optimism { get => _optimism; set { _optimism = value; OnPropertyChanged(); } }
        public int EmotionalExpression { get => _expression; set { _expression = value; OnPropertyChanged(); } }
        public int SelfCompassion { get => _compassion; set { _compassion = value; OnPropertyChanged(); } }
        public int EmotionalRegulation { get => _regulation; set { _regulation = value; OnPropertyChanged(); } }


        public EmotionalHealthViewModel(DatabaseService databaseService) : base(databaseService) { }

        protected override async Task SaveEntry()
        {
            if (IsBusy) return;
            IsBusy = true;

            var entry = new EmotionalHealthEntry
            {
                OverallMood = OverallMood,
                StressLevel = StressLevel,
                Anxiety = Anxiety,
                EmotionalStability = EmotionalStability,
                SocialConnection = SocialConnection,
                Gratitude = Gratitude,
                Optimism = Optimism,
                EmotionalExpression = EmotionalExpression,
                SelfCompassion = SelfCompassion,
                EmotionalRegulation = EmotionalRegulation,
                CreateDate = System.DateTime.Now
            };

            await _databaseService.SaveEmotionalHealthEntryAsync(entry);
            IsSavedForToday = true;
            IsBusy = false;
            await Shell.Current.DisplayAlert("Success", "Your emotional health entry has been saved.", "OK");
        }

        public override async Task CheckIfSavedToday()
        {
            IsSavedForToday = await _databaseService.HasEntryForToday<EmotionalHealthEntry>();
        }
    }
}
