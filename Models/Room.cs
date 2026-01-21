using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt.Models
{
	[Display(Name = "Pokój")]
	public class Room
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Numer pokoju jest wymagany.")]
		[Display(Name = "Numer")]
		public string Number { get; set; }

		[Required(ErrorMessage = "Podanie pojemności jest wymagane.")]
		[Range(1, 10, ErrorMessage = "Pojemność musi mieścić się w zakresie od 1 do 10.")]
		[Display(Name = "Pojemność")]
		public int Capacity { get; set; }

		[Required(ErrorMessage = "Podanie ceny za noc jest wymagane.")]
		[Range(0, 10000, ErrorMessage = "Cena za noc musi być z zakresu 0–10 000.")]
		[Column(TypeName = "decimal(10,2)")]
		[Display(Name = "Cena za noc")]
		public decimal PricePerNight { get; set; }

		[Display(Name = "Id hotelu")]
		public int HotelId { get; set; }

		[Display(Name = "Hotel")]
		public Hotel? Hotel { get; set; }

		public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
	}
}
