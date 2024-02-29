using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models
{
    public class CheckDuplicate
    {
        [Key]
        public int? Id { get; set; }
       // public string ReId { get; set; }
    }

    //public class CheckRetention
    //{
    //    [Key]
    //    public string ReId { get; set; }
    //}
}
