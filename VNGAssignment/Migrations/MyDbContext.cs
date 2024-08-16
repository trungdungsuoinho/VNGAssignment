using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VNGAssignment.Entities;
using VNGAssignment.Helpers;

namespace VNGAssignment.Migrations
{
    public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
