﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirlinesApp.Models;
using RestaurantWebApplication.RabbitMQ;

namespace AirlinesApp.Controllers
{
    public class FlightsController : Controller
    {
        private readonly AirlinesContext _context;
        private readonly IRabbitMqService _rabbitMqService;

        public FlightsController(AirlinesContext context)
        {
            _context = context;
            _rabbitMqService = new RabbitMQService();
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            _rabbitMqService.SendMessage("Flights page");
            var airlinesContext = _context.Flights.Include(f => f.AirportFrom).Include(f => f.AirportTo);
            return View(await airlinesContext.ToListAsync());
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.AirportFrom)
                .Include(f => f.AirportTo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            ViewData["AirportFromId"] = new SelectList(_context.Airports, "Id", "Name");
            ViewData["AirportToId"] = new SelectList(_context.Airports, "Id", "Name");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,AirportFromId,AirportToId,DepartureTime,ArrivalTime,Price")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AirportFromId"] = new SelectList(_context.Airports, "Id", "Name", flight.AirportFromId);
            ViewData["AirportToId"] = new SelectList(_context.Airports, "Id", "Name", flight.AirportToId);
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            ViewData["AirportFromId"] = new SelectList(_context.Airports, "Id", "Name", flight.AirportFromId);
            ViewData["AirportToId"] = new SelectList(_context.Airports, "Id", "Name", flight.AirportToId);
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,AirportFromId,AirportToId,DepartureTime,ArrivalTime,Price")] Flight flight)
        {
            if (id != flight.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.Id))
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
            ViewData["AirportFromId"] = new SelectList(_context.Airports, "Id", "Name", flight.AirportFromId);
            ViewData["AirportToId"] = new SelectList(_context.Airports, "Id", "Name", flight.AirportToId);
            return View(flight);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Flights == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.AirportFrom)
                .Include(f => f.AirportTo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Flights == null)
            {
                return Problem("Entity set 'AirlinesContext.Flights'  is null.");
            }
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                _context.Flights.Remove(flight);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
          return (_context.Flights?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
