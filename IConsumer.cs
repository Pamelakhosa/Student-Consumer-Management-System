
using ASPNETCore_DB.Models;

namespace ASPNETCore_DB.Interfaces
{
    public interface IConsumer
    {
        Consumer GetByEmail(string email);
        Consumer GetById(int id);
        Consumer Details(string id);
        void Create(Consumer consumer);
        void Edit(Consumer consumer);
        bool Delete(Consumer consumer);
        IQueryable<Consumer> GetConsumers(string searchString, string sortOrder);
    }
}//end namespace
