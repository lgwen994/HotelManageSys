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
using System.Collections;

namespace HotelManageSys.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(int? id, DateTime SearchDateFrom, DateTime SearchDateTo, int RoomTypeId)
        {
            //var applicationDbContext = _context.Room.Include(r => r.RoomStatus).Include(r => r.RoomType);
            //return View(await applicationDbContext.ToListAsync());

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            IQueryable<Booking> bookings = from p in _context.Booking.Include(b => b.Room) orderby p.StartDate select p;
            bookings =  bookings.Where(b => SearchDateFrom <= b.StartDate && b.EndDate <= SearchDateTo);
            var bookingList = bookings.ToList();
            IQueryable<Room> rooms = from p in _context.Room.Include(r => r.RoomStatus).Include(r => r.RoomType) orderby p.RoomId select p;
            if(RoomTypeId != 0)
            {
                rooms = rooms.Where(r => r.RoomTypeId == RoomTypeId);
            }
            rooms = rooms.Where(r => r.RoomStatus.Status.Equals("vacant clean"));
            List<Room> roomsList = await rooms.ToListAsync();
            foreach(Booking item in bookingList)
            {
                var itemToRemove = roomsList.SingleOrDefault(r => r.RoomId == item.RoomId);
                if (itemToRemove != null)
                {
                    roomsList.Remove(itemToRemove);
                }
                    
            }
            //roomsList.Remove()
            var roomsCustomerVM = new RoomsCustomerViewModel
            {
                Rooms = roomsList,
                Customer = customer,
                SearchDateFrom = DateTime.Now,
                SearchDateTo = DateTime.Now
            };
            ViewData["RoomTypeId"] = new SelectList(_context.Set<RoomType>(), "RoomTypeId", "Type");
            return View(roomsCustomerVM);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id, int? customerid)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.RoomStatus)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            Customer customer = null;
            if (customerid != null)
            {
                customer = await _context.Customer
                    .FirstOrDefaultAsync(m => m.CustomerId == customerid);
            }


            if (room == null)
            {
                return NotFound();
            }

            var roomCustomerVM = new RoomCustomerViewModel
            {
                Room = room,
                Customer = customer
            };

            return View(roomCustomerVM);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            ViewData["RoomStatusId"] = new SelectList(_context.Set<RoomStatus>(), "RoomStatusId", "RoomStatusId");
            ViewData["RoomTypeId"] = new SelectList(_context.Set<RoomType>(), "RoomTypeId", "RoomTypeId");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,RoomNumber,RoomStatusId,RoomTypeId,IsBooked")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomStatusId"] = new SelectList(_context.Set<RoomStatus>(), "RoomStatusId", "RoomStatusId", room.RoomStatusId);
            ViewData["RoomTypeId"] = new SelectList(_context.Set<RoomType>(), "RoomTypeId", "RoomTypeId", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["RoomStatusId"] = new SelectList(_context.Set<RoomStatus>(), "RoomStatusId", "Status", room.RoomStatusId);
            ViewData["RoomTypeId"] = new SelectList(_context.Set<RoomType>(), "RoomTypeId", "Type", room.RoomTypeId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,RoomNumber,RoomStatusId,RoomTypeId,IsBooked")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
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
            ViewData["RoomStatusId"] = new SelectList(_context.Set<RoomStatus>(), "RoomStatusId", "RoomStatusId", room.RoomStatusId);
            ViewData["RoomTypeId"] = new SelectList(_context.Set<RoomType>(), "RoomTypeId", "RoomTypeId", room.RoomTypeId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Room
                .Include(r => r.RoomStatus)
                .Include(r => r.RoomType)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Room.FindAsync(id);
            _context.Room.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Room.Any(e => e.RoomId == id);
        }
    }
}
