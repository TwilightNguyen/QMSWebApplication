using IdentityServer8.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.Job;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class JobsController(ApplicationDbContext context): ControllerBase
    { 
        private readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Url: /api/jobs
        /// </summary>
        /// 
        [HttpPost]
        public async Task<IActionResult> CreateJob([FromBody] JobCreateRequest request)
        {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(request.JobCode))
            {
                return BadRequest("Job Code cannot be empty."); 
            }

            var area = await _context.ProductionAreas.FirstOrDefaultAsync(pa => pa.Id == request.AreaId);

            if (area == null)
            {
                return BadRequest("Invalid Production Area.");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId && p.Enabled == true);

            if (product == null) {
                return BadRequest("Invalid Product.");
            }

            // Typical claim types (depends on your token/issuer)
            var userId = User?.FindFirst("sub")?.Value
                     ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var jobExists = await _context.JobDatas.FirstOrDefaultAsync(j => 
                j.JobCode == request.JobCode && 
                j.AreaId == request.AreaId &&
                j.Enabled == true );

            if (jobExists != null) {
                return BadRequest("Job with the same Job Code already exists in this Production Area.");
            }



            var iuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            var job = new Jobs
            {
                AreaId = request.AreaId,
                ProductId = request.ProductId,
                JobDecisionId = 1,
                JobCode = request.JobCode,
                POCode = request.POCode,
                SOCode = request.SOCode,
                PlannedQuantity = request.PlannedQuantity,
                OutputQuantity = request.OutputQuanlity,
                UserId = iuser?.IntUserID ?? -1,
                UploadedDateTime = DateTime.Now,
                Enabled = true,
            };

            await _context.JobDatas.AddAsync(job);
            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return CreatedAtAction(
                nameof(GetById),
                new {job.Id},
                new JobVm{
                    Id = job.Id,
                    AreaId = job.AreaId,
                    ProductId = job.ProductId,
                    JobCode = job.JobCode,
                    POCode = job.POCode,
                    SOCode = job.SOCode,
                    UserId = job.UserId,
                    UploadedDateTime = job.UploadedDateTime,
                    JobDecisionId = job.JobDecisionId,
                    PlannedQuanlity = job.PlannedQuantity,
                    OutputQuanlity = job.OutputQuantity,
                });
            }
            else
            {
                return BadRequest("Failed to create job.");
            }
        }

        ///<summary>
        /// Url: /api/jobs
        /// </summary>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobs = await _context.JobDatas.ToListAsync();

            if (jobs == null || jobs.Count == 0)
            {
                return BadRequest("No Job found.");
            }

            var jobVms = jobs.Select(job => new JobVm
            {
                Id = job.Id,
                AreaId = job.AreaId,
                ProductId = job.ProductId,
                JobCode = job.JobCode,
                POCode = job.POCode,
                SOCode = job.SOCode,
                JobDecisionId = job.JobDecisionId,
                PlannedQuanlity = job.PlannedQuantity,
                OutputQuanlity = job.OutputQuantity,
                UserId = job.UserId,
                UploadedDateTime = job.UploadedDateTime,
            });

            return Ok(jobVms);
        }

        /// <summary>
        /// Url: /api/jobs/paging
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("Paging")]
        public async Task<IActionResult> GetPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.JobDatas.Where(j => j.Enabled == true).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(j => j.JobCode!.Contains(filter));
            }

            List<JobVm> items = [..query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(job => new JobVm{ 
                    Id = job.Id,
                    JobCode=job.JobCode,
                    POCode=job.POCode,
                    SOCode  = job.SOCode,
                    JobDecisionId=job.JobDecisionId,
                    PlannedQuanlity = job.PlannedQuantity,
                    OutputQuanlity = job.OutputQuantity,
                    AreaId = job.AreaId,
                    ProductId = job.ProductId,
                    UploadedDateTime = job.UploadedDateTime,
                    UserId = job.UserId,
                })];

            var pagination = new Pagination<JobVm> { 
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(pagination);
        }


        ///<summary>
        /// Url /api/jobs
        /// </summary>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var job = await _context.JobDatas.FirstOrDefaultAsync(x => x.Id == Id);

            if(job == null)
            {
                return NotFound("Job not found.");
            }

            var jobVm = new JobVm
            {
                Id = job.Id,
                AreaId = job.AreaId,
                ProductId = job.ProductId,
                JobCode = job.JobCode,
                POCode = job.POCode,
                SOCode = job.SOCode,
                JobDecisionId = job.JobDecisionId,
                PlannedQuanlity = job.PlannedQuantity,
                OutputQuanlity = job.OutputQuantity,
                UploadedDateTime = job.UploadedDateTime,
                UserId = job.UserId,
            };

            return Ok(jobVm);
        }

        ///<summary>
        ///Url: /api/jobs/GetByAreaId
        /// </summary>
        /// 
        [HttpGet("GetByAreaId/{AreaId:int}")]
        public async Task<IActionResult> GetByAreaId(int AreaId) { 
            var jobs = await _context.JobDatas.Where(j => j.AreaId == AreaId).ToListAsync();

            if (jobs == null || jobs.Count == 0) {
                return BadRequest("No jobs found for the specificed Production Area.");
            }

            var jobVms = jobs.Select(job => new JobVm {
                Id = job.Id,
                AreaId = job.AreaId,
                ProductId = job.ProductId,
                JobCode = job.JobCode,
                POCode = job.POCode,
                SOCode = job.SOCode,
                JobDecisionId = job.JobDecisionId,
                PlannedQuanlity = job.PlannedQuantity,
                OutputQuanlity = job.OutputQuantity,
                UploadedDateTime = job.UploadedDateTime,
                UserId = job.UserId,
            });

            return Ok(jobVms);
        }

        /// <summary>
        /// Url: /api/jobs
        /// </summary>
        /// 
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateJob(int Id, JobVm jobVm) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(Id < 0 || Id != jobVm.Id)
            {
                return BadRequest("Invalid Job Id.");
            }

            var job = await _context.JobDatas.FirstOrDefaultAsync(j => 
                j.Id == jobVm.Id && 
                j.Enabled == true
            );

            if (job == null) {
                return NotFound("Job not found.");
            }

            if (string.IsNullOrWhiteSpace(jobVm.JobCode)) {
                return BadRequest("Job Code cannot be empty.");
            }

            var jobExists = await _context.JobDatas.FirstOrDefaultAsync(j =>
                j.JobCode == jobVm.JobCode &&
                j.AreaId == jobVm.AreaId &&
                j.Enabled == true
            );

            if (jobExists != null) {
                return BadRequest("Job with the same Job Code already exists in this Production Area.");
            }

            job.JobCode = jobVm.JobCode;
            job.POCode = jobVm.POCode;
            job.SOCode = jobVm.SOCode;
            job.JobDecisionId = jobVm.JobDecisionId;
            job.PlannedQuantity = jobVm.PlannedQuanlity;
            job.OutputQuantity = jobVm.OutputQuanlity;
            //job.IntProductID = jobVm.ProductId;

            _context.JobDatas.Update(job);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(new JobVm
                {
                    Id = job.Id,
                    AreaId = job.AreaId,
                    ProductId = job.ProductId,
                    JobCode = job.JobCode,
                    POCode = job.POCode,
                    SOCode = job.SOCode,
                    JobDecisionId = job.JobDecisionId,
                    PlannedQuanlity = job.PlannedQuantity,
                    OutputQuanlity = job.OutputQuantity,
                    UploadedDateTime = job.UploadedDateTime,
                    UserId = job.UserId,
                });
            }
            else
            {
                return BadRequest("Failed to update Job.");
            }
        }

        ///<summary>
        ///Url: /api/jobs
        /// </summary>
        /// 
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteJob(int Id)
        {
            var job = await _context.JobDatas.FirstOrDefaultAsync(j => j.Id == Id && j.Enabled == true);

            if(job == null)
            {
                return NotFound("Job not found.");
            }

            job.Enabled = false;

            _context.JobDatas.Update(job);

            var result = await _context.SaveChangesAsync();
            if (result > 0) {
                return Ok(new JobVm
                {
                    Id = job.Id,
                    AreaId = job.AreaId,
                    ProductId = job.ProductId,
                    JobCode = job.JobCode,
                    POCode = job.POCode,
                    SOCode = job.SOCode,
                    JobDecisionId = job.JobDecisionId,
                    PlannedQuanlity = job.PlannedQuantity,
                    OutputQuanlity = job.OutputQuantity,
                    UploadedDateTime = job.UploadedDateTime,
                    UserId = job.UserId,
                });
            }
            else
            {
                return BadRequest("Failed to delete job.");
            }
        }
    }
}
