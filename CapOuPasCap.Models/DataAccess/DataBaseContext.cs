using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CapOuPasCap.Models.Classes;


namespace CapOuPasCap.Models.DataAccess
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options) {

            this.Database.EnsureCreated();

        }
        public DbSet<Utilisateur> Utilisateur { get; set; }
        public DbSet<Defi> Defi { get; set; }
        public DbSet<Commentaire> Commentaire { get; set; }
        public DbSet<DefiRealise> DefiRealise { get; set; }
        public DbSet<Like> Like { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Utilisateur>()
                .HasIndex(u => u.Pseudo)
                .IsUnique();

            builder.Entity<Commentaire>()
                .HasOne(u => u.Createur)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
