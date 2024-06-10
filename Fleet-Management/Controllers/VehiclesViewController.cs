using Microsoft.AspNetCore.Mvc;
using Fleet_Management.Data;
using Fleet_Management.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
//////////////////  MVC Controller
///    //go to view
[Route("vehicles")]
    public class VehiclesViewController : Controller
    {
        public IActionResult Vehicles()
    {
        return View("~/Views/VehiclesView/Vehicles.cshtml");
        }
    }
