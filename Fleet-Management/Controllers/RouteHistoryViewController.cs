using Fleet_Management.Data;
using Fleet_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;  
using System.Linq;
//////////////////  MVC Controller
///    //go to view
[Route("RouteHistory")]
public class RouteHistoryViewController : Controller
{
    private readonly ApplicationDbContext _context;

    public RouteHistoryViewController(ApplicationDbContext context)
    {
        _context = context;
    }

    
    [HttpGet]
    public IActionResult RouteHistory()
    {
        return View("routeHistory");
    }
}
