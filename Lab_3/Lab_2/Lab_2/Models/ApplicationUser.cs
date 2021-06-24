using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab_2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Orders> Orders { get; set; }
    }
}
