using Microsoft.AspNetCore.Identity;

namespace Projekt.Models
{
	public class ApplicationUser : IdentityUser
	{
		public ICollection<Reservation> Reservations { get; set; }
	}
}
