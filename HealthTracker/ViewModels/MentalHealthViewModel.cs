using HealthTracker.Models;
using HealthTracker.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthTracker.ViewModels
{
    public class MentalHealthViewModel : HealthViewModelBase
    {
        // Properties for each question
        private int _focus = 3, _decision = 3, _memory = 3, _clarity = 3, _problem = 3, _energy = 3, _learning = 3, _overwhelm = 1, _resilience = 3;

        public int Focus { get => _focus; set { _focus = value; OnPropertyChanged(); } }
        public int DecisionMaking { get => _decision; set { _decision = value; OnPropertyChanged(); } }
        public int Memory { get => _memory; set { _memory = value; OnPropertyChanged(); } }
        public int MentalClarity { get => _clarity; set { _clarity = value; OnPropertyChanged(); } }
        public int ProblemSolving { get => _problem; set { _problem = value; OnPropertyChanged(); } }
        public int MentalEnergy { get => _energy; set { _energy = value; OnPropertyChanged(); } }
        public int Learning { get => _learning; set { _learning = value; OnPropertyChanged(); } }
        public int Overwhelm { get => _overwhelm; set { _overwhelm = value; OnPropertyChanged(); } }
        public int MentalResilience { get => _resilience; set { _resilience = value; OnPropertyChanged(); } }

        public MentalHealthViewModel(DatabaseService databaseService) : base(databaseService)
        {
        }

        protected override async Task SaveEntry()
        {
            if (IsBusy) return;
            IsBusy = true;

            var entry = new MentalHealthEntry
            {
                FocusAndConcentration = Focus,
                DecisionMaking = DecisionMaking,
                Memory = Memory,
                MentalClarity = MentalClarity,
                ProblemSolving = ProblemSolving,
                MentalEnergy = MentalEnergy,
                Learning = Learning,
                Overwhelm = Overwhelm,
                MentalResilience = MentalResilience,
                CreateDate = System.DateTime.Now
            };

            await _databaseService.SaveMentalHealthEntryAsync(entry);
            IsSavedForToday = true;
            IsBusy = false;
            await Shell.Current.DisplayAlert("Success", "Your mental health entry has been saved.", "OK");
        }

        public override async Task CheckIfSavedToday()
        {
            IsSavedForToday = await _databaseService.HasEntryForToday<MentalHealthEntry>();
        }
    }
}