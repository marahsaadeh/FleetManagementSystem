using Fleet_Management.Data;
using Fleet_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FPro; 
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data; 
using System.Linq;
using System.Threading.Tasks;

namespace Fleet_Management.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/VehicleInformation")]

    //contain features
    [ApiController]

    //ControllerBase -> Because it won't go directly to the view
    //Controller -> when go directly to the view
    public class VehicleInformationAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VehicleInformationAPIController(ApplicationDbContext context)
        {
            _context = context;
        }
        //GET

        [HttpGet]
        public async Task<IActionResult> GetVehicleInformations()
        {
           
            var lastRoutes = await _context.RouteHistories
                .GroupBy(rh => rh.VehicleID)
                .Select(group => new
                {
                    VehicleID = group.Key,
                    LastGPSSpeed = group.OrderByDescending(rh => rh.Epoch).Select(rh => rh.VehicleSpeed).FirstOrDefault(),
                    LastAddress = group.OrderByDescending(rh => rh.Epoch).Select(rh => rh.Address).FirstOrDefault(),
                    LastGPSTime = group.OrderByDescending(rh => rh.Epoch).Select(rh => rh.Epoch).FirstOrDefault(),
                    Latitude = group.OrderByDescending(rh => rh.Epoch).Select(rh => (float?)rh.Latitude).FirstOrDefault(),
                    Longitude = group.OrderByDescending(rh => rh.Epoch).Select(rh => (float?)rh.Longitude).FirstOrDefault()
                })
                .ToListAsync();

            var lastRoutesDict = lastRoutes.ToDictionary(grp => grp.VehicleID, grp => new
            {
                LastGPSSpeed = grp.LastGPSSpeed != null ? $"{grp.LastGPSSpeed} km/h" : null,
                LastAddress = grp.LastAddress ?? null,
                LastGPSTime = grp.LastGPSTime != null
                    ? (long?)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                    : null,
    

                LastPosition = grp.Latitude.HasValue && grp.Longitude.HasValue ? $"{grp.Latitude}, {grp.Longitude}" : null
            });

            var vehicleInformations = await _context.VehicleInformations
                .Include(vi => vi.Vehicle)
                .Include(vi => vi.Driver)
                .Select(vi => new VehicleInformationDto
                {
                    VehicleInformationID = vi.VehicleInformationID,
                    VehicleID = vi.VehicleID,
                    VehicleNumber = vi.Vehicle.VehicleNumber,
                    VehicleType = vi.Vehicle.VehicleType,
                    VehicleMake = vi.VehicleMake,
                    VehicleModel = vi.VehicleModel,
                    PurchaseDate = DateTimeOffset.FromUnixTimeMilliseconds((long)vi.PurchaseDate).DateTime,
                    DriverID = vi.DriverID,
                    DriverName = vi.Driver.DriverName,
                    PhoneNumber = vi.Driver.PhoneNumber.ToString(),
                    LastGPSSpeed = lastRoutesDict.ContainsKey(vi.VehicleID) ? lastRoutesDict[vi.VehicleID].LastGPSSpeed : null,
                    LastAddress = lastRoutesDict.ContainsKey(vi.VehicleID) ? lastRoutesDict[vi.VehicleID].LastAddress : null,

                    LastGPSTime = lastRoutesDict.ContainsKey(vi.VehicleID)
    ? lastRoutesDict[vi.VehicleID].LastGPSTime.HasValue 
        ? (DateTime?)DateTimeOffset.FromUnixTimeMilliseconds(lastRoutesDict[vi.VehicleID].LastGPSTime.Value).DateTime
        : null
    : null,



            LastPosition = lastRoutesDict.ContainsKey(vi.VehicleID) ? lastRoutesDict[vi.VehicleID].LastPosition : null
                })
                .ToListAsync();

            var gvar = new GVAR();
            gvar.DicOfDT["VehicleInformations"] = ConvertToDataTable(vehicleInformations);

            return Ok(new
            {
                DicOfDT = new { VehicleInformations = vehicleInformations },
                DicOfDic = new { Tags = new { SES = 1 } }
            });
        }
        //ADD   api/VehicleInformation/Add
        [HttpPost("Add")]
        public async Task<IActionResult> AddVehicleInformation(VehicleInformationDto vehicleInformationDto)
        {
            if (vehicleInformationDto == null)
            {

                return BadRequest(new
                {

                        DicOfDic = new Dictionary<string, object>
                        {
                            ["Tags"] = new { SES = 0 }
                        },
                        Error = "Invalid vehicle information data."
                    
                });
            }
            //In order not to happen Duplicate VehicleNumber
            var existingVehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleNumber == vehicleInformationDto.VehicleNumber);

            if (existingVehicle == null)
            {
                return NotFound(new
                {
                   
                        DicOfDic = new Dictionary<string, object>
                        {
                            ["Tags"] = new { SES = 0 },
                            ["Error"] = $"VehicleNumber {vehicleInformationDto.VehicleNumber} does not exist."
                        }
                    
                });
            }
            //In order not to happen Duplicate DriverID
            var existingDriver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.DriverID == vehicleInformationDto.DriverID);

            if (existingDriver == null)
            {
                return NotFound(new
                {
                   
                        DicOfDic = new Dictionary<string, object>
                        {
                            ["Tags"] = new { SES = 0 },
                            ["Error"] = $"DriverID {vehicleInformationDto.DriverID} does not exist."
                        }
                    
                });
            }
            //vehicleInformation = VehicleInformationDTO
            var vehicleInformation = new VehicleInformation
            {
                VehicleID = existingVehicle.VehicleID,
                VehicleMake = vehicleInformationDto.VehicleMake,
                VehicleModel = vehicleInformationDto.VehicleModel,
                PurchaseDate = ((DateTimeOffset)vehicleInformationDto.PurchaseDate).ToUnixTimeMilliseconds(),
                DriverID = vehicleInformationDto.DriverID
            };

            try
            {
                _context.VehicleInformations.Add(vehicleInformation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    
                        DicOfDic = new Dictionary<string, object>
                        {
                            ["Tags"] = new { SES = 0 }
                        },
                        Error = ex.Message
                    
                });
            }

            return Ok(new
            {
                
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 1 }
                    }
                
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleInformation(long id, VehicleInformationDto vehicleInformationDto)
        
       {
            if (vehicleInformationDto == null)
            {
                return BadRequest(new
                {
                    
                        DicOfDic = new Dictionary<string, object>
                        {
                            ["Tags"] = new { SES = 0 }
                        },
                        Error = "Invalid vehicle information data."
                    
                });
            }

            var vehicleInfo = await _context.VehicleInformations.FindAsync(id);
            if (vehicleInfo == null)
            {
                return NotFound(new
                {
                    
                        DicOfDic = new Dictionary<string, object>
                        {
                            ["Tags"] = new { SES = 0 },
                            ["Error"] = $"No vehicle information found with ID {id}."
                        }
                    
                });
            }

        
            vehicleInfo.VehicleMake = vehicleInformationDto.VehicleMake;
            vehicleInfo.VehicleModel = vehicleInformationDto.VehicleModel;
            vehicleInfo.PurchaseDate = ((DateTimeOffset)vehicleInformationDto.PurchaseDate).ToUnixTimeMilliseconds();
            vehicleInfo.DriverID = vehicleInformationDto.DriverID;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    
                        DicOfDic = new Dictionary<string, object>
                        {
                            ["Tags"] = new { SES = 1 }
                        }
                    
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                  
                        DicOfDic = new Dictionary<string, object>
                        {
                            ["Tags"] = new { SES = 0 }
                        },
                        Error = ex.Message
                    
                });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleInformation(long id)
        {
            var vehicleInfo = await _context.VehicleInformations.FindAsync(id);
            if (vehicleInfo == null)
            {
                return NotFound(new
                {
                    
                        DicOfDic = new Dictionary<string, object>
                        {
                            ["Tags"] = new { SES = 0 },
                            ["Error"] = $"No vehicle information found with ID {id}."
                        }
                    
                });
            }

            _context.VehicleInformations.Remove(vehicleInfo);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    
                        DicOfDic = new Dictionary<string, object>
                        {
                            ["Tags"] = new { SES = 1 }
                        },
                        Message = "Vehicle information deleted successfully."
                    
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










