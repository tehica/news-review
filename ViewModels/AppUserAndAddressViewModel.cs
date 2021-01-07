using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProgramiranje.Models;

namespace WebProgramiranje.ViewModels
{
    public class AppUserAndAddressViewModel
    {
        public ApplicationUser user { get; set; }
        public Address Address { get; set; }

    }
}
