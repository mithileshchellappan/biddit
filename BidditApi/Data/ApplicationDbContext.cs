using Microsoft.EntityFrameworkCore;
using BidditApi.Models;

namespace BidditApi.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base (options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Art> Arts { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<UserBids> UserBids { get; set; }


    }
}
