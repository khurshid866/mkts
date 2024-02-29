using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class School
    {
        [Key]
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public string Type { get; set; }
        public string SLevel { get; set; }
        public string Gender { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string UC { get; set; }

    }
}
