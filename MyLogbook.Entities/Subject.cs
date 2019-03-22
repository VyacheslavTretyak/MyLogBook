using MyLogbook.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyLogbook.Entities
{
    [Table("subjects")]
    public class Subject:DbEntity
    {
        [Column("name")]
        [StringLength(64)]
        public string Name{ get; set; }
        
        public virtual Faculty Faculty{ get; set; }
		public virtual List<Mark> Marks { get; set; }
    }
}
