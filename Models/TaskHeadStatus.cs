using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models
{
    public class TaskHeadStatus
    {
        [Key]
        public short StatusID {get;set;}
        public string Name { get; set; }
    }
}
