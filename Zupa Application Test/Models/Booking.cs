using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zupa_Application_Test.Models
{
    public class Booking
    {
        public long id { get; set; }
        public Seat[] seats = new Seat[4];
        public double cost = 0;
    }
}