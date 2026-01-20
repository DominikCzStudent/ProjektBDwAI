using System.ComponentModel.DataAnnotations;

namespace Projekt.Models
{
	public class Reservation : IValidatableObject
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Pole „Data od” jest wymagane.")]
		[Display(Name = "Data od")]
		public DateOnly FromDate { get; set; }

		[Required(ErrorMessage = "Pole „Data do” jest wymagane.")]
		[Display(Name = "Data do")]
		public DateOnly ToDate { get; set; }

		[Required(ErrorMessage = "Wybór hotelu jest wymagany.")]
		[Display(Name = "Hotel")]
		public int HotelId { get; set; }
		public Hotel? Hotel { get; set; }

		[Required(ErrorMessage = "Wybór pokoju jest wymagany.")]
		[Display(Name = "Pokój")]
		public int RoomId { get; set; }
		public Room? Room { get; set; }

		[Display(Name = "Id użytkownika")]
		public string? UserId { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if (ToDate <= FromDate)
			{
				yield return new ValidationResult(
					"Data zakończenia musi być późniejsza niż data rozpoczęcia (minimum 1 noc).",
					new[] { nameof(ToDate) }
				);
			}
		}
	}
}
