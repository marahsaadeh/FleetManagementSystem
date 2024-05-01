using Fleet_Management.Data;
using Fleet_Management.Fleet_Management;
using Fleet_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fleet_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleInformationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VehicleInformationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/VehicleInformation
        [HttpGet]
        public async Task<IActionResult> GetVehicleInformations()
        {
            var vehicleInformations = await _context.VehicleInformations
                .Include(vi => vi.Vehicle)
                .Include(vi => vi.Driver)
                .Select(vi => new VehicleInformationDto
                {
                    VehicleInformationID = vi.VehicleInformationID,
                    VehicleID = vi.VehicleID,
                    VehicleNumber = vi.Vehicle.VehicleNumber.ToString(),
                    VehicleType = vi.Vehicle.VehicleType,
                    VehicleMake = vi.VehicleMake,
                    VehicleModel = vi.VehicleModel,
                    PurchaseDate = DateTimeOffset.FromUnixTimeMilliseconds(vi.PurchaseDate).DateTime,
                    DriverID = vi.DriverID,
                    DriverName = vi.Driver.DriverName,
                    PhoneNumber = vi.Driver.PhoneNumber.ToString()
                })

                .ToListAsync();

            return Ok(vehicleInformations);
        }

    }


}