
using BookStore.Data;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;


namespace BookStore.DataAccess.Repository
{
    public class Repository <T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> dbSet;
        private static readonly char[] commaSeparator = { ',' };

        public Repository(ApplicationDBContext db)
        {
           _db = db;
            dbSet= db.Set<T>();
            _db.ProductTable.Include(u=>u.Category); //category will be autometicly populated based on the foreignkey 

            // foreign key diye base table er onno property access korte chaile just .include(u=>u.Property1).include(u=>u.property2) evabe add kore dilei hobe
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }


        public T Get(Expression<Func<T, bool>>? filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;

            if (tracked) // to aboid tracking by EF Core
            {
                query = dbSet; // here we assign the complete DBset to the query        
            }
            else
            {
                query = dbSet.AsNoTracking();  //this AsNoTracking stop auto Tracking in EF core
                // here we assign the complete DBset to the query
            }

            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var i in includeProperties.Split(commaSeparator, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }

            return query.FirstOrDefault();


        }
        //here includeProperties will recive coma seperated value
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
         
            IQueryable<T> query = dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var i in includeProperties.Split(commaSeparator,StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
                
            }
            return query.ToList();

        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }


        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }


    }
}
