using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.View
{
    public class LevelSP
    {
        [Key]
        public string name { get; set; }
        public int? Primary { get; set; }
        public int? Middle { get; set; }
        public int? High { get; set; }

    }
}
