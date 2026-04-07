using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.InspectionPlanSub;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class InspectionPlanSubsController(ApplicationDbContext context) : ControllerBase
    {
        public readonly ApplicationDbContext _context = context;

        /// <summary>
        ///  URL: /api/inspectionplansubs/2
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateInspectionPlanSub([FromBody] InspectionPlanSubCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inspPlan = _context.InspectionPlans.FirstOrDefault(p => p.Id == request.InspPlanId && p.Enabled == true);

            if (inspPlan == null)
            {
                return BadRequest("Invalid Inspection Plan.");
            }

            var inspPlanType = _context.InspPlanTypes.FirstOrDefault(p => p.Id == request.PlanTypeId);

            if (inspPlanType == null)
            {
                return BadRequest("Invalid Inspection Plan Type.");
            }

            var charExists = _context.InspectionPlanSubs.FirstOrDefault(x =>
                x.InspPlanId == request.InspPlanId &&
                x.PlanTypeId == request.PlanTypeId &&
                x.Enabled == true);

            if (charExists != null)
            {
                return BadRequest("Inspection Plan Sub with the same Inspection Plan and Plan Type already exists.");
            }

            var inspectionPlanSub = new InspectionPlanSubs
            {
                InspPlanId = request.InspPlanId,
                PlanTypeId = request.PlanTypeId,
                UploadedDateTime = DateTimeOffset.Now,
                Enabled = false,
            };

            _context.InspectionPlanSubs.Add(inspectionPlanSub);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return CreatedAtAction(
                    nameof(GetById),
                    new { inspectionPlanSub.Id },
                    new InspectionPlanSubVm
                    {
                        Id = inspectionPlanSub.Id,
                        InspPlanId = inspectionPlanSub.InspPlanId,
                        PlanTypeId = inspectionPlanSub.PlanTypeId,
                        UploadedDateTime = inspectionPlanSub.UploadedDateTime,
                    }
                );
            }
            else
            {
                return BadRequest("Failed to create Inspection Plan Sub.");
            }
        }

        /// <summary>
        /// Url: /api/inspectionplansubs/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var inspectionPlanSubs = _context.InspectionPlanSubs.Where(r => r.Enabled == true).ToList();

            if (inspectionPlanSubs == null) return NotFound("No inspection plan sub found.");

            var inspectionPlanSubVms = inspectionPlanSubs.Select(inspectionPlanSub => new InspectionPlanSubVm
            {
                Id = inspectionPlanSub.Id,
                InspPlanId = inspectionPlanSub.InspPlanId,
                PlanTypeId = inspectionPlanSub.PlanTypeId,
                UploadedDateTime = inspectionPlanSub.UploadedDateTime,
            });

            return Ok(inspectionPlanSubVms);
        }

        /// <summary>
        /// Url: /api/inspectionplansubs/?pageIndex=1&pageSize=10
        /// </summary>
        /// <returns></returns>
        [HttpGet("Pagging")]
        public async Task<IActionResult> GetPaging(int pageIndex, int pageSize)
        {
            var query = _context.InspectionPlanSubs.AsQueryable();

           
            List<InspectionPlanSubVm> items = [.. query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(inspectionPlan => new InspectionPlanSubVm
                {
                    Id = inspectionPlan.Id,
                    InspPlanId = inspectionPlan.InspPlanId,
                    PlanTypeId = inspectionPlan.PlanTypeId,
                    UploadedDateTime = inspectionPlan.UploadedDateTime,
                })];

            var paginaton = new Pagination<InspectionPlanSubVm>()
            {
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(paginaton);
        }

        /// <summary>
        /// Url: /api/inspectionplansubs/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var inspectionPlanSub = _context.InspectionPlanSubs.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if (inspectionPlanSub == null)
            {
                return NotFound("Inspection Plan Sub not found.");
            }

            var inspectionPlanSubVm = new InspectionPlanSubVm
            {
                Id  = inspectionPlanSub.Id,
                InspPlanId = inspectionPlanSub.InspPlanId,
                PlanTypeId = inspectionPlanSub.PlanTypeId,
                UploadedDateTime = inspectionPlanSub.UploadedDateTime,
            };

            return Ok(inspectionPlanSubVm);
        }


        /// <summary>
        /// Url: /api/inspectionplansubs/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("/GetByInsPlanId/{InsPlanId:int}")]
        public async Task<IActionResult> GetByInsPlanId(int InsPlanId)
        {
            var inspectionPlanSubs = _context.InspectionPlanSubs.Where(r => r.Id == InsPlanId && r.Enabled == true);

            if (inspectionPlanSubs == null || inspectionPlanSubs?.Count() == 0)
            {
                return NotFound("No Inspection Plan Subs found for the given Inspection Plan.");
            }

            var InspectionPlanSubVms = inspectionPlanSubs?.Select(inspectionPlanSub => new InspectionPlanSubVm
            {
                Id = inspectionPlanSub.Id,
                InspPlanId = inspectionPlanSub.InspPlanId,
                PlanTypeId = inspectionPlanSub.PlanTypeId,
                UploadedDateTime = inspectionPlanSub.UploadedDateTime,
            });

            return Ok(InspectionPlanSubVms);
        }

        /// <summary>
        /// Url: /api/inspectionplansubs/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteInspectionPlanSub(int Id)
        {
            var inspectionPlanSub = _context.InspectionPlanSubs.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if (inspectionPlanSub == null)
            {
                return NotFound("Inspection Plan Sub not found.");
            }

            inspectionPlanSub.Enabled = false;
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(new InspectionPlanSubVm
                {
                    Id = inspectionPlanSub.Id,
                    InspPlanId = inspectionPlanSub.InspPlanId,
                    PlanTypeId = inspectionPlanSub.PlanTypeId,
                    UploadedDateTime = inspectionPlanSub.UploadedDateTime,
                });
            }
            else
            {
                return BadRequest("Failed to delete Inspection Plan Sub.");
            }
        }
    }
}
