using HealthTracker.Models;
using HealthTracker.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HealthTracker.ViewModels
{
    // A helper class for a single line item in the results view (e.g., "Mood: 5")
    public class DailyEntryDisplay
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    // A helper class to structure data for the view
    public class HealthResultDisplay
    {
        public DateTime Date { get; set; }
        // MODIFIED to use the new helper class
        public List<DailyEntryDisplay> Entries { get; set; } = new();
    }

    [QueryProperty(nameof(HealthType), "HealthType")]
    public class HealthResultsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private string _healthType;
        private string _title;

        public ObservableCollection<HealthResultDisplay> FormattedData { get; } = new();

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string HealthType
        {
            get => _healthType;
            set
            {
                _healthType = value;
                // When the property is set from navigation, load the data
                Task.Run(async () => await LoadData());
            }
        }

        public HealthResultsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        private async Task LoadData()
        {
            if (IsBusy) return;
            IsBusy = true;

            // Dispatch to UI thread to update collections and properties
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                FormattedData.Clear();
                Title = $"{HealthType} Health Results";
            });

            switch (HealthType)
            {
                case "Mental":
                    var mentalData = await _databaseService.GetAllMentalHealthEntriesAsync();
                    // MODIFIED to use DailyEntryDisplay
                    var formattedMentalData = mentalData.Select(entry => new HealthResultDisplay
                    {
                        Date = entry.CreateDate,
                        Entries = new List<DailyEntryDisplay>
                        {
                            new() { Key = "Focus & Concentration", Value = entry.FocusAndConcentration.ToString() },
                            new() { Key = "Decision Making", Value = entry.DecisionMaking.ToString() },
                            new() { Key = "Memory", Value = entry.Memory.ToString() },
                            new() { Key = "Mental Clarity", Value = entry.MentalClarity.ToString() },
                            new() { Key = "Problem Solving", Value = entry.ProblemSolving.ToString() },
                            new() { Key = "Mental Energy", Value = entry.MentalEnergy.ToString() },
                            new() { Key = "Learning", Value = entry.Learning.ToString() },
                            new() { Key = "Overwhelm", Value = entry.Overwhelm.ToString() },
                            new() { Key = "Mental Resilience", Value = entry.MentalResilience.ToString() },
                        }
                    }).ToList();
                    UpdateFormattedData(formattedMentalData);
                    break;

                case "Physical":
                    var physicalData = await _databaseService.GetAllPhysicalHealthEntriesAsync();
                    // MODIFIED to use DailyEntryDisplay
                    var formattedPhysicalData = physicalData.Select(entry => new HealthResultDisplay
                    {
                        Date = entry.CreateDate,
                        Entries = new List<DailyEntryDisplay>
                        {
                            new() { Key = "Energy Level", Value = entry.EnergyLevel.ToString() },
                            new() { Key = "Sleep Quality", Value = entry.SleepQuality.ToString() },
                            new() { Key = "Pain/Discomfort", Value = entry.PainDiscomfort.ToString() },
                            new() { Key = "Hydration", Value = entry.Hydration.ToString() },
                            new() { Key = "Physical Strength", Value = entry.PhysicalStrength.ToString() },
                            new() { Key = "Recovery", Value = entry.Recovery.ToString() },
                            new() { Key = "Physical Activity", Value = entry.PhysicalActivity },
                            new() { Key = "Appetite", Value = entry.Appetite },
                        }
                    }).ToList();
                    UpdateFormattedData(formattedPhysicalData);
                    break;

                case "Emotional":
                    var emotionalData = await _databaseService.GetAllEmotionalHealthEntriesAsync();
                    // MODIFIED to use DailyEntryDisplay
                    var formattedEmotionalData = emotionalData.Select(entry => new HealthResultDisplay
                    {
                        Date = entry.CreateDate,
                        Entries = new List<DailyEntryDisplay>
                        {
                            new() { Key = "Overall Mood", Value = entry.OverallMood.ToString() },
                            new() { Key = "Stress Level", Value = entry.StressLevel.ToString() },
                            new() { Key = "Anxiety", Value = entry.Anxiety.ToString() },
                            new() { Key = "Emotional Stability", Value = entry.EmotionalStability.ToString() },
                            new() { Key = "Social Connection", Value = entry.SocialConnection.ToString() },
                            new() { Key = "Gratitude", Value = entry.Gratitude.ToString() },
                            new() { Key = "Optimism", Value = entry.Optimism.ToString() },
                            new() { Key = "Emotional Expression", Value = entry.EmotionalExpression.ToString() },
                            new() { Key = "Self-Compassion", Value = entry.SelfCompassion.ToString() },
                            new() { Key = "Emotional Regulation", Value = entry.EmotionalRegulation.ToString() },
                        }
                    }).ToList();
                    UpdateFormattedData(formattedEmotionalData);
                    break;
            }

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = false;
            });
        }

        private void UpdateFormattedData(List<HealthResultDisplay> data)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                FormattedData.Clear();
                foreach (var item in data)
                {
                    FormattedData.Add(item);
                }
            });
        }
    }
}