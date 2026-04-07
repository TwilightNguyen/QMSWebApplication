using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.MeasureType;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class SampleTypesController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        /// <summary>
        ///  URL: /api/measuretypes
        /// </summary> 
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMeasureType([FromBody] MeasureTypeCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Measure type name cannot be empty.");
            }

            var measureTypeExist = _context.MeasureTypes.FirstOrDefault(p => p.Name == request.Name);

            if (measureTypeExist != null)
            {
                return BadRequest("Measure type with the same name already exists.");
            }

            var measureType = new SampleTypes
            {
                Name = request.Name,
            }; 


            _context.MeasureTypes.Add(measureType);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return CreatedAtAction(
                    nameof(GetById),
                    new {measureType.Id },
                    new MeasureTypeVm
                    {
                        Id = measureType.Id,
                        Name = measureType.Name ?? string.Empty,
                    }
                );
            }
            else
            {
                return BadRequest("Failed to create measure type.");
            }
        }

        /// <summary>
        /// Url: /api/measuretypes/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var measureTypes = _context.MeasureTypes.ToList();

            if (measureTypes.Count == 0)
                return NotFound("No measure types found.");
            var measureTypeVms = measureTypes.Select(measureType => new MeasureTypeVm
            {
                Id = measureType.Id,
                Name = measureType.Name??string.Empty,
            });
            return Ok(measureTypeVms);
        }

        /// <summary>
        /// Url: /api/measuretypes/?filter=serchString&pageIndex=1&pageSize=10
        /// </summary>
        /// <returns></returns>
        [HttpGet("Pagging")]
        public async Task<IActionResult> GetPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.MeasureTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(r => r.Name!.Contains(filter));
            }

            List<MeasureTypeVm> items = [.. query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(measureType => new MeasureTypeVm{
                    Id = measureType.Id,
                    Name = measureType.Name??string.Empty,
                })];

            var paginaton = new Pagination<MeasureTypeVm>()
            {
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(paginaton);
        }

        /// <summary>
        /// Url: /api/measuretypes/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var measureType = _context.MeasureTypes.FirstOrDefault(r => r.Id == Id);

            if (measureType == null)
            {
                return NotFound("Measure type not found.");
            }

            var measureTypeVm = new MeasureTypeVm
            {
                Id = measureType.Id,
                Name = measureType.Name ?? string.Empty,
            };
            return Ok(measureTypeVm);
        }


        /// <summary>
        /// Url: /api/characteristics/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateMeasureType(int Id, MeasureTypeVm measureTypeVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id < 0 || Id != measureTypeVm.Id)
            {
                return BadRequest("Invalid measure type ID.");
            }

            if (string.IsNullOrWhiteSpace(measureTypeVm.Name))
            {
                return BadRequest("Measure type name cannot be empty.");
            }

            if (_context.MeasureTypes.Any(p => p.Name == measureTypeVm.Name && p.Id != Id))
            {
                return BadRequest("Measure type with the same name already exists.");
            }

            var measureType = _context.MeasureTypes.FirstOrDefault(r => r.Id == Id);

            if (measureType == null)
            {
                return NotFound("Measure type not found.");
            }

            measureType.Name = measureTypeVm.Name;

             _context.MeasureTypes.Update(measureType);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(new MeasureTypeVm
                {
                    Id = measureType.Id,
                    Name = measureType.Name ?? string.Empty,
                });
            }
            else
            {
                return BadRequest("Failed to update characteristic.");
            }
        }

        /// <summary>
        /// Url: /api/measuretypes/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteMeasureType(int Id)
        {
            var measureType = _context.MeasureTypes.FirstOrDefault(r => r.Id == Id);

            if (measureType == null)
            {
                return NotFound("Measure type not found.");
            }

            _context.MeasureTypes.Remove(measureType);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
            {
                return Ok(new MeasureTypeVm
                {
                    Id = measureType.Id,
                    Name = measureType.Name ?? string.Empty,
                });
            }
            else
            {
                return BadRequest("Failed to delete measure type.");
            }
        }
    }
}
