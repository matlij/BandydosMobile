using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BandydosMobile.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Time => $"{Date.TimeOfDay:hh\\:mm}";
        public string DateString => $"{Date.Date.ToLongDateString()}";
        public string DaysLeft { get => $"Dagar kvar: {(Date - DateTime.Now).Days}"; }
        public EventType? EventType { get; set; }
        public Address? Address { get; set; }
        public ObservableCollection<EventUser> Users { get; set; } = new ObservableCollection<EventUser>();
        public bool CurrentUserIsAttending { get; set; }
        public string UsersAttending { get => $"Närvaro: {Users.Count(u => u.IsAttending)} / {Users.Count}"; }
    }

    public class Address
    {
        public Guid Id { get; set; }
        public string FullAddress { get; set; } = string.Empty;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
