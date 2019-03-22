using Microsoft.EntityFrameworkCore;
using MyLogbook.Abstractions;
using MyLogbook.AppContext;
using MyLogbook.Entities;


namespace MyLogbook.Repositories
{
    public class MarkRepository : DbRepository<Mark>, IMarkRepository
    {
        public MarkRepository(AppDbContext context) : base(context)
        {
        }
    }
}
