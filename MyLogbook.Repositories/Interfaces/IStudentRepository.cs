using MyLogbook.Abstractions;
using MyLogbook.Entities;

namespace MyLogbook.Repositories
{
    public interface IStudentRepository:IDbRepository<Student>
    {
    }
}
