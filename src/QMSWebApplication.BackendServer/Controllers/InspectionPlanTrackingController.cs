using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.InspectionPlanTracking;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class InspectionPlanTrackingController (ApplicationDbContext context): ControllerBase
    {
        public readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Url: /api/inspectionplantracking/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var inspPlanTrackings = _context.InspectionPlanTracking.ToList();

            if (inspPlanTrackings == null) return NotFound("No inspection plan tracking found.");

            var inspPlanTrackingVms = inspPlanTrackings.Select(inspectionPlanTracking => new InspectionPlanTrackingVm
            {
                InspPlanId = inspectionPlanTracking.InspPlanId,
                Id = inspectionPlanTracking.Id,
                PlanState = inspectionPlanTracking.PlanState,
                //PlanTypeId = inspectionPlanTracking.PlanTypeId,
                UploadedDateTime = inspectionPlanTracking.UploadedDateTime,
                UserId = inspectionPlanTracking.UserId,
            });


            return Ok(inspPlanTrackingVms);
        }

        /// <summary>
        /// Url: /api/inspectionplantracking/Pagging/?pageIndex=1&pageSize=10
        /// </summary>
        /// <returns></returns>
        [HttpGet("Pagging")]
        public async Task<IActionResult> GetPaging(int pageIndex, int pageSize)
        {
            var baseQuery = _context.InspectionPlanTracking.AsNoTracking();

            var totalRecords = await baseQuery.CountAsync();

            var skip = (pageIndex - 1) * pageSize;

            var pageQuery =
                from ipt in baseQuery
                join u in _context.Users
                    on ipt.UserId equals u.IntUserID into gj   // đổi 'Id' đúng cột User
                join p in _context.InspectionPlans
                    on ipt.InspPlanId equals p.Id into pgj
                join pt in _context.InspPlanTypes
                    on ipt.PlanTypeId equals pt.Id into ptgj
                from u in gj.DefaultIfEmpty()
                from p in pgj.DefaultIfEmpty()
                from pt in ptgj.DefaultIfEmpty()
                select new InspectionPlanTrackingVm
                {
                    InspPlanId = ipt.InspPlanId,
                    Id = ipt.Id,
                    PlanState = ipt.PlanState,
                    PlanTypeId = ipt.PlanTypeId,
                    UploadedDateTime = ipt.UploadedDateTime,
                    UserId = ipt.UserId,
                    UserName = u.UserName,
                    InspPlanName = p.Name,
                    PlanTypeName = pt.Name
                };

            var items = await pageQuery
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var pagination = new Pagination<InspectionPlanTrackingVm>
            {
                Items = items,
                TotalRecords = totalRecords
            };

            return Ok(pagination);
        }

        /// <summary>
        /// Url: /api/inspectionplantracking/GetByInspPlanIdAndPlanTypeId/?intId=1planTypeId=1
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetByInspPlanIdAndPlanTypeId")]
        public async Task<IActionResult> GetByInspPlanIdAndPlanTypeId(int Id, int? planTypeId = -1)
        {
            var baseQuery = _context.InspectionPlanTracking
                .Where(x => x.InspPlanId == Id && (planTypeId == -1 || x.PlanTypeId == planTypeId))
                .AsNoTracking();

            var pageQuery =
                from ipt in baseQuery
                join u in _context.Users
                    on ipt.UserId equals u.IntUserID into gj   // đổi 'Id' đúng cột User
                join p in _context.InspectionPlans
                    on ipt.InspPlanId equals p.Id into pgj
                join pt in _context.InspPlanTypes
                    on ipt.PlanTypeId equals pt.Id into ptgj
                from u in gj.DefaultIfEmpty()
                from p in pgj.DefaultIfEmpty()
                from pt in ptgj.DefaultIfEmpty()
                select new InspectionPlanTrackingVm
                {
                    InspPlanId = ipt.InspPlanId,
                    Id = ipt.Id,
                    PlanState = ipt.PlanState,
                    PlanTypeId = ipt.PlanTypeId,
                    UploadedDateTime = ipt.UploadedDateTime,
                    UserId = ipt.UserId,
                    UserName = u.UserName,
                    InspPlanName = p.Name,
                    PlanTypeName = pt.Name
                };

            

            var inspectionPlanTrackings = await pageQuery
                .OrderBy(x => x.UploadedDateTime)
                .ToListAsync();
            
            if (!await pageQuery.AnyAsync())
            {
                return NotFound("No inspection plan tracking found for the given Inspection Plan and Plan Type.");
            }

            return Ok(inspectionPlanTrackings);
        }
    }
}
