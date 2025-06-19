using HealthTracker.Models;
using HealthTracker.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HealthTracker.ViewModels
{
    [QueryProperty(nameof(TodoItem), "TodoItem")]
    public class TodoItemDetailViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private TodoItem _item;
        private string _summary;
        private string _description;
        private int _priority = 3;
        private bool _isExistingItem;

        public TodoItem TodoItem
        {
            get => _item;
            set
            {
                _item = value;
                if (_item != null)
                {
                    Summary = _item.Summary;
                    Description = _item.Description;
                    Priority = _item.Priority;
                    IsExistingItem = true;
                }
                else
                {
                    // For a new item
                    IsExistingItem = false;
                    _item = new TodoItem();
                }
                OnPropertyChanged(nameof(IsReadOnly));
                OnPropertyChanged(nameof(IsEditable));
            }
        }

        public string Summary { get => _summary; set { _summary = value; OnPropertyChanged(); } }
        public string Description { get => _description; set { _description = value; OnPropertyChanged(); } }
        public int Priority { get => _priority; set { _priority = value; OnPropertyChanged(); } }
        public bool IsExistingItem { get => _isExistingItem; set { _isExistingItem = value; OnPropertyChanged(); } }

        public bool IsReadOnly => _item != null && _item.IsCompleted;
        public bool IsEditable => !IsReadOnly;

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }


        public TodoItemDetailViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            SaveCommand = new Command(async () => await SaveItem());
            DeleteCommand = new Command(async () => await DeleteItem());
        }

        private async Task SaveItem()
        {
            if (string.IsNullOrWhiteSpace(Summary))
            {
                await Shell.Current.DisplayAlert("Validation Error", "Summary cannot be empty.", "OK");
                return;
            }

            _item.Summary = Summary;
            _item.Description = Description;
            _item.Priority = Priority;
            // CreateDate is set on model creation

            await _databaseService.SaveTodoItemAsync(_item);

            // Notify the main list to refresh
            MessagingCenter.Send(this, "RefreshTodoList");

            await Shell.Current.GoToAsync("..");
        }

        private async Task DeleteItem()
        {
            if (_item == null || _item.Id == 0) return;

            bool confirmed = await Shell.Current.DisplayAlert("Confirm", "Are you sure you want to delete this item?", "Yes", "No");
            if (confirmed)
            {
                await _databaseService.DeleteTodoItemAsync(_item);
                MessagingCenter.Send(this, "RefreshTodoList");
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}