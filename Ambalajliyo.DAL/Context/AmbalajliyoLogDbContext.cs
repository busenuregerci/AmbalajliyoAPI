using Ambalajliyo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ambalajliyo.DAL.Context
{
    /// <summary>
    /// Represents the database context for logging entries.
    /// </summary>
    public class AmbalajliyoLogDbContext : DbContext
    {
        public AmbalajliyoLogDbContext(DbContextOptions<AmbalajliyoLogDbContext> options)
            : base(options)
        {
        }

        public DbSet<LogEntry> Logs { get; set; }

        /// <summary>
        /// Configures the model for log entries.
        /// </summary>
        /// <param name="modelBuilder">The model builder to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>().ToTable("Logs");
            base.OnModelCreating(modelBuilder);
        }
    }
}
