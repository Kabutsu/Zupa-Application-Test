using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace Zupa_Application_Test.Models
{
    public class SeatContext : DbContext
    {
        public SeatContext(DbContextOptions<SeatContext> options)
            : base(options)
        {
        }

        public DbSet<Seat> Seats { get; set; }
    }
}