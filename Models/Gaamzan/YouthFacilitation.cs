using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Gaamzan
{
    public class YouthFacilitation
    {
      public int  YouthFacilitationID {get;set;}
      public string  ParticipantName     {get;set;}
      public string  Gender              {get;set;}
      public string  District            {get;set;}
      public string  ClassBatch          {get;set;}
      public string  ModeFacilitation    {get;set;}
      public string SkillEnhanced { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        [NotMapped]
        public bool IsDuplicate { get; set; }
        [NotMapped]
        public short SerialNo { get; set; }


    }
}
