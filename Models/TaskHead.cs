using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models
{
    public class TaskHead
    {
        [Key]
        public int TaskID { get; set; }
        [ForeignKey("TaskSource")]
        [DisplayName("Source")]
        public short SourceID { get; set; }
        
        [ForeignKey("TaskType")]
        [DisplayName("Type")] 
        public short TypeID { get; set; }
        
        [DisplayName("Status")]
        [ForeignKey("TaskHeadStatus")]
        public short StatusID { get; set; }
        
        [ForeignKey("Department")] 
        public short DeptID { get; set; }
        public string Title { get; set; }

        [DisplayName("Reference")]
        public string ReferenceNo { get; set; }
        public string Detail { get; set; }

        [DisplayName("Performance")]
        [ForeignKey("Performance")] 
        public short PerformanceID { get; set; }
        [DisplayName("Attachment")] 
        public string attachmentPath { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Assign")]
        public DateTime DateAssign { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Dead Line")]
        public DateTime DateDeadline { get; set; }

        public int ForwardCount { get; set; }
        public int CurForwardDeptID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Completion")]
        public DateTime? DateCompletion { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }

        public virtual TaskSource TaskSource { get; set; }
        public virtual TaskType TaskType { get; set; }
        public virtual TaskHeadStatus TaskHeadStatus { get; set; }
        public virtual Department Department { get; set; }

        public virtual Performance Performance {get;set;}
    }
    
    public class TaskforwardList
    {
        public TaskHead TaskHead { get; set; }
        public Forward Forward { get; set; }
    }

}
