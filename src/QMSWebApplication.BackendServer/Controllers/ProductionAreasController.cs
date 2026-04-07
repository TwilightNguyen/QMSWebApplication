using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.ProductionArea;

namespace QMSWebApplication.BackendServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class ProductionAreasController(ApplicationDbContext context) : ControllerBase
    {
        // Controller code will go here

        public readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Url: /api/productionarea/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = _context.ProductionAreas.ToList();

            if (result == null || result.Count == 0) return NotFound("No production areas found.");

            var areaVms = result.Select(area => new ProductionAreaVm
            {
                Id = area.Id,
                Name = area.Name??string.Empty
            }).ToList();

            return Ok(areaVms);
        }

        /// <summary>
        /// Url: /api/productionarea/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var area = _context.ProductionAreas.FirstOrDefault(r => r.Id == Id);

            if (area == null)
            {
                return NotFound("Production Area not found.");
            }

            var areaVm = new ProductionAreaVm
            {
                Id = area.Id,
                Name = area.Name ?? string.Empty
            };
            return Ok(areaVm);
        }
    }
}
