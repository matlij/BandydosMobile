﻿using BandydosMobile.Models;
using BandydosMobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BandydosMobile.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class EventDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(EquipmentManagerTappedCommand))]
        private Event _event = new();

        [ObservableProperty]
        private string _attendBtnText = string.Empty;
        [ObservableProperty]
        private bool _isAttending;

        [ObservableProperty]
        private string _equipmentBtnText = string.Empty;
        [ObservableProperty]
        private bool _isEquipmentManager;
        [ObservableProperty]
        private string _equipmentManager;

        [ObservableProperty]
        private string _itemId = string.Empty;

        [ObservableProperty]
        private ObservableCollection<EventUser> users = new();

        private readonly IEventDataStore _eventDataStore;
        private readonly IDataStore<EventUser> _eventUserDataStore;
        private readonly Authenticator _authenticator;
        private User? _user;

        public EventDetailViewModel(IEventDataStore dataStore, IDataStore<EventUser> eventUserDataStore, Authenticator authenticator)
        {
            _eventDataStore = dataStore;
            _eventUserDataStore = eventUserDataStore;
            _authenticator = authenticator;
        }

        [RelayCommand]
        public async Task AddressTapped()
        {
            try
            {
                if (string.IsNullOrEmpty(Event.Address?.FullAddress))
                {
                    return;
                }

                var locations = await Geocoding.GetLocationsAsync(Event.Address.FullAddress);
                var location = locations?.FirstOrDefault();

                if (location is null)
                {
                    Debug.WriteLine($"No locations found for address '{Event.Address}'");
                    return;
                }

                await Map.OpenAsync(location);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Debug.WriteLine($"Map function not supported on device. Exception: {fnsEx.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"No map application available to open or Geocoding failed. Exception: {e.Message}");
            }
        }

        [RelayCommand]
        public async void AttendTapped(object obj)
        {
            await UpdateUser(u => 
            {
                u.UserReply = u.IsAttending ? EventReply.NotAttending : EventReply.Attending;
                if (u.IsEquipmentManager && !u.IsAttending)
                {
                    u.IsEquipmentManager = false;
                }
            });
        }

        [RelayCommand(CanExecute = nameof(CanBeEquipmentManager))]
        public async Task EquipmentManagerTapped()
        {
            await UpdateUser(u => u.IsEquipmentManager = !u.IsEquipmentManager);
        }

        private async Task UpdateUser(Action<EventUser> updateUserAction)
        {
            try
            {
                IsBusy= true;

                if (_user == null)
                {
                    await Shell.Current.GoToAsync(nameof(LoginPage));
                    return;
                }
                var eventUser = GetAndAddEventUserIfNotExists(_user, _event);

                // Update user name if not set from before
                eventUser.Name = _user.Name;
                updateUserAction(eventUser);

                var success = await _eventUserDataStore.UpdateAsync(_event.Id.ToString(), eventUser);
                if (success)
                {
                    UpdateObservableProperties(_event, eventUser.UserId);
                }
                else
                {
                    Debug.WriteLine($"Failed to update event with id: {_event.Id}");
                }
            }
            catch (Exception e)
            {
                await DisplayError("Misslyckades att uppdatera event med ID: " + _itemId, exception: e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private static EventUser GetAndAddEventUserIfNotExists(User user, Event @event)
        {
            var eventUser = @event.Users.FirstOrDefault(u => u.UserId == user.Id);
            if (eventUser == null)
            {
                eventUser = new EventUser(user)
                {
                    UserReply = EventReply.Attending
                };
                @event.Users.Add(eventUser);
            }

            return eventUser;
        }

        public async Task LoadItem()
        {
            try
            {
                _user = await _authenticator.GetLoggedInUserAsync();
                if (_user == null)
                {
                    await Shell.Current.GoToAsync(nameof(LoginPage));
                    return;
                }

                var @event = await _eventDataStore.GetAsync(_itemId);
                if (@event is null)
                {
                    await DisplayError("Hämta användare eller event misslyckades", "Bandydos API returned null for event with ID: " + _itemId);
                    return;
                }

                ItemId = @event.Id.ToString();
                Title = @event.EventType?.ToString() ?? "Event";
                UpdateObservableProperties(@event, _user.Id);
            }
            catch (Exception e)
            {
                await DisplayError("Hämta användare eller event misslyckades", exception: e);
            }
        }

        private static async Task DisplayError(string title, string? message = null, Exception? exception = null)
        {
            var page = Application.Current?.MainPage;
            if (page != null)
            {
                await page.DisplayAlert(title, message ?? exception?.Message, "Stäng");
            }
        }

        private void UpdateObservableProperties(Event @event, string userId)
        {
            Event = new(); // Needed since ObservableProperties only get's updated if object is changed
            Event = @event;
            UpdateEventUserCollection(@event);

            var user = @event.Users.FirstOrDefault(u => u.UserId == userId);
            IsAttending = user?.IsAttending ?? false;
            AttendBtnText = IsAttending
                ? "Av"
                : "På!";

            IsEquipmentManager = user?.IsEquipmentManager ?? false;
            EquipmentBtnText = IsEquipmentManager
                ? "Ta bort mig som materialare"
                : "Jag kan ta med bollar, västar mm";

            EquipmentManager = @event.Users.SingleOrDefault(u => u.IsEquipmentManager)?.Name ?? "(ingen)";
        }

        private void UpdateEventUserCollection(Event @event)
        {
            Users.Clear();
            foreach (var eventUser in @event.Users)
            {
                Users.Add(eventUser);
            }
        }

        private bool CanBeEquipmentManager()
        {
            var isAttending = Event.Users.FirstOrDefault(u => u.UserId == _user?.Id)?.IsAttending ?? false;
            if (!isAttending)
            {
                return false;
            }

            var equipmentManager = Event.Users.SingleOrDefault(u => u.IsEquipmentManager);
            return equipmentManager == null || equipmentManager.UserId == _user?.Id;
        }
    }
}
