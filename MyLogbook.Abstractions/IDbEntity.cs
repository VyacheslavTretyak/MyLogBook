using System;
using System.ComponentModel.DataAnnotations;

namespace MyLogbook.Abstractions
{
    public interface IDbEntity
    {
        [Key]
        Guid Id { get; set; }
    }
}
