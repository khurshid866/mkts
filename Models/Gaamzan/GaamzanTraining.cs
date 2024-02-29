using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Gaamzan
{
    public class GaamzanTraining
    {
        [Key]
        public int TrainingID { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantType { get; set; }
        public string Gender { get; set; }
        public string TrainingTheme { get; set; }
        public string TrainingMode { get; set; }
        public string TrainingHours { get; set; }
        public string District { get; set; }
        public string Partner { get; set; }
        public string SchoolName { get; set; }

        public string Email { get; set; }
        public string Contact { get; set; }

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
