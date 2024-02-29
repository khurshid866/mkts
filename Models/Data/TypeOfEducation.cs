using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class TypeOfEducation
    {
        [Key]
        public string TypeEducation { get; set; }
    }
}
