using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.BackendServer.Services;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.EventLog;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class EventLogController(
        ApplicationDbContext context
    ) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context; 

        /// <summary>
        /// Url: /api/eventlogs/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var eventLogs = _context.EventLogs.ToList();

            if (eventLogs == null || eventLogs.Count == 0)
            {
                return NotFound("No event logs found.");
            }

            var eventLogVms = eventLogs.Select(eventLog => new EventLogVm
            {
                Id = eventLog.Id,
                EventTime = eventLog.EventTime,
                EventCode = eventLog.EventCode,
                Description = eventLog.Description,
                Station = eventLog.Station,
            });


            return Ok(eventLogVms);
        }

        /// <summary>
        /// Url: /api/eventlogs/?filter=serchString&pageIndex=1&pageSize=10
        /// </summary>
        /// <returns></returns>
        [HttpGet("Pagging")]
        public async Task<IActionResult> GetPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.EventLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(r => r.Station!.Contains(filter));
            }

            List<EventLogVm> items = [.. query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(role => new EventLogVm{
                    Id = role.Id,
                    EventTime = role.EventTime,
                    EventCode = role.EventCode,
                    Description = role.Description,
                    Station = role.Station,
                })];
            
            if (items.Count == 0)
            {
                return NotFound("No event logs found.");
            }

            var paginaton = new Pagination<EventLogVm>()
            {
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(paginaton);
        }

        /// <summary>
        /// Url: /api/eventlogs/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var eventLog = _context.EventLogs.FirstOrDefault(r => r.Id == Id);

            if (eventLog == null)
            {
                return NotFound("Event log not found.");
            }
            var eventLogVm = new EventLogVm
            {
                Id = eventLog.Id,
                EventTime = eventLog.EventTime,
                EventCode = eventLog.EventCode,
                Description = eventLog.Description,
                Station = eventLog.Station,
            }; 
            return Ok(eventLogVm);
        }
    }
}
