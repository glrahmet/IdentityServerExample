using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Models
{
    public class IdentityDbContextManager : IdentityDbContext<User>
    {
        public IdentityDbContextManager(DbContextOptions<IdentityDbContextManager> options) : base(options)
        {

        }
    }
}
