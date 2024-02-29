using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class TypeOfParticipant
    {
        [Key]
        public short ID { get; set; }
        public string Participant { get; set; }
    }
}
