using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Gaamzan
{
    public class SchoolSupported
    {
        [Key]
        public int SchoolSupportID { get; set; }
        public string SchoolName { get; set; }
        public string SchoolType          {get;set;}
        public string SchoolLevel         {get;set;}
        public string District            {get;set;}
        public string Partner             {get;set;}
        public short    Year                {get;set;}
        public short    Washroom            {get;set;}
        public short    SolarPanel          {get;set;}
        public short    WaterTank           {get;set;}
        public short    PlasticMat          {get;set;}
        public short    WaterCooler         {get;set;}
        public short    BlackBoard          {get;set;}
        public short    TeacherChair        {get;set;}
        public short    Registar            {get;set;}
        public short    Shelter             {get;set;}
        public short    LearningMaterial    {get;set;}
        public short    LCDs                {get;set;}
        public short    Tablets             {get;set;}

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
