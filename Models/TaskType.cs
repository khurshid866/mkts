using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models
{
    public class TaskType
    {
        [Key]
        public short TypeID { get; set; }
        public string Name { get; set; }
    }
}
