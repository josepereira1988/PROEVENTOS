using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Models;

namespace ProEventos.Presistence.Data
{
    public class ProEventosContext : IdentityDbContext<User, Role, int, 
                                        IdentityUserClaim<int>,UserRole, 
                                        IdentityUserLogin<int>, IdentityRoleClaim<int>, 
                                        IdentityUserToken<int>>
    {
        //private IConfiguration _configuration;
        public ProEventosContext(DbContextOptions<ProEventosContext> options) : base(options) 
        {
        }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento > PalestrantesEventos { get; set; }
        public DbSet<RedeSocial> RedesSociais { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(u => {
                u.HasKey(ur => new {ur.UserId,ur.RoleId});
                u.HasOne(ur => ur.Role).WithMany(r => r.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired();
            });

            modelBuilder.Entity<PalestranteEvento>().HasKey(PE => new {PE.EventoId,PE.PalestranteId});
            modelBuilder.Entity<Evento>()
            .HasMany(e => e.RedesSociais)
            .WithOne(rs => rs.Evento )
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Palestrante>()
            .HasMany(e => e.RedesSociais)
            .WithOne(rs => rs.Palestrante)
            .OnDelete(DeleteBehavior.Cascade);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "server=localhost;port=3306;database=ProEventos;user=ajc;password=paralela";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            
        }
    }
}