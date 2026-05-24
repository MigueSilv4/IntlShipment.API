using IntlShipment.Models;
using Microsoft.EntityFrameworkCore;


namespace IntlShipment.Data
{
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            public DbSet<Shipment> Shipments { get; set; }

            public DbSet<User> Users { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Shipment>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.NumeroGuia).HasMaxLength(50);
                    entity.Property(e => e.PaisOrigen).HasMaxLength(100);
                    entity.Property(e => e.PaisDestino).HasMaxLength(100);
                    entity.Property(e => e.CiudadOrigen).HasMaxLength(100);
                    entity.Property(e => e.CiudadDestino).HasMaxLength(100);
                    entity.Property(e => e.NombreRemitente).HasMaxLength(150);
                    entity.Property(e => e.NombreDestinatario).HasMaxLength(150);
                    entity.Property(e => e.DescripcionMercancia).HasMaxLength(500);
                    entity.Property(e => e.PesoKg).HasColumnType("decimal(10,2)");
                    entity.Property(e => e.Estado).HasMaxLength(50);
                    entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETUTCDATE()");
                    entity.Property(e => e.FechaEstimadaEntrega).HasDefaultValueSql("GETUTCDATE()");

                });

                modelBuilder.Entity<User>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Email).HasMaxLength(100);
                    entity.Property(e => e.Password).HasMaxLength(255);
                    entity.Property(e => e.Rol).HasMaxLength(50);
                });
            }
        }
}


