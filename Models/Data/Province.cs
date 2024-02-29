using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class Province
    {
        [Key]
        public short ProvinceID { get; set; }
        [DisplayName("Province")]
        public string ProvinceName { get; set; }
    }
}
