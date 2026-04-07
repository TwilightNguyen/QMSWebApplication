using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.TVDisplay;

namespace QMSWebApplication.BackendServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class TVDisplaysController(ApplicationDbContext context) : ControllerBase
    {
        public readonly ApplicationDbContext _context = context;


        /// <summary>
        /// Url /api/tvdisplays
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tvDisplays = await _context.TVDisplays.ToListAsync();

            if (tvDisplays == null || tvDisplays.Count == 0)
            {
                return NotFound("No Tv Display found.");
            }

            var tvDisplayVm = tvDisplays.Select(tvDisplay => new TVDisplayVm
            {
                Id = tvDisplay.Id,
                Name = tvDisplay.Name
            });

            return Ok(tvDisplayVm);
        }

        ///<summary>
        /// Url: /api/tvdisplays
        /// </summary>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var tvDisplay = await _context.TVDisplays.FirstOrDefaultAsync(tv => tv.Id == Id);

            if(tvDisplay == null)
            {
                return NotFound("Tv Display not found.");
            }

            var tvDisplayVm = new TVDisplayVm
            {
                Id = tvDisplay.Id,
                Name = tvDisplay.Name,
            };

            return Ok(tvDisplayVm);
        }
    }
}
