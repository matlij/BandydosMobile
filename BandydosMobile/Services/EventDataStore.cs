using AutoMapper;
using Bandydos.Dto;
using Bandydos.Dto.Enums;
using BandydosMobile.Models;
using BandydosMobile.Models.Constants;
using BandydosMobile.Repository;

namespace BandydosMobile.Services
{
    public class EventDataStore : IEventDataStore
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository _repository;

        public EventDataStore(IMapper mapper, IGenericRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CrudResult> AddAsync(Event @event)
        {
            var uri = GetUri($"{UriConstants.EventUri}/{@event.Id}");

            var eventDto = _mapper.Map<EventDto>(@event);
            await _repository.PostAsync(uri.ToString(), eventDto);

            return await Task.FromResult(CrudResult.Ok);
        }

        public Task<bool> UpdateAsync(string id, Event @event)
        {
            var uri = GetUri($"{UriConstants.EventUri}/{@event.Id}");

            var eventDto = _mapper.Map<EventDto>(@event);
            return _repository.PutAsync(uri.ToString(), eventDto);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var uri = GetUri($"{UriConstants.EventUri}/{id}");


            var result = await _repository.DeleteAsync(uri.ToString());

            return await Task.FromResult(result);
        }

        public async Task<Event> GetAsync(string id)
        {
            var uri = GetUri($"{UriConstants.EventUri}/{id}");

            var result = await _repository.GetAsync<EventDto>(uri.ToString());
            return _mapper.Map<Event>(result);
        }

        public async Task<IEnumerable<Event>> GetAsync(DateTime? from = null, bool forceRefresh = false)
        {
            var uri = GetUri(UriConstants.EventUri);
            if (from.HasValue)
            {
                uri.Query += $"&from={from.Value.Date}";
            }

            return await GetEvents(uri);
        }

        public async Task<IEnumerable<Event>> GetFromUserAsync(Guid userId, bool forceRefresh = false)
        {
            var uri = GetUri(UriConstants.EventUri);

            return await GetEvents(uri);
        }

        private static UriBuilder GetUri(string path)
        {
            return new UriBuilder(UriConstants.BaseUri)
            {
                Path = path,
                Query = $"code={UriConstants.Apikey}"
            };
        }

        private async Task<IEnumerable<Event>> GetEvents(UriBuilder uri)
        {
            var result = await _repository.GetAsync<IEnumerable<EventDto>>(uri.ToString());

            return result?.Select(r => _mapper.Map<Event>(r));

        }
    }
}