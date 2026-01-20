using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Projekt.Controllers
{
	[Authorize]
	public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var reservations = _context.Reservations
				.Include(r => r.Room)
				.ThenInclude(r => r.Hotel)
				.AsQueryable();

			if (!User.IsInRole("Admin"))
			{
				reservations = reservations.Where(r => r.UserId == userId);
			}

			return View(await reservations.ToListAsync());
		}

		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var reservation = await _context.Reservations
		.Include(r => r.Room)
		.ThenInclude(r => r.Hotel)
		.FirstOrDefaultAsync(m => m.Id == id);

			if (reservation == null) return NotFound();

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!User.IsInRole("Admin") && reservation.UserId != userId)
			{
				return Forbid();
			}

			return View(reservation);
		}

		public IActionResult Create()
		{
			ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name");
			ViewData["RoomId"] = new SelectList(Enumerable.Empty<Room>(), "Id", "Number");

			return View();
		}

		public IActionResult GetRoomsByHotel(int hotelId)
		{
			var rooms = _context.Rooms
				.Where(r => r.HotelId == hotelId)
				.Select(r => new { r.Id, r.Number })
				.ToList();

			return Json(rooms);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Reservation reservation)
		{
			reservation.UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

			bool conflict = _context.Reservations.Any(r =>
				r.RoomId == reservation.RoomId &&
				reservation.FromDate < r.ToDate &&
				reservation.ToDate > r.FromDate
			);

			if (conflict)
			{
				ModelState.AddModelError("", "Ten pokój jest już zarezerwowany w tym terminie.");
			}

			if (ModelState.IsValid)
			{
				_context.Add(reservation);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			ViewData["HotelId"] = new SelectList(_context.Hotels, "Id", "Name", reservation.HotelId);

			ViewData["RoomId"] = new SelectList(
				_context.Rooms.Where(r => r.HotelId == reservation.HotelId),
				"Id",
				"Number",
				reservation.RoomId
			);

			return View(reservation);
		}

		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var reservation = await _context.Reservations
				.Include(r => r.Room)
				.ThenInclude(r => r.Hotel)
				.FirstOrDefaultAsync(r => r.Id == id);

			if (reservation == null)
			{
				return NotFound();
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!User.IsInRole("Admin") && reservation.UserId != userId)
			{
				return Forbid();
			}

			ViewData["HotelId"] = new SelectList(
				_context.Hotels,
				"Id",
				"Name",
				reservation.Room.HotelId
			);

			ViewData["RoomId"] = new SelectList(
				_context.Rooms.Where(r => r.HotelId == reservation.Room.HotelId),
				"Id",
				"Number",
				reservation.RoomId
			);

			return View(reservation);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(
			int id,
			[Bind("Id,FromDate,ToDate,HotelId,RoomId")] Reservation reservation)
		{
			var existing = await _context.Reservations.AsNoTracking()
				.FirstOrDefaultAsync(r => r.Id == reservation.Id);

			if (existing == null)
			{
				return NotFound();
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!User.IsInRole("Admin") && existing.UserId != userId)
			{
				return Forbid();
			}

			reservation.UserId = existing.UserId;

			if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
			ViewData["HotelId"] = new SelectList(
				_context.Hotels,
				"Id",
				"Name",
				reservation.HotelId
			);

			ViewData["RoomId"] = new SelectList(
				_context.Rooms.Where(r => r.HotelId == reservation.HotelId),
				"Id",
				"Number",
				reservation.RoomId
			);

			return View(reservation);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
				.ThenInclude(r => r.Hotel)
				.FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);

			if (reservation == null)
			{
				return NotFound();
			}

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (!User.IsInRole("Admin") && reservation.UserId != userId)
			{
				return Forbid();
			}

			if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
