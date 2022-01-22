using Meloman_clone.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meloman_clone.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookDescription> BookDescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>(entity => entity.HasIndex(e => e.Isbn).IsUnique());
            builder.Entity<BookAuthor>(entity => entity.HasIndex(e => e.Name).IsUnique());
        }
    }
}
