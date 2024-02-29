using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class TrainingTheme
    {
        [Key]
        public short ID { get; set; }
        [DisplayName("Theme Name")]
        public string ThemeName { get; set; }
    }
}
