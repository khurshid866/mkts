using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class Year
    {
        [Key]
        public short ID { get; set; }
        public int YearName { get; set; }
    }
}
