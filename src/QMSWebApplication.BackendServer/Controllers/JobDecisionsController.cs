using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.JobDecision;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class JobDecisionsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        ///<summary>
        /// /api/jobdecisions
        /// </summary>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var jobDecisions = await _context.JobDecisions.ToListAsync();

            if (jobDecisions == null || jobDecisions.Count == 0) {
                return BadRequest("No job decision found.");
            }

            var jobDecisionVms = jobDecisions.Select(jobDecision => new JobDecisionVm
            {
                Id = jobDecision.Id,
                Decision = jobDecision.Decision,
                ColorCode = jobDecision.ColorCode,
            });

            return Ok(jobDecisionVms);
        }
    }
}
