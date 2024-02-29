using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.View
{
    public class ProvinceSupply
    {
        [Key]
        public string Name { get; set; }
        public int? AJK { get; set; }
        public int? Balochistan { get; set; }
        public int? GB { get; set; }
        public int? ICT { get; set; }
        public int? KP { get; set; }
        public int? Punjab { get; set; }
        public int? Sindh { get; set; }
    }
}
