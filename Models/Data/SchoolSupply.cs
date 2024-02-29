using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class SchoolSupply
    {
        [Key]

        public int SSID { get; set; }
        public string SchoolName { get; set; }
        public string SchoolType { get; set; }
        public string SchoolLevel { get; set; }
        public string Gender { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Partner { get; set; }
        public short Year { get; set; }
        public short WashroomRepair { get; set; }
        public short ConstructionWall { get; set; }
        public short ConstructionWashroom { get; set; }
        public short InstallationHandpump { get; set; }
        public short RefurbishmentSchool { get; set; }
        public short InstallationSolarPanel { get; set; }
        public short RepairSchoolGate { get; set; }
        public short InstallationFiltrationPlant { get; set; }
        public short WhiteWash { get; set; }
        public short ElectricRepairments { get; set; }
        public short Flooring { get; set; }
        public short PlasticMat { get; set; }
        public short WaterCooler { get; set; }
        public short BlackBoard { get; set; }
        public short TeacherChair { get; set; }
        public short AttendanceRegister { get; set; }
        public short WaterTank { get; set; }
        public short Desk { get; set; }
        public short CupboardRack { get; set; }
        public short ElectricCooler { get; set; }

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
