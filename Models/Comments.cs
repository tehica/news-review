using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebProgramiranje.Models
{
    public class Comments : BaseEntity
    {
        [Required]
        public string Text { get; set; }

        // ForeignKey
        [Display(Name = "News")]
        public int NewsId { get; set; }
        [ForeignKey("NewsId")]
        public virtual News News { get; set; }

        // ForeignKey
        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
