using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExample.Models
{
    public class User : IdentityUser
    {
        public string City { get; set; }
        public string PictureUrl { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }
        public int Gender { get; set; }
    }
}
