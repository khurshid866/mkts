using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Gaamzan
{
    public class GaamzanEnrollment
    {
        [Key]
        public int EnrollmentID { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public short Class { get; set; }
        public short AdmnMonth { get; set; }
        public short AdmnDay { get; set; }
        public short AdmnYear { get; set; }
        public string Disability { get; set; }
        public string FatherName { get; set; }
        public string SchoolName { get; set; }
        public string SchoolLevel { get; set; }
        public string District { get; set; }
        public string Tehsil { get; set; }
        public string UnionCouncil { get; set; }
        public string Village { get; set; }
        public string TeacherName { get; set; }
        public string TeacherContact { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        [NotMapped]
        public bool IsDuplicate { get; set; }
        [NotMapped]
        public short SerialNo { get; set; }
    }


    public class GaamzanRawDataCollection
    {
        //public EnrollList()
        //{
        //    Enrollment = new Enrollment();
        //}
        public bool IsRecDup { get; set; }
        public short id { get; set; }
        public List<GaamzanEnrollment> Enrollments { get; set; }
        public List<GaamzanTraining> Training { get; set; }
        public List<SchoolSupported> SchoolSupported { get; set; }
        public List<YouthFacilitation> YouthFacilitation { get; set; }

    }

}
