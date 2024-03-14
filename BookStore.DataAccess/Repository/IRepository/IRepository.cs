using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - is a generic class which can be Categroy, Oderer, Customer or any other table
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null); //This method is designed to retrieve all entities of type T from the underlying data source.
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false); // the track parameter is to avoid automaticaly tracking By EF Core
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
