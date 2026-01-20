using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using Projekt.Models.DTO;

namespace Projekt.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ReservationsApiController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

        public ReservationsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var query = _context.Reservations
				.Include(r => r.Room)
				.ThenInclude(r => r.Hotel)
				.AsQueryable();

			if (!User.IsInRole("Admin"))
			{
				query = query.Where(r => r.UserId == userId);
			}

			var data = await query
				.Include(r => r.Room)
				.ThenInclude(r => r.Hotel)
				.Select(r => new ReservationDto
				{
					Id = r.Id,
					FromDate = r.FromDate,
					ToDate = r.ToDate,
					RoomId = r.RoomId,
					RoomNumber = r.Room.Number,
					HotelId = r.Room.Hotel.Id,
					HotelName = r.Room.Hotel.Name,
					UserId = r.UserId
				})
				.ToListAsync();

			return data;
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ReservationDto>> GetReservation(int id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var reservation = await _context.Reservations
				.Include(r => r.Room)
				.ThenInclude(r => r.Hotel)
				.FirstOrDefaultAsync(r => r.Id == id);

			if (reservation == null)
				return NotFound();

			if (!User.IsInRole("Admin") && reservation.UserId != userId)
				return Forbid();

			return new ReservationDto
			{
				Id = reservation.Id,
				FromDate = reservation.FromDate,
				ToDate = reservation.ToDate,
				RoomId = reservation.RoomId,
				RoomNumber = reservation.Room.Number,
				HotelId = reservation.Room.Hotel.Id,
				HotelName = reservation.Room.Hotel.Name,
				UserId = reservation.UserId
			};
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutReservation(int id, ReservationDto dto)
		{
			var reservation = await _context.Reservations.FindAsync(id);
			if (reservation == null) return NotFound();

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (!User.IsInRole("Admin") && reservation.UserId != userId)
				return Forbid();

			reservation.FromDate = dto.FromDate;
			reservation.ToDate = dto.ToDate;
			reservation.RoomId = dto.RoomId;
			reservation.HotelId = dto.HotelId;

			await _context.SaveChangesAsync();
			return NoContent();
		}

		[HttpPost]
		public async Task<ActionResult> PostReservation(ReservationDto dto)
		{
			var reservation = new Reservation
			{
				FromDate = dto.FromDate,
				ToDate = dto.ToDate,
				RoomId = dto.RoomId,
				HotelId = dto.HotelId,
				UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
			};

			_context.Reservations.Add(reservation);
			await _context.SaveChangesAsync();

			return Ok();
		}

		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
			var existing = await _context.Reservations.AsNoTracking()
				.FirstOrDefaultAsync(r => r.Id == id);

			if (existing == null)
			{
				return NotFound();
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!User.IsInRole("Admin") && existing.UserId != userId)
			{
				return Forbid();
			}

			var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
