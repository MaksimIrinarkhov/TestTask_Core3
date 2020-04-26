using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask_Core3.DTOs
{
    public class StudentDTO
    {
        public Guid Id { get; set; }
        public bool Sex { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; } = string.Empty;
        public string UniqueId { get; set; } = string.Empty;
        public IEnumerable<string> Groups { get; set; }
    }
}
