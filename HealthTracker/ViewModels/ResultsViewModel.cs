using HealthTracker.Models;
using HealthTracker.Services;
using HealthTracker.Views;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthTracker.ViewModels
{
    public class ResultsViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public ICommand NavigateToHealthResultsCommand { get; }
        public ICommand ExportDataCommand { get; }
        public ICommand ImportDataCommand { get; }

        public ResultsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            NavigateToHealthResultsCommand = new Command<string>(async (healthType) => await NavigateToHealthResults(healthType));
            ExportDataCommand = new Command(async () => await ExportData());
            ImportDataCommand = new Command(async () => await ImportData());
        }

        private async Task NavigateToHealthResults(string healthType)
        {
            if (string.IsNullOrWhiteSpace(healthType)) return;
            // Navigate to the results page, passing the health type as a query parameter
            await Shell.Current.GoToAsync($"{nameof(HealthResultsPage)}?HealthType={healthType}");
        }

        private async Task ExportData()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                var allData = new AllData
                {
                    MentalHealthEntries = await _databaseService.GetAllMentalHealthEntriesAsync(),
                    PhysicalHealthEntries = await _databaseService.GetAllPhysicalHealthEntriesAsync(),
                    EmotionalHealthEntries = await _databaseService.GetAllEmotionalHealthEntriesAsync(),
                    JournalEntries = await _databaseService.GetAllJournalEntriesAsync(),
                    TodoItems = await _databaseService.GetTodoItemsAsync()
                };

                var jsonString = JsonSerializer.Serialize(allData, new JsonSerializerOptions { WriteIndented = true });
                var exportFileName = $"HealthTracker_Export_{DateTime.Now:yyyyMMdd_HHmmss}.json";
                var exportFilePath = Path.Combine(FileSystem.CacheDirectory, exportFileName);

                await File.WriteAllTextAsync(exportFilePath, jsonString);

                await Share.Default.RequestAsync(new ShareFileRequest
                {
                    Title = "Export Health Data",
                    File = new ShareFile(exportFilePath)
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to export data: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ImportData()
        {
            if (IsBusy) return;

            try
            {
                var fileResult = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select Health Data JSON File",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.json" } },
                        { DevicePlatform.Android, new[] { "application/json" } },
                        { DevicePlatform.WinUI, new[] { ".json" } },
                        { DevicePlatform.MacCatalyst, new[] { "json" } },
                    })
                });

                if (fileResult == null) return; // User cancelled picker

                bool confirm = await Shell.Current.DisplayAlert("Warning", "Importing data will replace ALL existing data in the app. This cannot be undone. Are you sure you want to continue?", "Yes, Import", "Cancel");
                if (!confirm) return;

                IsBusy = true;

                string jsonContent = await File.ReadAllTextAsync(fileResult.FullPath);
                var importedData = JsonSerializer.Deserialize<AllData>(jsonContent);

                if (importedData != null)
                {
                    await _databaseService.ImportAllDataAsync(importedData);
                    await Shell.Current.DisplayAlert("Success", "Data imported successfully.", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Could not read data from the selected file. It might be empty or corrupt.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to import data: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
