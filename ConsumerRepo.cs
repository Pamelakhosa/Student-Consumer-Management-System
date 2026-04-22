using ASPNETCore_DB.Data;
using ASPNETCore_DB.Interfaces;
using ASPNETCore_DB.Models;

namespace ASPNETCore_DB.Repositories
{
    public class ConsumerRepo : IConsumer
    {
        private readonly SQLiteDBContext _context;

        public ConsumerRepo(SQLiteDBContext context)
        {
            _context = context;
        }

        public void Create(Consumer consumer)
        {
            _context.Consumers.Add(consumer);
            _context.SaveChanges();
        }
        public bool IsExist(string id)
        {
            bool isExist = false;
            Consumer existConsumer = Details(id);
            if (existConsumer == null)
            {
                isExist = true;
            }
            return isExist;
        }

        public bool Delete(Consumer consumer)
        {
            _context.Remove(consumer);
            _context.SaveChanges();
            return IsExist(consumer.Email);
        }

        public Consumer Details(string id)
        {
            var consumer = _context.Consumers?.FirstOrDefault(x => x.FullName == id);
            return consumer;
        }

        public void Edit(Consumer consumer)
        {
            _context.Consumers.Update(consumer);
            _context.SaveChanges();
        }

        public IQueryable<Consumer> GetConsumers(string searchString, string sortOrder)
        {
            var consumer = _context.Consumers
               .ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                consumer = consumer.Where(s => s.FullName.Contains(searchString)).ToList();
            }
            switch (sortOrder)
            {
                case "number_desc":
                    consumer = consumer.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "name_desc":
                    consumer = consumer.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "Date":
                    consumer = consumer.OrderBy(s => s.FullName).ToList();
                    break;
                case "date_desc":
                    consumer = consumer.OrderByDescending(s => s.FullName).ToList();
                    break;
                default:
                    consumer = consumer.OrderBy(s => s.FullName).ToList();
                    break;
            }

            return consumer.AsQueryable();
        }

        public Consumer GetByEmail(string email)
        {
            return _context.Consumers.FirstOrDefault(c => c.Email == email);
        }

        public Consumer GetById(int id)
        {
            return _context.Consumers.FirstOrDefault(c => c.ConsumerId == id);
        }
    }//end class
}//end namespace
