using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirlinesApp.Models;
using System.Text.Json.Serialization.Metadata;
using RestaurantWebApplication.RabbitMQ;

namespace AirlinesApp.Controllers
{
    public class TicketsController : Controller
    {
        private readonly AirlinesContext _context;
        private readonly IRabbitMqService _rabbitMqService;

        public TicketsController(AirlinesContext context)
        {
            _context = context;
            _rabbitMqService = new RabbitMQService();
        }

        // GET: Tickets
        public async Task<IActionResult> Index(int? id, string? name)
        {
            _rabbitMqService.SendMessage("Tickets page");
            if (id == null || name == null)
            {
                return RedirectToAction("Index", "Passengers");
            }
            ViewBag.PassengerId = id;
            ViewBag.PassengerName = name;
            var airlinesContext = _context.Tickets.Where(ticket => ticket.Id == id).Include(ticket => ticket.Flight);
            return View(await airlinesContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Flight)
                .Include(t => t.Passenger)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create(int id)
        {
            ViewBag.PassengerId = id;
            ViewBag.PassengerName = _context.Passengers.First(passenger => passenger.Id == id).FirstName;
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int PassengerId, [Bind("FlightId")] Ticket ticket)
        {
            ticket.PassengerId = PassengerId;
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.PassengerId = PassengerId;
            ViewBag.PassengerName = _context.Passengers.First(passenger => passenger.Id == PassengerId).FirstName;
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Name");
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Name", ticket.FlightId);
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "Id", "FirstName", ticket.PassengerId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlightId,PassengerId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            ViewData["FlightId"] = new SelectList(_context.Flights, "Id", "Name", ticket.FlightId);
            ViewData["PassengerId"] = new SelectList(_context.Passengers, "Id", "FirstName", ticket.PassengerId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tickets == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Flight)
                .Include(t => t.Passenger)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tickets == null)
            {
                return Problem("Entity set 'AirlinesContext.Tickets'  is null.");
            }
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
          return (_context.Tickets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
