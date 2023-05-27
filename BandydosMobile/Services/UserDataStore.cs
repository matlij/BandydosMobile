using AutoMapper;
using Bandydos.Dto;
using Bandydos.Dto.Enums;
using BandydosMobile.Models;
using BandydosMobile.Models.Constants;
using BandydosMobile.Repository;

namespace BandydosMobile.Services
{
    internal class UserDataStore : IDataStore<User>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository _repository;

        public UserDataStore(IMapper mapper, IGenericRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CrudResult> AddAsync(User item)
        {
            var uri = GetUri(UriConstants.UserUri);

            var (result, _) = await _repository.PostAsync(uri.ToString(), item);
            return result;
        }

        public async Task<bool> UpdateAsync(string id, User item)
        {
            var uri = GetUri($"{UriConstants.UserUri}/{item.Id}");

            return await _repository.PutAsync(uri.ToString(), item);
        }

        public Task<bool> DeleteAsync(string id)
        {
            var uri = new UriBuilder(UriConstants.BaseUri)
            {
                Path = $"{UriConstants.UserUri}/{id}"
            };

            return _repository.DeleteAsync(uri.ToString());
        }

        public async Task<IEnumerable<User>> GetAsync(DateTime? from = null, bool forceRefresh = false)
        {
            var uri = GetUri(UriConstants.UserUri);

            var result = await _repository.GetAsync<IEnumerable<UserDto>>(uri.ToString());
            return _mapper.Map<IEnumerable<User>>(result);
        }

        public async Task<User?> GetAsync(string id)
        {
            var uri = GetUri($"{UriConstants.UserUri}/{id}");

            var result = await _repository.GetAsync<UserDto>(uri.ToString());
            if (result is null)
            {
                return null;
            }

            return _mapper.Map<User>(result);
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
