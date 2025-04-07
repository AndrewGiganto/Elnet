using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using System.Security.Cryptography;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
        private readonly HotelManagementContext _context;
        private const int MaxFailedAttempts = 3;
        private const int LockoutMinutes = 15;

        public AuthController(HotelManagementContext context)
        {
            _context = context;
        }

        // Login (GET)
        public IActionResult Login()
        {
            return View();
        }

        // Login (POST)
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (user == null)
            {
                ViewBag.Error = "Invalid credentials. Try again.";
                return View();
            }

            if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                ViewBag.Error = $"Account is locked. Please try again after {user.LockoutEnd.Value.ToLocalTime():HH:mm:ss}";
                return View();
            }

            if (!VerifyPassword(model.Password, user.PasswordHash))
            {
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= MaxFailedAttempts)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(LockoutMinutes);
                    ViewBag.Error = $"Account locked for {LockoutMinutes} minutes due to multiple failed attempts.";
                }
                else
                {
                    ViewBag.Error = "Invalid credentials. Try again.";
                }
                await _context.SaveChangesAsync();
                return View();
            }

            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Dashboard");
        }

        // Signup (GET)
        public IActionResult Signup()
        {
            return View(new SignupViewModel());
        }

        // Helper method to hash password with salt
        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        // Helper method to verify password
        private bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                var parts = storedHash.Split(':');
                var salt = Convert.FromBase64String(parts[0]);
                var hash = parts[1];

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                return hash == hashed;
            }
            catch
            {
                return false;
            }
        }

        // Signup (POST)
        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            if (model.Password.Length < 8 || !model.Password.Any(char.IsUpper) || !model.Password.Any(char.IsLower) ||
                !model.Password.Any(char.IsDigit) || !model.Password.Any(c => !char.IsLetterOrDigit(c)))
            {
                ModelState.AddModelError("Password", "Password must be at least 8 characters long and contain uppercase, lowercase, number, and special character.");
                return View(model);
            }

            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ViewBag.Error = "Email already registered.";
                return View(model);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                Role = "User" // Default role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

