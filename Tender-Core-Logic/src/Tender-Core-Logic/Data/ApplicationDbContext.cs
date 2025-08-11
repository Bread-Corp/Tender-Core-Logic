using Microsoft.EntityFrameworkCore;
using Tender_Core_Logic.Models;

namespace Tender_Core_Logic.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BaseTender> Tenders { get; set; }

        //Child Entities
        public DbSet<eTender> eTenders { get; set; }
        public DbSet<EskomTender> EskomTenders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BaseTender>(entity => { entity.ToTable("BaseTender"); });
            modelBuilder.Entity<eTender>(entity => { entity.ToTable("eTender"); });
            modelBuilder.Entity<EskomTender>(entity => { entity.ToTable("EskomTender"); });

            //modelBuilder.Entity<SupportingDoc>().HasNoKey();
        }
    }
}
