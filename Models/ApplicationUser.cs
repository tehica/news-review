using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebProgramiranje.Extensions;

namespace WebProgramiranje.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public string Gender { get; set; }

        public Address Address { get; set; }


        public enum UserGender
        {
            Unknown = 0,
            Male = 1,
            Female = 2
        }
    }
}
