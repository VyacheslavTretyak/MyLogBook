using MyLogbook.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MyLogbook.Entities
{
    [Table("marks")]
    public class Mark:DbEntity
    {
        [Column("value")]        
        public int Value{ get; set; }

		[DataType(DataType.Date)]
		[Column("date")]        
        public DateTime Date{ get; set; }

		public virtual Subject Subject { get; set; }
		public virtual Teacher Teacher { get; set; }
		public virtual Student Student { get; set; }


	}
}
