using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelManageSys.Data;
using HotelManageSys.Models;
using HotelManageSys.Models.ViewModels;

namespace HotelManageSys.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(int? id)
        {
            //var applicationDbContext = _context.Booking.Include(b => b.Customer).Include(b => b.Parking).Include(b => b.PaymentType).Include(b => b.Room);
            //return View(await applicationDbContext.ToListAsync());
            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);

            IQueryable<Booking> bookings = from p in _context.Booking.Include(b => b.Customer).Include(b => b.Parking).Include(b => b.PaymentType).Include(b => b.Room) orderby p.StartDate select p;
            if (id != null)
            {
                bookings = bookings.Where(s => s.CustomerId.Equals(id));
            }


            var bookingCustomerVM = new BookingCustomerViewModel
            {
                Bookings = await bookings.ToListAsync(),
                Customer = customer
            };
            return View(bookingCustomerVM);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Customer)
                .Include(b => b.Parking)
                .Include(b => b.PaymentType)
                .Include(b => b.Room)
                .Include(b => b.Room.RoomStatus)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create(int? roomid, int? customerid)
        {

            if (roomid == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.RoomStatus)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.RoomId == roomid);

            if (room == null)
            {
                return NotFound();
            }

            if (customerid == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == customerid);
            if (customer == null)
            {
                return NotFound();
            }
            var booking = new Booking
            {
                Room = room,
                Customer = customer,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
            IQueryable<Parking> parking = from p in _context.Parking orderby p.ParkingNum select p;
            parking = parking.Where(p => p.IsAvailable);
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "FirstName", customerid);
            ViewData["ParkingId"] = new SelectList(parking.ToList(), "ParkingId", "ParkingNum");
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "Type");
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomNumber", roomid);
            return View(booking);


        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,StartDate,EndDate,IsPaid,PaymentTypeId,NeedParking,CustomerId,RoomId,ParkingId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                if (booking.NeedParking)
                {
                    var parking = await _context.Parking
                    .FirstOrDefaultAsync(m => m.ParkingId == booking.ParkingId);
                    parking.IsAvailable = false;
                    _context.Update(parking);
                    await _context.SaveChangesAsync();
                }
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CardNum", booking.CustomerId);
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>(), "ParkingId", "ParkingId", booking.ParkingId);
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "PaymentTypeId", booking.PaymentTypeId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomId", booking.RoomId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CardNum", booking.CustomerId);
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>(), "ParkingId", "ParkingId", booking.ParkingId);
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "PaymentTypeId", booking.PaymentTypeId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomId", booking.RoomId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,StartDate,EndDate,IsPaid,PaymentTypeId,NeedParking,CustomerId,RoomId,ParkingId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CardNum", booking.CustomerId);
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>(), "ParkingId", "ParkingId", booking.ParkingId);
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "PaymentTypeId", booking.PaymentTypeId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomId", booking.RoomId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Customer)
                .Include(b => b.Parking)
                .Include(b => b.PaymentType)
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> CheckIn(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            var room = await _context.Room
                .Include(r => r.RoomStatus)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.RoomId == booking.RoomId);

            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "FirstName", booking.CustomerId);
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>(), "ParkingId", "ParkingNum", booking.ParkingId);
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "Type", booking.PaymentTypeId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomNumber", booking.RoomId);
            ViewData["RoomStatusId"] = new SelectList(_context.Set<RoomStatus>(), "RoomStatusId", "Status", room.RoomStatusId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int id, [Bind("BookingId,StartDate,EndDate,IsPaid,PaymentTypeId,NeedParking,CustomerId,RoomId,ParkingId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            Room room = await _context.Room
                .Include(r => r.RoomStatus)
                 .Include(r => r.RoomType)
                 .FirstOrDefaultAsync(m => m.RoomId == booking.RoomId);
            RoomStatus roomStatus = await _context.RoomStatus.FirstOrDefaultAsync(s => s.Status.Equals("occupied clean"));
            
            if (ModelState.IsValid)
            {
                try
                {
                    room.RoomStatusId = roomStatus.RoomStatusId;
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "FirstName", booking.CustomerId);
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>(), "ParkingId", "ParkingNum", booking.ParkingId);
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "Type", booking.PaymentTypeId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomNumber", booking.RoomId);
            ViewData["RoomStatusId"] = new SelectList(_context.Set<RoomStatus>(), "RoomStatusId", "Status", booking.Room.RoomStatusId); return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> CheckOut(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            var room = await _context.Room
                .Include(r => r.RoomStatus)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.RoomId == booking.RoomId);

            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "FirstName", booking.CustomerId);
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>(), "ParkingId", "ParkingNum", booking.ParkingId);
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "Type", booking.PaymentTypeId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomNumber", booking.RoomId);
            ViewData["RoomStatusId"] = new SelectList(_context.Set<RoomStatus>(), "RoomStatusId", "Status", room.RoomStatusId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id, [Bind("BookingId,StartDate,EndDate,IsPaid,PaymentTypeId,NeedParking,CustomerId,RoomId,ParkingId")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            Room room = await _context.Room
                .Include(r => r.RoomStatus)
                 .Include(r => r.RoomType)
                 .FirstOrDefaultAsync(m => m.RoomId == booking.RoomId);
            RoomStatus roomStatus = await _context.RoomStatus.FirstOrDefaultAsync(s => s.Status.Equals("vacant dirty"));

            if (ModelState.IsValid)
            {
                try
                {
                    room.RoomStatusId = roomStatus.RoomStatusId;
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "FirstName", booking.CustomerId);
            ViewData["ParkingId"] = new SelectList(_context.Set<Parking>(), "ParkingId", "ParkingNum", booking.ParkingId);
            ViewData["PaymentTypeId"] = new SelectList(_context.Set<PaymentType>(), "PaymentTypeId", "Type", booking.PaymentTypeId);
            ViewData["RoomId"] = new SelectList(_context.Set<Room>(), "RoomId", "RoomNumber", booking.RoomId);
            ViewData["RoomStatusId"] = new SelectList(_context.Set<RoomStatus>(), "RoomStatusId", "Status", booking.Room.RoomStatusId); return View(booking);
        }
    }
}
