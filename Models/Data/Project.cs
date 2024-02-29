using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models.Data
{
    public class Project
    {
        [Key]
        public short ProjectID { get; set; }
       
        [DisplayName("Project")]
        public string ProjectName { get; set; }

       // public string ProvinceName { get; set; }
    }
}
