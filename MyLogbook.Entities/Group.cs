using MyLogbook.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyLogbook.Entities
{
    [Table("groups")]
    public class Group : DbEntity
    {
        [Column("name")]
        [StringLength(64)]
        public string Name { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual List<Student> Students { get; set; }
    }
}
