using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models; // Make sure to include the correct namespace for the model

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        // Create the ViewModel and populate it with data
        var model = new DashboardViewModel
        {
            UserName = "Admin",  // Example: you could get this dynamically from the logged-in user
            TotalUsers = 100  // Example: replace with real data from the database
        };

        // Pass the ViewModel to the view
        return View(model);
    }
}
