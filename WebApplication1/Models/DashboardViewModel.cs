using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class DashboardViewModel
    {
        public required string UserName { get; set; }
        public int TotalUsers { get; set; }
        public int TotalRooms { get; set; }
        public int TotalBookings { get; set; }
    }
}