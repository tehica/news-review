using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebProgramiranje.Extensions;

namespace WebProgramiranje.Models
{
    public class News : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }
        public enum NewsType
        {
            Undefine = 0,
            General = 1,
            Sport = 2
        }
        public string Image { get; set; }
        [NotMapped]
        [FileImgExtension]
        public IFormFile ImageUpload { get; set; }
    }
}
