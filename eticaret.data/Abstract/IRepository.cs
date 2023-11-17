using eticaret.data.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Abstract
{
    public interface IRepository<T> where T: class
    {
        // Reading
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
        Task<T> GetByIdAsync(string id, bool tracking = true);

        // Writing
        Task<bool> AddAsync(T model);
        Task<bool> AddRangeAsync(List<T> datas);
        bool Remove(T model);
        Task<bool> RemoveAsync(string id);
        bool RemoveRange(List<T> model);
        bool Update(T model);
        Task<int> SaveAsync();
        int Save();
        DbSet<T> Table { get; }
        ETicaretDbContext Context { get; }
    }
}
