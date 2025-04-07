using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using WebApplication1.Data;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly HotelManagementContext _context;

        public HomeController(HotelManagementContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {            
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {            
            return View();
        }

        public IActionResult Rooms()
        {            
            // Create sample room data if database is empty
                if (!_context.Rooms.Any())
            {
                var sampleRooms = new List<Room>
                {
                    new Room
                    {
                        RoomNumber = "101",
                        RoomType = "Mini Suite",
                        IsAvailable = true,
                        ImagePath = "~/images/mini-suite.jpg",
                        RoomName = "Mini Suite",
                        Rating = 4.5,
                        Price = 150,
                        DetailsLink = "MiniSuite"
                    },
                    new Room
                    {
                        RoomNumber = "102",
                        RoomType = "Ultra Deluxe",
                        IsAvailable = true,
                        ImagePath = "~/images/ultra-deluxe.jpg",
                        RoomName = "Ultra Deluxe",
                        Rating = 4.8,
                        Price = 250,
                        DetailsLink = "UltraDeluxe"
                    },
                    new Room
                    {
                        RoomNumber = "103",
                        RoomType = "Luxury Room",
                        IsAvailable = true,
                        ImagePath = "~/images/luxury-room.jpg",
                        RoomName = "Luxury Room",
                        Rating = 4.7,
                        Price = 200,
                        DetailsLink = "LuxuryRoom"
                    },
                    new Room
                    {
                        RoomNumber = "104",
                        RoomType = "Premium Room",
                        IsAvailable = true,
                        ImagePath = "~/images/premium-room.jpg",
                        RoomName = "Premium Room",
                        Rating = 4.6,
                        Price = 180,
                        DetailsLink = "PremiumRoom"
                    },
                    new Room
                    {
                        RoomNumber = "105",
                        RoomType = "Master Room",
                        IsAvailable = true,
                        ImagePath = "~/images/master-room.jpg",
                        RoomName = "Master Room",
                        Rating = 4.9,
                        Price = 300,
                        DetailsLink = "MasterRoom"
                    }
                };

                _context.Rooms.AddRange(sampleRooms);
                _context.SaveChanges();
            }

            // Get rooms from database
            var rooms = _context.Rooms.ToList();
            return View(rooms);
        }

        public IActionResult About()
        {            
            return View();
        }

        public IActionResult Contact()
        {            
            return View();
        }

        public IActionResult MiniSuite()
        {            
            return View();
        }

        public IActionResult UltraDeluxe()
        {            
            return View();
        }

        public IActionResult LuxuryRoom()
        {            
            return View();
        }

        public IActionResult PremiumRoom()
        {            
            return View();
        }

        public IActionResult MasterRoom()
        {            
            return View();
        }

        public IActionResult Signup()
        {            
            return RedirectToAction("Signup", "Auth");
        }
    }
}

