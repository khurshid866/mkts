using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class Partner
    {
        [Key]
        public short PartnerID { get; set; }
        [DisplayName("partner")]
        public string PartnerName { get; set; }
    }
}
