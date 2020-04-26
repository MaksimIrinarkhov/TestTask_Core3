using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask_Core3.DTOs
{
    public class GroupDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int StudentsCount { get; set; }
    }
}
