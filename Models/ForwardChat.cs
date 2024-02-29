using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MKTS.Models
{
    public class ForwardChat
    {
        [Key]
        public int ChatID { get; set; }
        public int ForwardID { get; set; }
        public string Comments { get; set; }
        public string AttachmentPath { get; set; }

        public virtual Forward Forward { get; set; }
    }
}
