using Fleet_Management.Data;
using Fleet_Management.Models;
using FPro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace Fleet_Management.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Drivers")]

    //contain features
    [ApiController]

    //ControllerBase -> Because it won't go directly to the view
    //Controller -> when go directly to the view
    public class DriverController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VehiclesAPIController> _logger;

        public DriverController(ApplicationDbContext context, ILogger<VehiclesAPIController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //GET
        [HttpGet]
        public async Task<IActionResult> GetDrivers()
        {
            try
            {
                var drivers = _context.Drivers
                 .Select(dr => new
                 {
                     dr.DriverID,
                     dr.DriverName,
                     dr.PhoneNumber
         
                 })
                 .ToList();

                var dataTable = ConvertToDataTable(drivers);
                var dataTableDto = new DataTableDto(dataTable);
                  var gvar = new GVAR();
                  gvar.DicOfDT["Drivers"] = ConvertToDataTable(drivers);
                  return Ok(new
                  {
                      DicOfDT = new { Driver = drivers },
                      DicOfDic = new { Tags = new { SES = 1 } }
                  });
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError("Failed to fetch drivers: {0}", ex.Message);
                return StatusCode(500, "Internal Server Error: Database operation failed.");

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


        [HttpPost("Add")]
        public async Task<IActionResult> AddDriver([FromBody] Driver newDriver)
        {

            if (newDriver == null)
            {
                return BadRequest(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = "Invalid driver data."
                });
            }


  
            var existingDriver = await _context.Drivers
                .FirstOrDefaultAsync(d => d.PhoneNumber == newDriver.PhoneNumber);

            if (existingDriver != null)
            {
              
                return BadRequest(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 },
                        ["Error"] = $"PhoneNumber {newDriver.PhoneNumber} already exists."
                    }
                });
            }
        
            var driver = new Driver
            {
                DriverName = newDriver.DriverName,
                PhoneNumber = newDriver.PhoneNumber
            };

            try
            {
              
                _context.Drivers.Add(driver);
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
                _logger.LogError("Failed to add driver: {Message}", ex.Message);
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
                Driver = new
                {
                    DriverID = driver.DriverID,
                    DriverName = driver.DriverName,
                    PhoneNumber = driver.PhoneNumber
                }
            });
     

           
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDriver(long id, [FromBody] Driver updatedDriver)
        {
            if (updatedDriver == null)
            {
                return BadRequest(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = "Invalid driver data provided."
                });
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = $"Driver with ID {id} not found."
                });
            }

            // Check if the phone number already exists in another driver entry
            var existingDriverWithPhone = await _context.Drivers
                .FirstOrDefaultAsync(d => d.PhoneNumber == updatedDriver.PhoneNumber && d.DriverID != id);
            if (existingDriverWithPhone != null)
            {
                return BadRequest(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = $"PhoneNumber {updatedDriver.PhoneNumber} is already in use by another driver."
                });
            }

            // Update properties
            driver.DriverName = updatedDriver.DriverName;
            driver.PhoneNumber = updatedDriver.PhoneNumber;

            try
            {
                _context.Drivers.Update(driver);
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
                _logger.LogError("Failed to update driver: {Message}", ex.Message);
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
                Driver = new
                {
                    DriverID = driver.DriverID,
                    DriverName = driver.DriverName,
                    PhoneNumber = driver.PhoneNumber
                }
            });
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(long id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null) {
                return NotFound(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 },
                        ["Error"] = $"No driver  found with ID {id}."
                    }
                }
                    
                    );
            }
               

            _context.Drivers.Remove(driver);
            try
            {
                await _context.SaveChangesAsync();
            return Ok(new {
                DicOfDic = new Dictionary<string, object>
                {
                    ["Tags"] = new { SES = 1 }
                },
                Message = "Driver deleted successfully."

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

    }

}
