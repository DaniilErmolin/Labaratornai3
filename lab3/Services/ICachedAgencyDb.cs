using lab3.Models;

namespace lab3.Services
{
    public interface ICachedAgencyDb
    {
        void AddClientToCache(string key, int rowsNumber = 20);
        IEnumerable<Client> GetClient(string key, int rowsNumber = 20);
    }
}
