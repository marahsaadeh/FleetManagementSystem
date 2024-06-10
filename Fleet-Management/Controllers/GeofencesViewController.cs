using Fleet_Management.Data;
using Fleet_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
//////////////////  MVC Controller
///    //go to view
[Route("Geofences")]

public class GeofencesViewController : Controller
    {
    [HttpGet]
  
    public IActionResult Geofences()
    {
        return View("~/Views/GeofencesView/Geofences.cshtml");
    }

}



