using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class Supply
    {
        [Key]
      public short  SupplyID {get;set;}
      public bool  IsSupply {get;set;}
      public string Name { get; set; }


    }
}
