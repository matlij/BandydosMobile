using AutoMapper;
using Bandydos.Dto;
using Bandydos.Dto.Enums;
using BandydosMobile.Models;
using BandydosMobile.Models.Constants;
using BandydosMobile.Repository;

namespace BandydosMobile.Services
{
    public class EventUserDataStore : IDataStore<EventUser>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository _repository;

        public EventUserDataStore(IMapper mapper, IGenericRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public Task<CrudResult> AddAsync(EventUser item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<EventUser> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EventUser>> GetAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string id, EventUser user)
        {
            var uri = GetUri($"{UriConstants.EventUri}/{id}/eventuser");

            var eventDto = _mapper.Map<EventUserDto>(user);
            return _repository.PutAsync(uri.ToString(), eventDto);
        }

        private static UriBuilder GetUri(string path)
        {
            return new UriBuilder(UriConstants.BaseUri)
            {
                Path = path,
                Query = $"code={UriConstants.Apikey}"
            };
        }
    }
}