using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.View
{
    public class GenderSP
    {
        [Key]
        public string name { get; set; }
        public int? Boys { get; set; }
        public int? Girls { get; set; }
        public int? CoEducation { get;set;}

    }
}
