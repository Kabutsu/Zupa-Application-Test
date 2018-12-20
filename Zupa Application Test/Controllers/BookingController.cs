using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Zupa_Application_Test.Models;
using System.Web.Http;

namespace Zupa_Application_Test.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/booking")]
    [ApiController]
    public class BookingController : ApiController
    {
        private BookingContext _context;

        public BookingController()
        {
            if (_context == null)
            {
                var optionsBuilder = new DbContextOptionsBuilder<BookingContext>();
                optionsBuilder.UseInMemoryDatabase("BookingSystem");

                _context = new BookingContext(optionsBuilder.Options);
                FillContext();
            } else if (_context.Bookings.Count() == 0) FillContext();
        }

        public BookingController(BookingContext context)
        {
            _context = context;

            if (_context.Bookings.Count() == 0) FillContext();
        }

        private void FillContext()
        {
            _context.Bookings.Add(new Booking());
            _context.SaveChanges();
        }

        
        // POST: api/Todo
        // The meat & veg of making a booking is right here
        // I'm assuming that seat names will come in like e.g. "A,10"
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IHttpActionResult PostBooking(String[] names, String[] emails, String[] seatNames)
        {
            //check that there are 1-4 seats booked
            int seatsBooked = names.Length;
            if ((!(seatsBooked == emails.Length && seatsBooked == seatNames.Length)) || seatsBooked > 4 || seatsBooked <= 0) return (IHttpActionResult) new ForbidResult();

            // create the booking
            Seat[] seatsToBook = new Seat[seatsBooked];
            for(int i = 0; i < seatsBooked; i++)
            {
                seatsToBook[i] = new Seat() { taken = true, name = names[i], email = emails[i],
                    row = seatNames[i].Split(',')[0], column = Int32.Parse(seatNames[i].Split(',')[1]) };
            }

            Booking booking = new Booking() { seats = seatsToBook, cost = seatsBooked * BookingHandler.COST_PER_SEAT };

            // look up the seats in the database and make the booking if possible
            if (BookingHandler.MakeBooking(booking))
            {
                _context.Bookings.Add(booking);
                _context.SaveChangesAsync();
                return Created("booking", booking);
            } else return (IHttpActionResult)new ForbidResult();
        }

        
        // GET: api/Todo
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings.ToListAsync();
        }

        // GET: api/Todo/5
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public IHttpActionResult GetBooking(long id)
        {
            var booking = _context.Bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }


        // PUT: api/Todo/5
        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        public IHttpActionResult UpdateBooking(long id, Booking booking)
        {
            if (id != booking.id)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;
            _context.SaveChangesAsync();

            return (IHttpActionResult) new NoContentResult();
        }

        // DELETE: api/Todo/5
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public IHttpActionResult DeleteBooking(long id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            _context.SaveChangesAsync();

            return Ok(booking);
        }
    }
}

