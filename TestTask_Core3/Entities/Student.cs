using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask_Core3.Entities
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public bool Sex { get; set; }

        [Required]
        [MaxLength(40)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(40)]
        public string FirstName { get; set; }

        [MaxLength(60)]
        public string MiddleName { get; set; } = string.Empty;

        [MinLength(6), MaxLength(16)]
        public string UniqueId { get; set; } = string.Empty;

        public ICollection<StudentsGroup> StudentsGroups { get; set; }


        public string SexStr => Sex ? "муж." : "жен.";
    }
}
