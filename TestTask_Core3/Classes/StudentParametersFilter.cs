using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestTask_Core3.Classes
{
    public class StudentParametersFilter : EntityListParameters
    {
        public bool? Sex { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string UniqueId { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
    }
}
