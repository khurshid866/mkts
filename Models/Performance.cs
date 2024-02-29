using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models
{
    public class Performance
    {
        [Key]
        public short PerformanceID { get; set; }

        public string Name { get; set; }
    }
}
