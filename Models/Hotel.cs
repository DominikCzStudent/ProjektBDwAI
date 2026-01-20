using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
	public class Hotel
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Podanie nazwy jest wymagane.")]
		[StringLength(100, ErrorMessage = "Nazwa może mieć maksymalnie 100 znaków.")]
		[Display(Name = "Nazwa")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Podanie miasta jest wymagane.")]
		[Display(Name = "Miasto")]
		public string City { get; set; }

		public List<Room> Rooms { get; set; } = new();

		public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
	}
}
