using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Meloman_clone.Models;
using Microsoft.EntityFrameworkCore;

namespace Meloman_clone.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) :base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });
        }
    }
}
