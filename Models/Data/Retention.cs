using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class Retention
    {
      [Key]
      public  int RetentionID  {get;set;}
      public  string Province     {get;set;}
      public  string Partner      {get;set;}
      public  int Year         {get;set;}
      public  int DropoutBoys      {get;set;}
      public  int DropoutGirls { get;set;}

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
       

        [NotMapped]
    public int SerialNo { get; set; }
        [NotMapped]
        public bool IsDuplicate { get; set; }

    }
}
