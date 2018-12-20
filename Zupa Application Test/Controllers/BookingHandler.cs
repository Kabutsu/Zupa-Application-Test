using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Zupa_Application_Test.Models;

namespace Zupa_Application_Test.Controllers
{

    //In future, link to a DB
    public static class BookingHandler
    {
        private static DbContextOptionsBuilder<BookingContext> bookingOptionsBuilder;
        private static DbContextOptionsBuilder<SeatContext> seatOptionsBuilder;
        public static BookingContext bookingContext;
        public static SeatContext seatContext;

        public static Booking[] bookings;

        public const double COST_PER_SEAT = 0;

        // set up the DBs
        public static void Init()
        {
            bookingOptionsBuilder = new DbContextOptionsBuilder<BookingContext>();
            bookingOptionsBuilder.UseInMemoryDatabase("BookingSystem");

            bookingContext = new BookingContext(bookingOptionsBuilder.Options);

            seatOptionsBuilder = new DbContextOptionsBuilder<SeatContext>();
            seatOptionsBuilder.UseInMemoryDatabase("BookingSystem");

            seatContext = new SeatContext(seatOptionsBuilder.Options);
        }

        // return all bookings
        public static List<Booking> Bookings()
        {
            List<Booking> bookings = new List<Booking>();
            foreach (Booking booking in bookingContext.Bookings) bookings.Add(booking);

            return bookings;
        }

        // check to see if the seats are available
        // save to the database if so
        public static bool MakeBooking(Booking booking)
        {
            // check that none of the seats are taken and that name and email are unique
            foreach(Seat seat in booking.seats)
            {
                if (SeatsHandler.TakenSeats().Contains(seat)) return false;

                foreach(Seat freeSeat in SeatsHandler.FreeSeats())
                {
                    if (freeSeat.name == seat.name || freeSeat.email == seat.email) return false;
                }

                seat.taken = true;
            }

            // update SeatsHandler's database
            foreach(Seat seat in booking.seats)
            {
                SeatsHandler.TakeSeat(seat.name, seat.email, seat.row, seat.column);
            }
            
            // update this database
            bookingContext.Add(booking);
            return true;
        }
    }
}