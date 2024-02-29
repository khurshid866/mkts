using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class Season
    {
        [Key]
        public short SeasonID { get; set; }
        [DisplayName("Season")]
        public string SeasonName { get; set; }
    }
}
