using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.Function;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class FunctionsController(ApplicationDbContext context) : ControllerBase
    {
        public readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Url: /api/functions/
        /// </summary>
        /// 
        [HttpPost]
        public async Task<IActionResult> CreateFunction([FromBody] FunctionCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int existingFunctionCount = _context.Functions.Count(f => f.Name == request.Name);
            if (existingFunctionCount != 0)
            {
                return BadRequest("Function with the same name already exists.");
            }

            if(request.ParentId != null && request.ParentId > 0) 
            {
                var parentFunction = _context.Functions.FirstOrDefault(f => f.Id == request.ParentId);
                if (parentFunction == null)
                {
                    return BadRequest("Parent function not found.");
                }
            }

            int maxOrderNumber = _context.Functions.Any() ? _context.Functions.Max(f => f.OrderNumber ?? 0) : 0;

            var function = new Functions
            {
                Name = request.Name,
                Url = request.Url,
                OrderNumber = maxOrderNumber + 1,
                ParentId = request.ParentId,
                UploadedDateTime = DateTimeOffset.Now,
                Enabled = true
            };

            _context.Functions.Add(function);
            await _context.SaveChangesAsync();
            return Ok(function);
        }


        /// <summary>
        /// Url: /api/functions/
        /// </summary>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var functions = _context.Functions.ToList();
            if (functions == null || functions.Count == 0)
            {
                return NotFound("No functions found.");
            }
            var functionVms = functions.Select(function => new FunctionVm
            {
                Id = function.Id,
                Name = function.Name,
                Url = function.Url,
                OrderNumber = function.OrderNumber,
                ParentId = function.ParentId > 0 ? function.ParentId : -1,
                UploadedDateTime = function.UploadedDateTime,
                ModifiedDateTime = function.ModifiedDateTime,
                Enabled = function.Enabled,
            });
            return Ok(functionVms);

        }

        /// <summary>
        /// Url: /api/functions/{id}
        /// </summary>
        /// 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var function = _context.Functions.FirstOrDefault(f => f.Id == id);
            if (function == null)
            {
                return NotFound($"Function with Id {id} not found.");
            }
            var functionVm = new FunctionVm
            {
                Id = function.Id,
                Name = function.Name,
                Url = function.Url,
                OrderNumber = function.OrderNumber,
                ParentId = function.ParentId,
                UploadedDateTime = function.UploadedDateTime,
                ModifiedDateTime = function.ModifiedDateTime,
                Enabled = function.Enabled
            };
            return Ok(functionVm);
        }

        /// <summary>
        /// Url: /api/functions/pagging/?filter=serchString&pageIndex=1&pageSize=10
        /// </summary>
        /// <returns></returns>
        [HttpGet("Pagging")]
        public async Task<IActionResult> GetPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.Functions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(r => r.Name!.Contains(filter));
            }

            List<FunctionVm> items = [.. query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(function => new FunctionVm
                {
                    Id = function.Id,
                    Name = function.Name,
                    OrderNumber = function.OrderNumber,
                    ParentId = function.ParentId,
                    Url = function.Url,
                    UploadedDateTime = function.UploadedDateTime,
                    ModifiedDateTime = function.ModifiedDateTime,
                    Enabled = function.Enabled,

                })];

            var paginaton = new Pagination<FunctionVm>()
            {
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(paginaton);
        }


        /// <summary>
        /// Url: /api/functions/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateFunction(int Id, FunctionCreateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var function = _context.Functions.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if (function == null)
            {
                return NotFound($"Function with Id {Id} not found.");
            }

            var funcExists = _context.Functions.FirstOrDefault(x =>
                x.Name == function.Name);

            if (funcExists != null)
            {
                return BadRequest("Function with the same name already exists.");
            }

            function.Name = request.Name;
            function.Url = request.Url;
            function.ModifiedDateTime = DateTimeOffset.Now; 
            function.ParentId = request.ParentId;

            _context.Functions.Update(function);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(new FunctionVm
                {
                    Id = function.Id,
                    Name = function.Name,
                    Url = function.Url,
                    UploadedDateTime = function.UploadedDateTime,
                    ModifiedDateTime = function.ModifiedDateTime,
                    Enabled = function.Enabled,
                });
            }
            else
            {
                return BadRequest("Failed to update Function.");
            }
        }

        /// <summary>
        /// Url: /api/functions/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteFunction(int Id)
        {
            var function = _context.Functions.FirstOrDefault(r => r.Id == Id);

            if (function == null)
            {
                return NotFound($"Function with Id {Id} not found.");
            }

             _context.Functions.Remove(function);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(new FunctionVm
                {
                    Id = function.Id,
                    Name = function.Name,
                    Url = function.Url,
                    UploadedDateTime = function.UploadedDateTime,
                    ModifiedDateTime = function.ModifiedDateTime,
                    Enabled = function.Enabled,
                });
            }
            else
            {
                return BadRequest("Failed to delete inspection plan.");
            }
        }
    }
}
