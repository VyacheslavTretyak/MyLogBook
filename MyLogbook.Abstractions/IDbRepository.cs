using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyLogbook.Abstractions
{
    public interface IDbRepository<T> where T:class, IDbEntity
    {
        IQueryable<T> AllItems { get; }
        DbContext Context { get; }
        Task<List<T>> ToListAsync();
        Task<bool> AddItemAsync(T item);
        Task<int> AddItemsAsync(IEnumerable<T> items);
        Task<bool> ChangeItemAsync(T item);
        Task<bool> DeleteItemAsync(Guid id);
        Task<T> GetItemAsync(Guid id);
        Task<int> SaveChangesAsync();
    }
}
