using Microsoft.EntityFrameworkCore;
using MyLogbook.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLogbook.Repositories
{
    public class DbRepository<T> : IDbRepository<T> where T : class, IDbEntity
    {
        DbContext _context;

        public DbRepository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<T> AllItems => _context.Set<T>();
        public DbContext Context => _context;

        async public Task<List<T>> ToListAsync()
        {
            return await AllItems.ToAsyncEnumerable<T>().ToList();
        }

        async public Task<bool> AddItemAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
            return await SaveChangesAsync() > 0;
        }

        async public Task<int> AddItemsAsync(IEnumerable<T> items)
        {
            await _context.Set<T>().AddRangeAsync(items);
            return await SaveChangesAsync();
        }

        async public Task<bool> ChangeItemAsync(T item)
        {
            //T changed = await GetItemAsync(item.Id);			
            //if (changed == null) return false;
			//changed = item;	
			_context.Update(item);//add this line
			return await SaveChangesAsync() > 0;

        }

        async public Task<bool> DeleteItemAsync(Guid id)
        {
            T item = await _context.Set<T>().FindAsync(id);
            if (item == null) return false;
            _context.Set<T>().Remove(item);
            return await SaveChangesAsync() > 0;
        }

        async public Task<T> GetItemAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        async public Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
#pragma warning disable 0168
            catch(Exception e)
#pragma warning restore 0168
            {
                return -1;
            }
        }
    }
}
