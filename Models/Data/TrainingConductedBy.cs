using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class TrainingConductedBy
    {
        [Key]
        public short ID { get; set; }
        [DisplayName("Conducted by")]
        public string ConductedBy { get; set; }
    }
}
