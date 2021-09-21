using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreRoleBased.Models.ViewModel;

namespace CoreRoleBased.Models
{
    public class ApplicationContext:IdentityDbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        //public DbSet<CoreRoleBased.Models.ViewModel.SignUpViewModel> SignUpViewModel { get; set; }
    }
}
