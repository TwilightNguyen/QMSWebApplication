using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.Process;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class ProcessesController(ApplicationDbContext context) : ControllerBase
    {
        // Controller code will go here

        public readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Url: /api/processes/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = _context.Processes.ToList();

            if (result == null || result.Count == 0) return NotFound("No processes found.");

            var processVms = result.Select(process => new ProcessVm
            {
                Id = process.Id,
                Name = process.Name ?? string.Empty,
                AreaId = process.AreaId
            }).ToList();

            return Ok(processVms);
        }

        /// <summary>
        /// Url: /api/proccess/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var process = _context.Processes.FirstOrDefault(r => r.Id == Id);

            if (process == null)
            {
                return NotFound("Process not found.");
            }

            var processVm = new ProcessVm
            {
                Id = process.Id,
                Name = process.Name ?? string.Empty,
                AreaId = process.AreaId
            };
            return Ok(processVm);
        }

        /// <summary>
        /// Url: /api/proccess/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpGet("GetByAreaId/{AreaId:int}")]
        public async Task<IActionResult> GetByAreaId(int AreaId)
        {
            var area = _context.ProductionAreas.FirstOrDefault(r => r.Id == AreaId);
            if (area == null)
            {
                return NotFound("Production Area not found.");
            }

            var result = _context.Processes.Where(x => x.AreaId == AreaId).ToList();

            if (result == null || result.Count == 0) return NotFound("No processes found.");

            var processVms = result.Select(process => new ProcessVm
            {
                Id = process.Id,
                Name = process.Name ?? string.Empty,
                AreaId = process.AreaId
            }).ToList();

            return Ok(processVms);
        }
    }
}
