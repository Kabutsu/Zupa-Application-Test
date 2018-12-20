using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zupa_Application_Test.Models
{
    public class Seat
    {
        public long id { get; set; }
        public bool taken { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string row { get; set; }
        public int column { get; set; }
    }
}