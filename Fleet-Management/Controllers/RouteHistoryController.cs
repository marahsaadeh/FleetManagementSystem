using Fleet_Management.Data;
using Fleet_Management.Models;
using FPro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;



namespace Fleet_Management.Controllers
//////////////////  ApiController
{
    //[Route("api/[controller]")]
    [Route("api/RouteHistory")]

    //contain features
    [ApiController]

    //ControllerBase -> Because it won't go directly to the view
    //Controller -> when go directly to the view
    public class RouteHistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<RouteHistoryHub> _hubContext;
        private readonly ILogger<VehiclesAPIController> _logger;


        public RouteHistoryController(ApplicationDbContext context, IHubContext<RouteHistoryHub> hubContext, ILogger<VehiclesAPIController> logger)
        {
            _context = context;
            _hubContext = hubContext;
            _logger = logger;

        }

        [HttpGet("GetAll")]
        public IActionResult GetAllRouteHistories()
        {
            var routeHistories = _context.RouteHistories
                .Include(rh => rh.Vehicle)
                .Select(rh => new
                {
                    rh.VehicleID,
                    VehicleNumber = rh.Vehicle.VehicleNumber.ToString(),
                    rh.Address,
                    Status = rh.Status.ToString(),
                    Latitude = (float?)rh.Latitude,
                    Longitude = (float?)rh.Longitude,
                    rh.VehicleDirection,
                    GPSSpeed = rh.VehicleSpeed != null ? $"{rh.VehicleSpeed} " : null,
                    GPSTime = DateTimeOffset.FromUnixTimeMilliseconds(rh.Epoch).DateTime.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToList();

            var dataTable = ConvertToDataTable(routeHistories);

            var dataTableDto = new DataTableDto(dataTable);

            var gvar = new GVAR();
            gvar.DicOfDT["RouteHistories"] = dataTableDto.ToDataTable();

            return Ok(new { DicOfDT = new { RouteHistories = dataTableDto.Rows } });
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
        public async Task<IActionResult> AddRouteHistory([FromBody] RouteHistory newRouteHistory)
        {

            if (newRouteHistory == null)
            {
                return BadRequest(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = "Invalid Route History data."
                });
            }
      

            newRouteHistory.Epoch = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var routeHistory = new RouteHistory
            {
     VehicleID = newRouteHistory.VehicleID,
                VehicleDirection=newRouteHistory.VehicleDirection,
                Status=newRouteHistory.Status,
                VehicleSpeed= newRouteHistory.VehicleSpeed,
                Epoch= newRouteHistory.Epoch,
                Address=newRouteHistory.Address,
                Latitude= newRouteHistory.Latitude,
                Longitude= newRouteHistory.Longitude

            };
 
            _context.RouteHistories.Add(routeHistory);
            await _context.SaveChangesAsync();


            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "New route history point added");

            return Ok(new
            {
                DicOfDic = new Dictionary<string, object>
                {
                    ["Tags"] = new { SES = 1 }
                },
                RouteHistory = new
                {
                    RouteHistoryID = routeHistory.RouteHistoryID,
                    VehicleID= routeHistory.VehicleID,
                    VehicleDirection=routeHistory.VehicleDirection,
                    Status= routeHistory.Status,
                    VehicleSpeed= routeHistory.VehicleSpeed,
                    Epoch= routeHistory.Epoch,
                    Address= routeHistory.Address,
                    Latitude= routeHistory.Latitude,
                    Longitude= routeHistory.Longitude
                }
            });
        }
    

        //UpdateRouteHistoy
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateRouteHistoryByVehicle(long vehicleId, [FromBody] RouteHistory updatedRouteHistory)
        {
            if (updatedRouteHistory == null)
            {
                return BadRequest(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 }
                    },
                    Error = "Invalid Route History data."
                });
            }

            var existingRouteHistory = await _context.RouteHistories
                .Where(rh => rh.VehicleID == updatedRouteHistory.VehicleID)
                .OrderByDescending(rh => rh.Epoch)
                .FirstOrDefaultAsync();



            if (existingRouteHistory == null)
            {
                return NotFound(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 0 },
                        ["Error"] = $"No route history found for vehicle ID {vehicleId}."
                    }
                });
            }

       
            existingRouteHistory.VehicleID = updatedRouteHistory.VehicleID;
            existingRouteHistory.VehicleDirection = updatedRouteHistory.VehicleDirection;
            existingRouteHistory.Status = updatedRouteHistory.Status;
            existingRouteHistory.VehicleSpeed = updatedRouteHistory.VehicleSpeed;
            existingRouteHistory.Epoch = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            existingRouteHistory.Address = updatedRouteHistory.Address;
            existingRouteHistory.Latitude = updatedRouteHistory.Latitude;
            existingRouteHistory.Longitude = updatedRouteHistory.Longitude;

            try
            {
                await _context.SaveChangesAsync();
              
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Route history point updated");

                return Ok(new
                {
                    DicOfDic = new Dictionary<string, object>
                    {
                        ["Tags"] = new { SES = 1 }
                    },
                    RouteHistory = new
                    {
                        RouteHistoryID = existingRouteHistory.RouteHistoryID,
                        VehicleID = existingRouteHistory.VehicleID,
                        VehicleDirection = existingRouteHistory.VehicleDirection,
                        Status = existingRouteHistory.Status,
                        VehicleSpeed = existingRouteHistory.VehicleSpeed,
                        Epoch = existingRouteHistory.Epoch,
                        Address = existingRouteHistory.Address,
                        Latitude = existingRouteHistory.Latitude,
                        Longitude = existingRouteHistory.Longitude
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




    }

}




