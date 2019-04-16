using Microsoft.EntityFrameworkCore;
using StationApplication.Entity;
using StationApplication.Entity.Entities;
using System.Collections.Generic;
using System.Linq;

namespace StationApplication.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        int AddSmartTicket(SmartTicket smartTicket);

    }
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;
        private readonly DbSet<T> dbSet;
        public Repository(ApplicationDbContext _context)
        {
            context = _context;
            dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }
        public void AddRange(IEnumerable<T>entities)
        {
            dbSet.AddRange(entities);
            context.SaveChanges();
        }
        public void Update(T entity)
        {
            dbSet.Update(entity);
            context.SaveChanges();
        }
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
            context.SaveChanges();
        }
        public IQueryable<T> GetAll()
        {
            return dbSet;
        }
        public int AddSmartTicket(SmartTicket smartTicket)
        {
            context.Add(smartTicket);
            context.SaveChanges();
            return smartTicket.Id;
        }
    }
}
