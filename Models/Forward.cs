using MKTS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace MKTS.Models
{
    public class Forward
    {
        [Key]
        public int ForwardID { get; set; }
        
        [DisplayName("Department")]
        public short DeptID { get; set; }
        public int TaskID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Assign")]
        public DateTime DateAssign { get; set; }
        public string Comments { get; set; }
        public string AttachmentPath { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [NotMapped]
        public bool Resolved { get; set; }
        public virtual Department Department { get; set; }
    }

}
