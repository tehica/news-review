using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProgramiranje.Models;

namespace WebProgramiranje.ViewModels
{
    public class NewsCommentsAndUsersViewModel
    {
        public News News { get; set; }
        public IEnumerable<Comments> Comments { get; set; }

        public Comments Comment { get; set; }

    }
}
