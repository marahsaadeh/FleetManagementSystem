using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fleet_Management.Data;
using FPro;

namespace Fleet_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeofenceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GeofenceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllGeofences")]
        public IActionResult GetAllGeofences()
        {
            var geofences = _context.Geofences
                .Select(gf => new
                {
                    gf.GeofenceID,
                    gf.GeofenceType,
                    AddedDate = DateTimeOffset.FromUnixTimeMilliseconds(gf.AddedDate).DateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    gf.StrokeColor,
                    gf.StrokeOpacity,
                    gf.StrokeWeight,
                    gf.FillColor,
                    gf.FillOpacity
                })
                .ToList();

            var dataTable = ConvertToDataTable(geofences);
            var dataTableDto = new DataTableDto(dataTable);

            var gvar = new GVAR();
            gvar.DicOfDT["Geofences"] = dataTableDto.ToDataTable();

            return Ok(new { DicOfDT = new { Geofences = dataTableDto.Rows } });
        }

        [HttpGet("GetAllCircleGeofences")]
        public IActionResult GetAllCircleGeofences()
        {
            var circleGeofences = _context.CircleGeofence
                .Select(cgf => new
                {
                    cgf.GeofenceID,
                    cgf.Radius,
                    cgf.Latitude,
                    cgf.Longitude
                })
                .ToList();

            var dataTable = ConvertToDataTable(circleGeofences);
            var dataTableDto = new DataTableDto(dataTable);

            var gvar = new GVAR();
            gvar.DicOfDT["CircleGeofences"] = dataTableDto.ToDataTable();

            return Ok(new { DicOfDT = new { CircleGeofences = dataTableDto.Rows } });
        }

        [HttpGet("GetAllRectangleGeofences")]
        public IActionResult GetAllRectangleGeofences()
        {
            var rectangleGeofences = _context.RectangleGeofence
                .Select(rgf => new
                {
                    rgf.GeofenceID,
                    rgf.North,
                    rgf.East,
                    rgf.West,
                    rgf.South
                })
                .ToList();

            var dataTable = ConvertToDataTable(rectangleGeofences);
            var dataTableDto = new DataTableDto(dataTable);

            var gvar = new GVAR();
            gvar.DicOfDT["RectangleGeofences"] = dataTableDto.ToDataTable();

            return Ok(new { DicOfDT = new { RectangleGeofences = dataTableDto.Rows } });
        }

        [HttpGet("GetAllPolygonGeofences")]
        public IActionResult GetAllPolygonGeofences()
        {
            var polygonGeofences = _context.PolygonGeofence
                .Select(pgf => new
                {
                    pgf.GeofenceID,
                    pgf.Latitude,
                    pgf.Longitude
                })
                .ToList();

            var dataTable = ConvertToDataTable(polygonGeofences);
            var dataTableDto = new DataTableDto(dataTable);

            var gvar = new GVAR();
            gvar.DicOfDT["PolygonGeofences"] = dataTableDto.ToDataTable();

            return Ok(new { DicOfDT = new { PolygonGeofences = dataTableDto.Rows } });
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
