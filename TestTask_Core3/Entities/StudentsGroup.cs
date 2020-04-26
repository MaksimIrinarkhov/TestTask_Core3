using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask_Core3.Entities
{
    [Table("StudentsGroup")]
    public class StudentsGroup
    {
        [Key, Column(Order = 1)]
        public Guid StudentId { get; set; }

        [Key, Column(Order = 2)]
        public Guid GroupId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [ForeignKey("GroupId")]
        public Group Group { get; set; }
    }
}
