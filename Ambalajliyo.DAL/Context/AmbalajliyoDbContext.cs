using Ambalajliyo.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ambalajliyo.DAL.Context
{
    /// <summary>
    /// Represents the database context for the application, inheriting from IdentityDbContext for user management.
    /// </summary>
    public class AmbalajliyoDbContext : IdentityDbContext<AmbalajliyoUser, AmbalajliyoRole, string>
    {
        public AmbalajliyoDbContext(DbContextOptions<AmbalajliyoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AmbalajliyoRole> AmbalajliyoRoles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }

        /// <summary>
        /// Configures the model relationships and seeding data for roles and users.
        /// </summary>
        /// <param name="builder">The model builder to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Generate GUIDs for roles and users
            var adminRoleId = Guid.NewGuid().ToString();
            var contentManagerRoleId = Guid.NewGuid().ToString();
            var customerServiceRoleId = Guid.NewGuid().ToString();

            var adminUserId = Guid.NewGuid().ToString();
            var contentManagerUserId = Guid.NewGuid().ToString();
            var customerServiceUserId = Guid.NewGuid().ToString();

            // Define roles
            var adminRole = new AmbalajliyoRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" };
            var contentManagerRole = new AmbalajliyoRole { Id = contentManagerRoleId, Name = "Content Manager", NormalizedName = "CONTENT MANAGER" };
            var customerServiceRole = new AmbalajliyoRole { Id = customerServiceRoleId, Name = "Customer Service", NormalizedName = "CUSTOMER SERVICE" };

            builder.Entity<AmbalajliyoRole>().HasData(adminRole, contentManagerRole, customerServiceRole);

            // Define users
            var adminUser = new AmbalajliyoUser
            {
                Id = adminUserId,
                UserName = "admin",
                Email = "busenur@example.com",
                Name = "Busenur",
                Surname = "Eğerci",
                IsAdmin = true,
                IsDeleted = false,
                AmbalajliyoRoleId = adminRole.Id
            };

            var contentManagerUser = new AmbalajliyoUser
            {
                Id = contentManagerUserId,
                UserName = "contentmanager",
                Email = "ummuhan@example.com",
                Name = "Ümmühan",
                Surname = "Erdoğan",
                IsAdmin = false,
                IsDeleted = false,
                AmbalajliyoRoleId = contentManagerRole.Id
            };

            var customerServiceUser = new AmbalajliyoUser
            {
                Id = customerServiceUserId,
                UserName = "customerservice",
                Email = "sinan@example.com",
                Name = "Sinan",
                Surname = "Kaya",
                IsAdmin = false,
                IsDeleted = false,
                AmbalajliyoRoleId = customerServiceRole.Id
            };

            // Hash passwords
            var hasher = new PasswordHasher<AmbalajliyoUser>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");
            contentManagerUser.PasswordHash = hasher.HashPassword(contentManagerUser, "Admin123!");
            customerServiceUser.PasswordHash = hasher.HashPassword(customerServiceUser, "Admin123!");

            builder.Entity<AmbalajliyoUser>().HasData(adminUser, contentManagerUser, customerServiceUser);

            // Define user-role relationships
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = adminUserId, RoleId = adminRoleId },
                new IdentityUserRole<string> { UserId = contentManagerUserId, RoleId = contentManagerRoleId },
                new IdentityUserRole<string> { UserId = customerServiceUserId, RoleId = customerServiceRoleId }
            );

            // Configure relationships
            builder.Entity<AmbalajliyoUser>()
                .HasOne(u => u.AmbalajliyoRole)
                .WithMany(r => r.AmbalajliyoUsers)
                .HasForeignKey(u => u.AmbalajliyoRoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
