using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class District
    {
        [Key]
        public short DistrictID { get; set; }
        [DisplayName("Disrict")]
        public string  DistrictName { get; set; }
        //public string ProjectID { get; set; }
        public string ProvinceName { get; set; }
        //public virtual Province Provinces { get; set; }

    }
}
