using CommunityToolkit.Mvvm.ComponentModel;
using BandydosMobile.Models;
using BandydosMobile.Services;
using BandydosMobile.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BandydosMobile.ViewModels;

public partial class EventsViewModel : BaseViewModel
{
    [ObservableProperty]
    private Event? selectedItem;

    private readonly IEventDataStore _dataStore;

    public ObservableCollection<Event> Items { get; }
    public Command LoadItemsCommand { get; }
    public Command AddItemCommand { get; }
    public Command<Event> ItemTapped { get; }

    public EventsViewModel(IEventDataStore dataStore)
    {
        Title = "Aktiviteter";
        Items = new ObservableCollection<Event>();
        LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

        ItemTapped = new Command<Event>(OnItemSelected);

        AddItemCommand = new Command(OnAddItem);
        _dataStore = dataStore;
    }

    private async Task ExecuteLoadItemsCommand()
    {
        IsBusy = true;

        try
        {
            Items.Clear();
            var items = await _dataStore.GetAsync();
            foreach (var @event in items.OrderBy(i => i.Date))
            {
                @event.CurrentUserIsAttending = UserIsAttendingEvent(@event, Guid.NewGuid()); //TODO
                Items.Add(@event);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hämta events misslycakdes", ex.Message);
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static async Task DisplayAlert(string title, string message)
    {
        var page = Application.Current?.MainPage;
        if (page != null)
        {
            await page.DisplayAlert(title, message, "Stäng");
        }
    }

    public void OnAppearing()
    {
        IsBusy = true;
        SelectedItem = null;
    }

    private async void OnAddItem(object obj)
    {
        //await Shell.Current.GoToAsync($"{nameof(NewItemPage)}?{nameof(NewItemViewModel.ItemId)}=0");
    }

    private async void OnItemSelected(Event item)
    {
        if (item == null)
            return;

        //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
    }

    private static bool UserIsAttendingEvent(Event @event, Guid userId)
    {
        var user = @event.Users.FirstOrDefault(u => u.UserId == userId);

        return user != null && user.IsAttending;
    }
}
