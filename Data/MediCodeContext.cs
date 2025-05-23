using MediCode.Models;
using Microsoft.EntityFrameworkCore;

namespace MediCode.Data
{
    public class MediCodeContext : DbContext
    {
        public MediCodeContext(DbContextOptions<MediCodeContext> options)
            : base(options)
        {
        }

        public DbSet<Lekarz> Lekarze { get; set; }
        public DbSet<Pacjent> Pacjenci { get; set; }
        public DbSet<Choroba> Choroby { get; set; }
        public DbSet<Wizyta> Wizyty { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unikalny Pesel dla pacjenta
            modelBuilder.Entity<Pacjent>()
                .HasIndex(p => p.Pesel)
                .IsUnique();

            // Relacja lekarz - pacjent
            modelBuilder.Entity<Pacjent>()
                .HasOne(p => p.DodanyPrzez)
                .WithMany(l => l.DodaniPacjenci)
                .HasForeignKey(p => p.DodanyPrzezId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relacja lekarz - wizyta
            modelBuilder.Entity<Wizyta>()
                .HasOne(w => w.Lekarz)
                .WithMany(l => l.Wizyty)
                .HasForeignKey(w => w.LekarzId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacja pacjent - wizyta
            modelBuilder.Entity<Wizyta>()
                .HasOne(w => w.Pacjent)
                .WithMany(p => p.Wizyty)
                .HasForeignKey(w => w.PacjentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relacja pacjent - choroba
            modelBuilder.Entity<Choroba>()
                .HasOne(w => w.Pacjent)
                .WithMany(p => p.Choroby)
                .HasForeignKey(w => w.PacjentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
