using lab3.Models;
using Microsoft.Extensions.Caching.Memory;

namespace lab3.Services
{
    public class CachedAgencyDb
    {
        private readonly TouristAgency1Context _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly int _saveTime;
        public CachedAgencyDb(TouristAgency1Context dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _saveTime = 2 * 11 + 240;
        }
        public void AddClientToCache(string key, int rowsNumber = 100)
        {
            if (!_memoryCache.TryGetValue(key, out IEnumerable<Client> cachedUser))
            {
                cachedUser = _dbContext.Clients.Take(rowsNumber).ToList();

                if (cachedUser != null)
                {
                    _memoryCache.Set(key, cachedUser, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_saveTime)
                    });
                }
                Console.WriteLine("Таблица Client занесена в кеш");
            }
            else
            {
                Console.WriteLine("Таблица Client уже находится в кеше");
            }
        }
        public IEnumerable<Client> GetClient(string key, int rowsNumber = 100)
        {
            IEnumerable<Client> clients;
            if (!_memoryCache.TryGetValue(key, out clients))
            {
                clients = _dbContext.Clients.Take(rowsNumber).ToList();
                if (clients != null)
                {
                    _memoryCache.Set(key, clients,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(_saveTime)));
                }
            }
            return  clients;
        }
    }
}
