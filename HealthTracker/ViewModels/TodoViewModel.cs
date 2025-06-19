using HealthTracker.Models;
using HealthTracker.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HealthTracker.Views;
using System;

namespace HealthTracker.ViewModels
{
    public class TodoViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<TodoItem> CurrentItems { get; } = new ObservableCollection<TodoItem>();
        public ObservableCollection<TodoItem> CompletedItems { get; } = new ObservableCollection<TodoItem>();

        public ICommand AddItemCommand { get; }
        public ICommand CompleteItemCommand { get; }
        public ICommand ViewItemCommand { get; }

        public TodoViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            AddItemCommand = new Command(async () => await GoToDetailsPage(null));
            CompleteItemCommand = new Command<TodoItem>(async (item) => await CompleteItem(item));
            ViewItemCommand = new Command<TodoItem>(async (item) => await GoToDetailsPage(item));

            // Subscribe to messaging center to refresh list when an item is saved from the details page
            MessagingCenter.Subscribe<TodoItemDetailViewModel>(this, "RefreshTodoList", async (sender) => {
                await LoadItems();
            });
        }

        ~TodoViewModel()
        {
            MessagingCenter.Unsubscribe<TodoItemDetailViewModel>(this, "RefreshTodoList");
        }


        public async Task LoadItems()
        {
            if (IsBusy) return;
            IsBusy = true;

            var allItems = await _databaseService.GetTodoItemsAsync();

            CurrentItems.Clear();
            CompletedItems.Clear();

            foreach (var item in allItems.Where(i => !i.IsCompleted))
            {
                CurrentItems.Add(item);
            }

            foreach (var item in allItems.Where(i => i.IsCompleted).OrderByDescending(i => i.CompleteDate))
            {
                CompletedItems.Add(item);
            }

            IsBusy = false;
        }

        private async Task CompleteItem(TodoItem item)
        {
            if (item == null) return;

            item.IsCompleted = true;
            item.CompleteDate = DateTime.Now;
            await _databaseService.SaveTodoItemAsync(item);
            await LoadItems(); // Refresh the lists
        }

        private async Task GoToDetailsPage(TodoItem item)
        {
            var navigationParameters = new Dictionary<string, object>
            {
                { "TodoItem", item } // item will be null for new items
            };
            await Shell.Current.GoToAsync(nameof(TodoItemDetailPage), navigationParameters);
        }
    }
}
