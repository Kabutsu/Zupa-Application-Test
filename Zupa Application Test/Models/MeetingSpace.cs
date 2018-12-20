using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zupa_Application_Test.Models
{
    public class MeetingSpace
    {
        public Seat[,] seats { get; set; } = new Seat[10, 10];
    }
}