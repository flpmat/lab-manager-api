using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Contexts
{
    public class AppContext : DbContext
    {
        public virtual DbSet<Cluster> Cluster { get; set; }

        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

       
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new SegSistemaMap());
        }
    }
}
