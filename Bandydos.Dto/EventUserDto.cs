using System;

namespace Bandydos.Dto
{
    public class EventUserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public UserReply UserReply { get; set; }
        public bool IsOwner { get; set; }
    }

    public enum UserReply
    {
        Unknown = 0,
        Going = 1,
        NotGoing = 2
    }
}
