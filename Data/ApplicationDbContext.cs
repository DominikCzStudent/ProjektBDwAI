using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Projekt.Models;

namespace Projekt.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Hotel> Hotels { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<Reservation> Reservations { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Reservation>()
				.Property(r => r.FromDate)
				.HasConversion(
					d => d.ToDateTime(TimeOnly.MinValue),
					d => DateOnly.FromDateTime(d)
				);

			modelBuilder.Entity<Reservation>()
				.Property(r => r.ToDate)
				.HasConversion(
					d => d.ToDateTime(TimeOnly.MinValue),
					d => DateOnly.FromDateTime(d)
				);

			modelBuilder.Entity<Reservation>()
				.HasOne(r => r.Hotel)
				.WithMany(h => h.Reservations)
				.HasForeignKey(r => r.HotelId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
