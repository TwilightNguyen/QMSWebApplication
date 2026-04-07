using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.Function;
using QMSWebApplication.ViewModels.System.InspectionPlan;

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

            if (String.IsNullOrWhiteSpace(request.Name)) { 
                return BadRequest("Function name is required.");
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

            int maxOrderNumber = _context.Functions.Any() ? _context.Functions.Max(f => f.DisplayOrder ?? 0) : 0;

            var function = new Functions
            {
                Name = request.Name,
                Url = request.Url,
                DisplayOrder = maxOrderNumber + 1,
                ParentId = request.ParentId > 0 ? request.ParentId : -1,
                UploadedDateTime = DateTimeOffset.Now,
                Enabled = true
            };

            _context.Functions.Add(function);
            var result = await _context.SaveChangesAsync();
            if(result > 0)
            {
                return CreatedAtAction(
                    nameof(GetById),
                    new { function.Id },
                    new FunctionVm
                    {
                        Id = function.Id,
                        Name = function.Name,
                        Url = function.Url,
                        ParentId = function.ParentId,
                        UploadedDateTime = function.UploadedDateTime,
                        ModifiedDateTime = function.ModifiedDateTime,
                        Enabled = function.Enabled,
                        DisplayOrder = function.DisplayOrder
                    }
                );
            }
            else
            {
                return BadRequest("Failed to create function.");
            }
        }


        /// <summary>
        /// Url: /api/functions/
        /// </summary>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var functions = await _context.Functions.OrderBy(x => x.DisplayOrder).ToListAsync();
            if (functions == null || functions.Count == 0)
            {
                return NotFound("No functions found.");
            }
            var functionVms = functions.Select(function => new FunctionVm
            {
                Id = function.Id,
                Name = function.Name,
                Url = function.Url,
                DisplayOrder = function.DisplayOrder,
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
                DisplayOrder = function.DisplayOrder,
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
                .OrderBy(x => x.DisplayOrder)
                .Select(function => new FunctionVm
                {
                    Id = function.Id,
                    Name = function.Name,
                    DisplayOrder = function.DisplayOrder,
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

            if(String.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Function name is required.");
            }

            var function = _context.Functions.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if (function == null)
            {
                return NotFound($"Function with Id {Id} not found.");
            }

            var funcExists = _context.Functions.FirstOrDefault(x =>
                x.Name == request.Name && x.Id != Id);

            if (funcExists != null)
            {
                return BadRequest("Function with the same name already exists.");
            }

            if (request.ParentId != null && request.ParentId > 0)
            {
                var parentFunction = _context.Functions.FirstOrDefault(f => f.Id == request.ParentId);
                if (parentFunction == null)
                {
                    return BadRequest("Parent function not found.");
                }
            }

            function.Name = request.Name;
            function.Url = request.Url;
            function.ModifiedDateTime = DateTimeOffset.Now; 
            function.ParentId = request.ParentId > 0 ? request.ParentId : -1;

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
                    DisplayOrder = function.DisplayOrder,
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
                    DisplayOrder = function.DisplayOrder,
                    Enabled = function.Enabled,
                });
            }
            else
            {
                return BadRequest("Failed to delete function.");
            }
        }
    }
}
