using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class Enrollment
    {
        [Key]
        [DisplayName("Serial No.")]
        public int EnrollmentID { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public int Year { get; set; }
        public string Season { get; set; }
        [DisplayName("Type of Education")]
        public string EducationType { get; set; }
        public string Partner { get; set; }
        public short Class { get; set; }
        [DisplayName("New Enrolled Boys")]
        public int EnrolledBoys { get; set; }
        [DisplayName("New Enrolled Girls")]
        public int EnrolledGirls { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        [NotMapped]
        public bool IsDuplicate { get; set; }
        [NotMapped]
        public short SerialNo { get; set; }

    }
    public class RawDataCollection
    {
        //public EnrollList()
        //{
        //    Enrollment = new Enrollment();
        //}
        public bool IsRecDup { get; set; }
        public short id { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Retention> Retentions { get; set; }
        public List<SchoolSupply> SchoolSupplies { get; set; }
        public List<Training> Trainings { get; set; }

    }
}
