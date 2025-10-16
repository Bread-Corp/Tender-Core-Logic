using Microsoft.EntityFrameworkCore;
using Tender_Core_Logic.Models;
using Tender_Core_Logic.UserModels;

namespace Tender_Core_Logic.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BaseTender> Tenders { get; set; }
        public DbSet<TenderUser> Users { get; set; }

        //Child Entities
        public DbSet<eTender> eTenders { get; set; }
        public DbSet<EskomTender> EskomTenders { get; set; }
        public DbSet<SanralTender> SanralTenders { get; set; }
        public DbSet<TransnetTender> TransnetTenders { get; set; }
        public DbSet<SarsTender> SarsTenders { get; set; }

        //User Child Entities
        public DbSet<StandardUser> StandardUsers { get; set; }
        public DbSet<SuperUser> SuperUsers { get; set; }

        //Bridge tables -- manual creation
        public DbSet<User_Tender> User_Tenders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //base tender
            modelBuilder.Entity<BaseTender>(entity => { entity.ToTable("BaseTender"); });
            //children
            modelBuilder.Entity<eTender>(entity => { entity.ToTable("eTender"); });
            modelBuilder.Entity<EskomTender>(entity => { entity.ToTable("EskomTender"); });
            modelBuilder.Entity<SanralTender>(entity => { entity.ToTable("SanralTender"); });
            modelBuilder.Entity<TransnetTender>(entity => { entity.ToTable("TransnetTender"); });
            modelBuilder.Entity<SarsTender>(entity => { entity.ToTable("SarsTender"); });
            //user specific
            modelBuilder.Entity<TenderUser>(entity => { entity.ToTable("TenderUser"); });
            modelBuilder.Entity<StandardUser>(entity => { entity.ToTable("StandardUser"); });
            modelBuilder.Entity<SuperUser>(entity => { entity.ToTable("SuperUser"); });
            modelBuilder.Entity<User_Tender>(entity => { entity.ToTable("User_Tender"); });

            //Ensures no duplicates of watchlist entries by checking if foreign keys are unique
            modelBuilder.Entity<User_Tender>()
                .HasIndex(uw => new { uw.FKTenderID, uw.FKUserID })
                .IsUnique();

            //Indexing for watchlist calls or calls for analytics
            modelBuilder.Entity<User_Tender>()
                .HasIndex(uw => uw.FKUserID);

            modelBuilder.Entity<User_Tender>()
                .HasIndex(uw => uw.FKTenderID);

            //modelBuilder.Entity<SupportingDoc>().HasNoKey();
        }
    }
}
