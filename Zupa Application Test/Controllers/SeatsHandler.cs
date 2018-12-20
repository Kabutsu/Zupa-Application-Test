using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Zupa_Application_Test.Models;

namespace Zupa_Application_Test.Controllers
{

    //In future, link to a DB
    public static class SeatsHandler
    {
        private static DbContextOptionsBuilder<BookingContext> bookingOptionsBuilder;
        private static DbContextOptionsBuilder<SeatContext> seatOptionsBuilder;
        public static BookingContext bookingContext;
        public static SeatContext seatContext;
        
        public static Seat[,] seats;
        public const int ROWS = 10;
        public const int COLUMNS = 10;

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

        // return all seats
        public static List<Seat> Seats()
        {
            List<Seat> seats = new List<Seat>();
            foreach (Seat seat in seatContext.Seats) seats.Add(seat);

            return seats;
        }

        // return all seats with no bookings
        public static List<Seat> FreeSeats()
        {
            List<Seat> freeSeats = new List<Seat>();
            foreach (Seat seat in seatContext.Seats)
            {
                if (seat.taken == false) freeSeats.Add(seat);
            }

            return freeSeats;
        }

        // return all seats that are booked
        public static List<Seat> TakenSeats()
        {
            List<Seat> takenSeats = new List<Seat>();
            foreach (Seat seat in seatContext.Seats)
            {
                if (seat.taken) takenSeats.Add(seat);
            }

            return takenSeats;
        }

        // update the db to take the given seat
        public static void TakeSeat(string name, string email, string row, int column)
        {
            //this little bit can probably be removed? not planning on storing all seats in pure memory
            int rowNo = (int)(row.ToCharArray()[0]) % 32;

            Seat seatToTake = seats[rowNo, column];
            seatToTake.taken = true;
            seatToTake.name = name;
            seatToTake.email = email;

            //update the seat listing in the db
            foreach (Seat seat in seatContext.Seats)
            {
                if (seat.row == row && seat.column == column)
                {
                    seat.taken = true;
                    seat.name = name;
                    seat.email = email;
                }
            }
        }
    }
}