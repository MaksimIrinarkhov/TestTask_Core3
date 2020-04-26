using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TestTask_Core3.Classes
{
    public class GroupParametersFilter : EntityListParameters
    {
        public string Name { get; set; } = string.Empty;
    }
}
