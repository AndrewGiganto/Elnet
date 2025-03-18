using Microsoft.AspNetCore.Mvc;

public class AuthController : Controller
{
    // Login (GET)
    public IActionResult Login()
    {
        return View();
    }

    // Login (POST)
    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // Dummy authentication logic (replace with actual logic)
        if (username == "admin" && password == "password")
        {
            // Redirect to the Dashboard after login
            return RedirectToAction("Index", "Dashboard"); // Ensure that 'Index' action exists in DashboardController
        }
        ViewBag.Error = "Invalid credentials. Try again.";
        return View(); // Show the login page again with error message
    }

    // Signup (GET)
    public IActionResult Signup()
    {
        return View();
    }

    // Signup (POST)
    [HttpPost]
    public IActionResult Signup(string username, string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            ViewBag.Error = "Passwords do not match.";
            return View();
        }
        // Save user details (database logic should go here, such as saving username and password)
        return RedirectToAction("Login"); // Redirect to the login page after successful signup
    }
}
