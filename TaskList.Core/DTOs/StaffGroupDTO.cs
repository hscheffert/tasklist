using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace TaskList.Core.DTOs
{
    public class StaffGroupDTO
    {
        public string StaffTypeName { get; set; }
        public IEnumerable<string?> StaffNames { get; set; }
        public int Count { get; set; }
    }
}
