using Bandydos.Dto;
using BandydosMobile.Extensions;
using AutoMapper;
using BandydosMobile.Models;

namespace BandydosMobile.Models.Translation
{
    internal class BandydosProfile : Profile
    {
        public BandydosProfile()
        {
            CreateMap<Address, AddressDto>();
            CreateMap<User, UserDto>();
            CreateMap<Event, EventDto>();
            CreateMap<EventUser, EventUserDto>();
            CreateMap<EventType, Bandydos.Dto.Enums.EventType>();

            CreateMap<AddressDto, Address>();
            CreateMap<UserDto, User>();
            CreateMap<EventDto, Event>();
            CreateMap<EventUserDto, EventUser>();
            CreateMap<Bandydos.Dto.Enums.EventType, EventType>()
                .ConvertUsing(src => EventType.FromInt((int)src));
        }
    }
}
