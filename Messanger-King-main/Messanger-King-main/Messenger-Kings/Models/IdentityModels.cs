using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Messenger_Kings.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<AccountCategory> AccountCategories { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankCategory> BankCategories { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientCategory> ClientCategories { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Driver> Drivers { get; set; }
  

        public DbSet<Order> Orders { get; set; }
      
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Tracking> Trackings { get; set; }

        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        public DbSet<IdentityUserRole> UserInRole { get; set; }
        public DbSet<ApplicationRole> appRoles { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Waybill> Waybills { get; set; }

        public System.Data.Entity.DbSet<Messenger_Kings.Models.Status> Status { get; set; }

        public System.Data.Entity.DbSet<Messenger_Kings.Models.Spins> Spins { get; set; }

        public System.Data.Entity.DbSet<Messenger_Kings.Models.ClientSignature> ClientSignatures { get; set; }

        public System.Data.Entity.DbSet<Messenger_Kings.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<Messenger_Kings.Models.Application> Applications { get; set; }
    }
}