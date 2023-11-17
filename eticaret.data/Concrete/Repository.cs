using eticaret.data.Abstract;
using eticaret.data.Contexts;
using eticaret.entity.BaseEntity;
using eticaret.entity.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Concrete
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public readonly ETicaretDbContext _context;

        public Repository(ETicaretDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Table => _context.Set<T>();
        public ETicaretDbContext Context => _context;

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> entityEntry = await Table.AddAsync(model);
            return entityEntry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<T> datas)
        {
            await Table.AddRangeAsync(datas);
            return true;
        }

        public IQueryable<T> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking) { query = query.AsNoTracking(); }
            return query;
        }

        public async Task<T> GetByIdAsync(string id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking) { query = query.AsNoTracking(); }
            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking) { query = query.AsNoTracking(); }
            return await query.FirstOrDefaultAsync(method);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking) { query = query.AsNoTracking(); }
            return query;
        }

        public bool Remove(T model)
        {
            EntityEntry<T> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            T? model = await Table.
                FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
            if (model != null)
                return Remove(model);
            else
                return false;
        }

        public bool RemoveRange(List<T> model)
        {
            Table.RemoveRange(model);
            return true;
        }

        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();

        public int Save()
            => _context.SaveChanges();

        public bool Update(T model)
        {
            EntityEntry entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
