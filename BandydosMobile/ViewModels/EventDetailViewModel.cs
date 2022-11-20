using BandydosMobile.Models;
using BandydosMobile.Services;
using BandydosMobile.ViewModels;
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
        private Event _event;
        [ObservableProperty]
        private string _attendBtnText;
        [ObservableProperty]
        private bool _isAttending;
        [ObservableProperty]
        private string _itemId;
        [ObservableProperty]
        private ObservableCollection<EventUser> users;

        private readonly IEventDataStore _eventDataStore;
        private readonly IDataStore<EventUser> _eventUserDataStore;
        private readonly User _user;

        public EventDetailViewModel(IEventDataStore dataStore, IDataStore<EventUser> eventUserDataStore)
        {
            _eventDataStore = dataStore;
            _eventUserDataStore = eventUserDataStore;
            _user = new User()
            {
                Id = Guid.Parse("937ac36b-c115-4574-9b41-d7a8b1c65cfd")
            };
            Users = new ObservableCollection<EventUser>();
        }

        [RelayCommand]
        private async Task AddressTapped()
        {
            try
            {
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
        private async void AttendTapped(object obj)
        {
            try
            {
                IsBusy = true;

                var eventUser = _event.Users.FirstOrDefault(u => u.UserId == _user.Id);
                if (eventUser == null)
                {
                    var newEventUser = new EventUser(_user.Id)
                    {
                        UserName = _user.Name,
                        UserReply = EventReply.Attending
                    };
                    _event.Users.Add(newEventUser);
                }
                else
                {
                    eventUser.UserReply = eventUser.IsAttending
                        ? EventReply.NotAttending
                        : EventReply.Attending;
                }

                var success = await _eventUserDataStore.UpdateAsync(_event.Id.ToString(), eventUser);
                if (success)
                {
                    UpdateProperties(_event);
                }
                else
                {
                    Debug.WriteLine($"Failed to update event with id: {_event.Id}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to update event. " + e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadItem()
        {
            try
            {
                //User = await _userLoginService.GetUserFromLocalDb();
                var @event = await _eventDataStore.GetAsync(_itemId);
                ItemId = @event.Id.ToString();
                Title = @event.EventType?.ToString() ?? "Event";
                UpdateProperties(@event);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to Load Item. " + e.Message);
            }
        }

        private void UpdateProperties(Event @event)
        {
            Event = @event;
            UpdateEventUserCollection(@event);

            var user = @event.Users.FirstOrDefault(u => u.UserId == _user.Id);
            IsAttending = user?.IsAttending ?? false;
            AttendBtnText = IsAttending
                ? "UnAttend"
                : "Attend";
        }

        private void UpdateEventUserCollection(Event @event)
        {
            Users.Clear();
            foreach (var eventUser in @event.Users)
            {
                Users.Add(eventUser);
            }
        }
    }
}
