using System;

namespace BandydosMobile.Models
{
    public class EventUser : IEquatable<EventUser>
    {
        public EventUser(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
        public string UserName { get; set; }

        public bool IsAttending => UserReply == EventReply.Attending;

        public bool IsOwner { get; set; }
        public EventReply UserReply { get; set; }

        public string UserReplyString 
        { 
            get
            {
                return UserReply switch
                {
                    EventReply.NotReplied => "Ej svarat",
                    EventReply.Attending => "På",
                    EventReply.NotAttending => "Av",
                    _ => "Oklart",
                };
            }
        }


        public bool Equals(EventUser other)
        {
            if (other == null)
                return false;

            return other.UserId == UserId;
        }
    }
}
