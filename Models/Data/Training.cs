using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class Training
    {
        [Key]
      public int  TrainingID            {get;set;}
      public string  ProvinceName          {get;set;}
      public string  DistrictName          {get;set;}
      public string  ThemeName             {get;set;}
      public string  Partner             {get;set; }
        public string  TrainingType          { get;set;}
      public string  GroupName             {get;set;}
      public short  Duration              {get;set;}
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate             {get;set;}
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime  EndDate               {get;set;}
      public string  ConductedBy           {get;set;}
      public string  ParticipantType1      {get;set;}
      public short  Male1                 {get;set;}
      public short  Female1               {get;set;}
      public string  ParticipantType2      {get;set;}
      public short  Male2                 {get;set;}
      public short Female2 { get; set; }

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
