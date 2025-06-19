using HealthTracker.Models;
using HealthTracker.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthTracker.ViewModels
{
    public class PhysicalHealthViewModel : HealthViewModelBase
    {
        private int _energy = 3, _sleep = 3, _pain = 1, _hydration = 3, _strength = 3, _recovery = 3;
        private string _activity, _appetite;

        public int EnergyLevel { get => _energy; set { _energy = value; OnPropertyChanged(); } }
        public int SleepQuality { get => _sleep; set { _sleep = value; OnPropertyChanged(); } }
        public string PhysicalActivity { get => _activity; set { _activity = value; OnPropertyChanged(); } }
        public int PainDiscomfort { get => _pain; set { _pain = value; OnPropertyChanged(); } }
        public string Appetite { get => _appetite; set { _appetite = value; OnPropertyChanged(); } }
        public int Hydration { get => _hydration; set { _hydration = value; OnPropertyChanged(); } }
        public int PhysicalStrength { get => _strength; set { _strength = value; OnPropertyChanged(); } }
        public int Recovery { get => _recovery; set { _recovery = value; OnPropertyChanged(); } }

        public List<string> ActivityOptions { get; } = new List<string> { "None", "Light (walking)", "Moderate (30min exercise)", "Vigorous (60+ min intense)" };
        public List<string> AppetiteOptions { get; } = new List<string> { "No appetite", "Poor", "Normal", "Good", "Excessive" };

        public PhysicalHealthViewModel(DatabaseService databaseService) : base(databaseService)
        {
            PhysicalActivity = ActivityOptions[0];
            Appetite = AppetiteOptions[2];
        }

        protected override async Task SaveEntry()
        {
            if (IsBusy) return;
            IsBusy = true;

            var entry = new PhysicalHealthEntry
            {
                EnergyLevel = EnergyLevel,
                SleepQuality = SleepQuality,
                PhysicalActivity = PhysicalActivity,
                PainDiscomfort = PainDiscomfort,
                Appetite = Appetite,
                Hydration = Hydration,
                PhysicalStrength = PhysicalStrength,
                Recovery = Recovery,
                CreateDate = System.DateTime.Now
            };

            await _databaseService.SavePhysicalHealthEntryAsync(entry);
            IsSavedForToday = true;
            IsBusy = false;
            await Shell.Current.DisplayAlert("Success", "Your physical health entry has been saved.", "OK");
        }

        public override async Task CheckIfSavedToday()
        {
            IsSavedForToday = await _databaseService.HasEntryForToday<PhysicalHealthEntry>();
        }
    }
}
