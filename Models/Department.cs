using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models
{
    public class Department
    {
        [Key]
        public short DeptID { get; set; }
        public string Name { get; set; }
        public short SupervisorID { get; set; }
    }
}
