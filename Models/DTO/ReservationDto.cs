namespace Projekt.Models.DTO
{
	public class ReservationDto
	{
		public int Id { get; set; }

		public DateOnly FromDate { get; set; }
		public DateOnly ToDate { get; set; }

		public int RoomId { get; set; }
		public string RoomNumber { get; set; }

		public int HotelId { get; set; }
		public string HotelName { get; set; }

		public string UserId { get; set; }
	}
}
