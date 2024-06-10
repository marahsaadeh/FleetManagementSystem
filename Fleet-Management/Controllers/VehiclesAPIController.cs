using Fleet_Management.Data;
using Fleet_Management.Models;
using FPro;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Vehicles")]
    //contain features
    [ApiController]
    //ControllerBase -> Because it won't go directly to the view
    //Controller -> when go directly to the view
    public class VehiclesAPIController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehiclesAPIController> _logger;


        public VehiclesAPIController(ApplicationDbContext context, ILogger<VehiclesAPIController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //GET
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            try
            {
               var lastRoutes = await _context.RouteHistories
              .GroupBy(rh => rh.VehicleID)
              .Select(group => new
              {
                  VehicleID = group.Key,

                  LastDirection = group.OrderByDescending(rh => rh.Epoch).Select(rh => rh.VehicleDirection).FirstOrDefault(),
                  LastStatus = group.OrderByDescending(rh => rh.Epoch)
                  .Select(rh => Convert.ToChar(rh.Status))  
                  .FirstOrDefault(),

                  LastAddress = group.OrderByDescending(rh => rh.Epoch).Select(rh => rh.Address).FirstOrDefault(),
                  Latitude = group.OrderByDescending(rh => rh.Epoch).Select(rh => (float?)rh.Latitude).FirstOrDefault(),
                  Longitude = group.OrderByDescending(rh => rh.Epoch).Select(rh => (float?)rh.Longitude).FirstOrDefault()
              })
              .ToListAsync();

            var lastRoutesDict = lastRoutes.ToDictionary(grp => grp.VehicleID, grp => new
            {
                LastDirection = grp.LastDirection,
                LastStatus = grp.LastStatus,
                LastAddress = grp.LastAddress ?? null,
                LastPosition = grp.Latitude.HasValue && grp.Longitude.HasValue ? $"{grp.Latitude}, {grp.Longitude}" : null
            });
            var vehicles = await _context.Vehicles
                .Select(vi => new VehicleDto
                {
                    VehicleID = vi.VehicleID,
                    VehicleNumber = vi.VehicleNumber,
                    VehicleType = vi.VehicleType,
                    LastDirection = lastRoutesDict.ContainsKey(vi.VehicleID) ? lastRoutesDict[vi.VehicleID].LastDirection : 0,
                   LastStatus = lastRoutesDict.ContainsKey(vi.VehicleID) ? lastRoutesDict[vi.VehicleID].LastStatus : ' ',
                    LastAddress = lastRoutesDict.ContainsKey(vi.VehicleID) ? lastRoutesDict[vi.VehicleID].LastAddress : null,
                    LastPosition = lastRoutesDict.ContainsKey(vi.VehicleID) ? lastRoutesDict[vi.VehicleID].LastPosition : null
                })
                .ToListAsync();
            var gvar = new GVAR();
            gvar.DicOfDT["Vehicles"] = ConvertToDataTable(vehicles);
            return Ok(new
            {
                DicOfDT = new { Vehicles = vehicles },
                DicOfDic = new { Tags = new { SES = 1 } }
            });
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError("Failed to fetch vehicles: {0}", ex.Message);
                return StatusCode(500, "Internal Server Error: Database operation failed.");
                
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddVehicle(VehicleDto vehicleDto)
        {
            if (vehicleDto == null)
            {
                return BadRequest(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = "Invalid vehicle data."
                });
            }

      
            var existingVehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleNumber == vehicleDto.VehicleNumber);

            if (existingVehicle != null)
            {

                return BadRequest(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 },
                        ["Error"] = $"VehicleNumber {vehicleDto.VehicleNumber} already exists."
                    }
                });
            }

            var vehicle = new Vehicle
            {
                VehicleNumber = vehicleDto.VehicleNumber,
                VehicleType = vehicleDto.VehicleType
            };

            try
            {
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = "Database update failed: " + dbEx.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to add vehicle: {Message}", ex.Message);
                return StatusCode(500, new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = "An unexpected error occurred: " + ex.Message
                });
            }


            return Ok(new
            {
                DicOfDic = new Dictionary<string, object>
                {
                    ["Tags"] = new { SES = 1 }
                },
                Vehicle = new
                {
                    Id = vehicle.VehicleID,
                    VehicleNumber = vehicle.VehicleNumber,
                    VehicleType = vehicle.VehicleType
                }
            });

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(long id, VehicleDto vehicleDto)
        {
            if (vehicleDto == null)
            {
                return BadRequest(new {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = "Invalid vehicle data provided."
                });
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound(new {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = $"Vehicle with ID {id} not found."
                });
            }

          
            var lastRouteHistory = _context.RouteHistories
                .Where(rh => rh.VehicleID == id)
                .OrderByDescending(rh => rh.Epoch)
                .Select(rh => new RouteHistory
                {
                    VehicleID = rh.VehicleID,
                    Address = rh.Address,
                    Status = rh.Status,
                    Latitude = rh.Latitude,
                    Longitude = rh.Longitude,
                    VehicleDirection = rh.VehicleDirection,
                    VehicleSpeed = rh.VehicleSpeed,
                    Epoch = rh.Epoch
                })
                .FirstOrDefault();

            if (lastRouteHistory != null)
            {
                lastRouteHistory.VehicleDirection = vehicleDto.LastDirection;
                lastRouteHistory.Status = vehicleDto.LastStatus;
                lastRouteHistory.Address = vehicleDto.LastAddress;

                var positionParts = vehicleDto.LastPosition?.Split(',');
                if (positionParts != null && positionParts.Length == 2 &&
                    float.TryParse(positionParts[0], out float latitude) &&
                    float.TryParse(positionParts[1], out float longitude))
                {
                    lastRouteHistory.Latitude = latitude;
                    lastRouteHistory.Longitude = longitude;
                }

      
            }

   
            vehicle.VehicleNumber = vehicleDto.VehicleNumber;
            vehicle.VehicleType = vehicleDto.VehicleType;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 1 }
                    },
                    Message = "Vehicle updated successfully."
                });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _context.Update(vehicle); 
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update vehicle with ID {Id}", id);
                return StatusCode(500, new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = "Update failed due to server error: " + ex.Message
                });
            }

            return StatusCode(500, new
            {
                DicOfDic = new Dictionary<string, object>
                {
                    ["Tags"] = new { SES = 0 }
                },
                Error = "An unexpected error occurred."
            });
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(long id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound(new
                {

                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 },
                        ["Error"] = $"No vehicle  found with ID {id}."
                    }

                });
            }

            _context.Vehicles.Remove(vehicle);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {

                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 1 }
                    },
                    Message = "Vehicle deleted successfully."

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {

                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 },
                        ["Error"] = ex.Message
                    }

                });
            }
        }











        private static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            var dataTable = new DataTable(typeof(T).Name);
            var properties = typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var prop in properties)
            {
                dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (var item in data)
            {
                var values = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null) ?? DBNull.Value;
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }



    }

}
