using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Bid> Bids { get; set; }
    }
}
