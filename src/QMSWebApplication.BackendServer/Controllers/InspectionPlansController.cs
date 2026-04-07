using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.InspectionPlan;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class InspectionPlansController(ApplicationDbContext context) : ControllerBase
    {
        public readonly ApplicationDbContext _context = context;

        /// <summary>
        ///  URL: /api/inspectionplans/
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateInspectionPlan([FromBody] InspectionPlanCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Inspection Plan name cannot be empty.");
            }

            var area = _context.ProductionAreas.FirstOrDefault(p => p.Id == request.AreaId);

            if (area == null)
            {
                return BadRequest("Invalid Production Area.");
            }
            
            var charExists = _context.InspectionPlans.FirstOrDefault(x =>
                x.Name == request.Name &&
                x.AreaId == request.AreaId &&
                x.Enabled == true);

            if (charExists != null)
            {
                return BadRequest("Inspection Plan with the same name already exists in this Production Area.");
            }


            var inspectionPlan = new InspectionPlans
            {
                Name = request.Name,
                AreaId = request.AreaId,
                ModifiedDateTime = DateTimeOffset.Now,
                Enabled = true,
            };

            _context.InspectionPlans.Add(inspectionPlan);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return CreatedAtAction(
                    nameof(GetById),
                    new { inspectionPlan.Id },
                    new InspectionPlanVm {                         
                        Id = inspectionPlan.Id,
                        Name = inspectionPlan.Name,
                        AreaId = inspectionPlan.AreaId,
                    }
                );
            }
            else
            {
                return BadRequest("Failed to create Inspection Plan.");
            }
        }

        /// <summary>
        /// Url: /api/inspectionplans/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var inspectionPlans = _context.InspectionPlans.Where(r => r.Enabled == true).ToList();

            if (inspectionPlans == null) return NotFound("No inspection plan found.");

            var inspectionPlanVms = inspectionPlans.Select(inspectionPlan => new InspectionPlanVm
            {
                Id = inspectionPlan.Id,
                Name = inspectionPlan.Name,
                AreaId = inspectionPlan.AreaId,
                UploadedDateTime = inspectionPlan.UploadedDateTime,
                ModifiedDateTime = inspectionPlan.ModifiedDateTime,
            });


            return Ok(inspectionPlanVms);
        }

        /// <summary>
        /// Url: /api/inspectionplans/?filter=serchString&pageIndex=1&pageSize=10
        /// </summary>
        /// <returns></returns>
        [HttpGet("Pagging")]
        public async Task<IActionResult> GetPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.InspectionPlans.Where(r => r.Enabled == true).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(r => r.Name!.Contains(filter));
            }

            List<InspectionPlanVm> items = [.. query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(inspectionPlan => new InspectionPlanVm
                {
                    Id = inspectionPlan.Id, 
                    Name = inspectionPlan.Name,
                    AreaId = inspectionPlan.AreaId,
                    UploadedDateTime = inspectionPlan.UploadedDateTime,
                    ModifiedDateTime = inspectionPlan.ModifiedDateTime,
                })];

            var paginaton = new Pagination<InspectionPlanVm>()
            {
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(paginaton);
        }

        /// <summary>
        /// Url: /api/inspectionplans/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var inspectionPlan = _context.InspectionPlans.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if (inspectionPlan == null)
            {
                return NotFound("Inspection Plan not found.");
            }

            var inspectionPlanVm = new InspectionPlanVm
            {
                Id = inspectionPlan.Id,
                Name = inspectionPlan.Name,
                AreaId = inspectionPlan.AreaId,
                UploadedDateTime = inspectionPlan.UploadedDateTime,
                ModifiedDateTime = inspectionPlan.ModifiedDateTime,
            };

            return Ok(inspectionPlanVm);
        }


        /// <summary>
        /// Url: /api/inspectionplans/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("/GetByAreaId/{AreaId:int}")]
        public async Task<IActionResult> GetByAreaId(int AreaId)
        {
            var inspectionPlans = _context.InspectionPlans.Where(r => r.AreaId == AreaId && r.Enabled == true);

            if (inspectionPlans == null || inspectionPlans?.Count() == 0)
            {
                return NotFound("No inspection plan found for the specified production area.");
            }

            var InspectionPlanVms = inspectionPlans?.Select(inspectionPlan => new InspectionPlanVm
            {
                Id = inspectionPlan.Id,
                Name = inspectionPlan.Name,
                AreaId = inspectionPlan.AreaId,
                UploadedDateTime = inspectionPlan.UploadedDateTime,
                ModifiedDateTime = inspectionPlan.ModifiedDateTime,
            });

            return Ok(InspectionPlanVms);
        }


        /// <summary>
        /// Url: /api/inspectionplans/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateInspectionPlan(int Id, InspectionPlanVm inspectionPlanVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id < 0 || Id != inspectionPlanVm.Id)
            {
                return BadRequest("Invalid inspection plan Id.");
            }

            var inspectionPlan = _context.InspectionPlans.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if (inspectionPlan == null)
            {
                return NotFound("Inspection Plan not found.");
            }

            if (string.IsNullOrWhiteSpace(inspectionPlanVm.Name))
            {
                return BadRequest("Inspection Plan name cannot be empty.");
            }

            var insPlanExists = _context.InspectionPlans.FirstOrDefault(x =>
                x.Name == inspectionPlanVm.Name &&
                x.AreaId == inspectionPlanVm.AreaId &&
                x.Enabled == true &&
                x.Id != inspectionPlanVm.Id);

            if (insPlanExists != null)
            {
                return BadRequest("Inspection plan with the same name already exists in this production area.");
            }

            inspectionPlan.Name = inspectionPlanVm.Name;
            inspectionPlan.ModifiedDateTime = DateTimeOffset.Now;


            _context.InspectionPlans.Update(inspectionPlan);

            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
            {
                return Ok(new InspectionPlanVm
                {
                    Id = inspectionPlan.Id,
                    Name = inspectionPlan.Name,
                    AreaId = inspectionPlan.AreaId,
                    UploadedDateTime = inspectionPlan.UploadedDateTime,
                    ModifiedDateTime = inspectionPlan.ModifiedDateTime,
                });
            }
            else
            {
                return BadRequest("Failed to update inspection plan.");
            }
        }

        /// <summary>
        /// Url: /api/inspectionplans/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteInspectionPlan(int Id)
        {
            var inspectionPlan = _context.InspectionPlans.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if (inspectionPlan == null)
            {
                return NotFound("Inspection plan not found.");
            }

            inspectionPlan.Enabled = false;
            _context.InspectionPlans.Update(inspectionPlan);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(new InspectionPlanVm
                {
                    Id = inspectionPlan.Id,
                    Name = inspectionPlan.Name,
                    AreaId = inspectionPlan.AreaId,
                    UploadedDateTime = inspectionPlan.UploadedDateTime,
                    ModifiedDateTime = inspectionPlan.ModifiedDateTime,
                });
            }
            else
            {
                return BadRequest("Failed to delete inspection plan.");
            }
        }
    }
}
