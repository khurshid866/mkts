using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models
{
    public class TaskSource
    {
        [Key]
        public short SourceID { get; set; }
        public string Name { get; set; }
    }
}
