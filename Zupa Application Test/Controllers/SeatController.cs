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
    [Microsoft.AspNetCore.Mvc.Route("api/seat")]
    [ApiController]
    public class SeatController : ApiController
    {
        private SeatContext _context;

        public SeatController()
        {
            if (_context == null)
            {
                var optionsBuilder = new DbContextOptionsBuilder<SeatContext>();
                optionsBuilder.UseInMemoryDatabase("SeatSystem");

                _context = new SeatContext(optionsBuilder.Options);
                FillContext();
            }
            else if (_context.Seats.Count() == 0) FillContext();
        }

        public SeatController(SeatContext context)
        {
            _context = context;

            if (_context.Seats.Count() == 0) FillContext();
        }

        // set up the DBs and fill them
        private void FillContext()
        {
            SeatsHandler.Init();
            BookingHandler.Init();

            int rows = SeatsHandler.ROWS;
            int columns = SeatsHandler.COLUMNS;
            SeatsHandler.seats = new Seat[rows, columns];
            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    SeatsHandler.seats[i, j] = new Seat() { taken = false, name = "", email = "", row = NumberToLetter(i + 1, true), column = j + 1 };

                    _context.Seats.Add(SeatsHandler.seats[i, j]);
                    _context.SaveChanges();

                    SeatsHandler.seatContext.Seats.Add(SeatsHandler.seats[i, j]);
                    SeatsHandler.seatContext.SaveChanges();
                }
            }
        }

        private String NumberToLetter(int number, bool isCaps)
        {
            Char c = (Char)((isCaps ? 65 : 97) + (number - 1));
            return c.ToString();
        }

        // GET: api/Todo
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<ActionResult<IEnumerable<Seat>>> GetSeats()
        {
            return await _context.Seats.ToListAsync();
        }

        // GET: api/Todo/5
        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public IHttpActionResult GetSeat(long id)
        {
            var seat = _context.Seats.FindAsync(id);

            if (seat == null)
            {
                return NotFound();
            }

            return Ok(seat);
        }

        // POST: api/Todo
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public IHttpActionResult PostSeat(Seat seat)
        {
            _context.Seats.Add(seat);
            _context.SaveChangesAsync();

            return Created("seat", seat);
        }


        // PUT: api/Todo/5
        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        public IHttpActionResult PutTodoItem(long id, Seat seat)
        {
            if (id != seat.id)
            {
                return BadRequest();
            }

            _context.Entry(seat).State = EntityState.Modified;
            _context.SaveChangesAsync();

            return (IHttpActionResult)new NoContentResult();
        }

        // DELETE: api/Todo/5
        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        public IHttpActionResult DeleteSeat(long id)
        {
            var seat = _context.Seats.Find(id);
            if (seat == null)
            {
                return NotFound();
            }

            _context.Seats.Remove(seat);
            _context.SaveChangesAsync();

            return Ok(seat);
        }
    }
}

