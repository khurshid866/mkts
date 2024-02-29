using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class IndicatorTrackUnverify
    {
        [Key]
       public int Id { get; set; }
       public int  SchoolID     {get;set;}
       public int  IndicatorID  {get;set;}
       public DateTime  Datetime     {get;set;}
       public string  MachineName  {get;set;}
       public string  MacAddress   {get;set;}

    }
}
