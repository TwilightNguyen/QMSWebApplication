using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.Characteristic;
using Microsoft.EntityFrameworkCore;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class CharacteristicsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;


        /// <summary>
        ///  URL: /api/characteristics
        /// </summary>
        /// <param name="roleVm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCharacteristic([FromBody] CharacteristicCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Characteristic name cannot be empty.");
            }

            var process = _context.Processes.FirstOrDefault(p => p.Id == request.ProcessId);
            
            if(process == null)
            {
                return BadRequest("Invalid Process.");
            }

            var measType = _context.MeasureTypes.FirstOrDefault(m => m.Id == request.MeaTypeId);
            
            if (measType == null)
            {
                return BadRequest("Invalid Measure Type.");
            }

            if(!(new[] { 0, 1 }).Contains(request.DataType))
            {
                return BadRequest("Invalid Characteristic Type.");
            }

            if (request.Decimals < 0)
            {
                return BadRequest("Decimals cannot be negative.");
            }

            var charExists = _context.Characteristics.FirstOrDefault(x => 
                x.Name == request.Name && 
                x.ProcessId == request.ProcessId &&
                x.Enabled == true);

            if (charExists != null)
            {
                return BadRequest("Characteristic with the same name already exists in this process.");
            }

           
            var characteristic = new Data.Entities.Characteristics
            {
                Name = request.Name,
                MeaTypeId = request.MeaTypeId,
                ProcessId = request.ProcessId,
                DataType = request.DataType,
                Unit = request.Unit,
                DefectRateLimit = request.DefectRateLimit,
                EmailEventModel = request.EmailEventModel,
                Decimals = request.Decimals,
                Enabled = true,
            };

            _context.Characteristics.Add(characteristic);

            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return CreatedAtAction(
                    nameof(GetById),
                    new { characteristic.Id }, 
                    new CharacteristicVm
                    {
                        Id = characteristic.Id,
                        Name = characteristic.Name,
                        MeaTypeId = characteristic.MeaTypeId,
                        ProcessId = characteristic.ProcessId,
                        DataType = characteristic.DataType,
                        Unit = characteristic.Unit,
                        DefectRateLimit =  characteristic.DefectRateLimit,
                        EmailEventModel = characteristic.EmailEventModel,
                        Decimals = characteristic.Decimals,
                        Enabled = characteristic.Enabled,
                    }
                );
            }
            else
            {
                return BadRequest("Failed to create characteristic.");
            }
        }

        /// <summary>
        /// Url: /api/characteristics/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var characteristics = await _context.Characteristics.Where(r => r.Enabled == true).ToListAsync();

            if (characteristics == null) return NotFound("No characteristics found.");

            var characteristicVms = characteristics.Select(characteristic => new CharacteristicVm
            {
                Id = characteristic.Id,
                Name = characteristic.Name,
                MeaTypeId = characteristic.MeaTypeId,
                ProcessId = characteristic.ProcessId,
                DataType = characteristic.DataType,
                Unit = characteristic.Unit,
                DefectRateLimit = characteristic.DefectRateLimit, 
                EmailEventModel = characteristic.EmailEventModel,
                Decimals = characteristic.Decimals,
                Enabled = characteristic.Enabled,
            });
            

            return Ok(characteristicVms);
        }

        /// <summary>
        /// Url: /api/characteristics/?filter=serchString&pageIndex=1&pageSize=10
        /// </summary>
        /// <returns></returns>
        [HttpGet("Pagging")]
        public async Task<IActionResult> GetPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.Characteristics.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(r => r.Name!.Contains(filter) && r.Enabled == true);
            }

            List<CharacteristicVm> items = [.. query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(characteristic => new CharacteristicVm
                {
                    Id = characteristic.Id,
                    Name = characteristic.Name,
                    MeaTypeId = characteristic.MeaTypeId,
                    ProcessId = characteristic.ProcessId,
                    DataType = characteristic.DataType,
                    Unit = characteristic.Unit,
                    DefectRateLimit = characteristic.DefectRateLimit,
                    EmailEventModel = characteristic.EmailEventModel,
                    Decimals = characteristic.Decimals,
                    Enabled = characteristic.Enabled,
                })];

            var paginaton = new Pagination<CharacteristicVm>()
            {
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(paginaton);
        }

        /// <summary>
        /// Url: /api/characteristics/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var characteristic = _context.Characteristics.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if(characteristic == null)
            {
                return NotFound("Characteristic not found.");
            }

            var characteristicVm = new CharacteristicVm
            {
                Id = characteristic.Id,
                Name = characteristic.Name,
                MeaTypeId = characteristic.MeaTypeId,
                ProcessId = characteristic.ProcessId,
                DataType = characteristic.DataType,
                Unit = characteristic.Unit,
                DefectRateLimit = characteristic.DefectRateLimit,
                EmailEventModel = characteristic.EmailEventModel,
                Decimals = characteristic.Decimals,
                Enabled = characteristic.Enabled,
            };

            return Ok(characteristicVm);
        }


        /// <summary>
        /// Url: /api/characteristics/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpGet("/GetByProcessId/{ProcessId:int}")]
        public async Task<IActionResult> GetByProcessId(int ProcessId)
        {
            var characteristics = await _context.Characteristics.Where(r => r.ProcessId == ProcessId && r.Enabled == true).ToListAsync();

            if (characteristics == null || characteristics?.Count() == 0)
            {
                return NotFound("No characteristics found for the specified process.");
            }

            var characteristicVms = characteristics?.Select(characteristic => new CharacteristicVm
            {
                Id = characteristic.Id,
                Name = characteristic.Name,
                MeaTypeId = characteristic.MeaTypeId,
                ProcessId = characteristic.ProcessId,
                DataType = characteristic.DataType,
                Unit = characteristic.Unit,
                DefectRateLimit = characteristic.DefectRateLimit,
                EmailEventModel = characteristic.EmailEventModel,
                Decimals = characteristic.Decimals,
                Enabled = characteristic.Enabled,
            });

            return Ok(characteristicVms);
        }


        /// <summary>
        /// Url: /api/characteristics/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateCharacteristic(int Id, CharacteristicVm characterisitcVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id < 0 || Id != characterisitcVm.Id)
            {
                return BadRequest("Invalid characteristic ID.");
            }

            var characteristic = _context.Characteristics.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if (characteristic == null)
            {
                return NotFound("Characteristic not found.");
            }

            if (string.IsNullOrWhiteSpace(characterisitcVm.Name))
            {
                return BadRequest("Characteristic name cannot be empty.");
            }

            var measType = _context.MeasureTypes.FirstOrDefault(m => m.Id == characterisitcVm.MeaTypeId);

            if (measType == null)
            {
                return BadRequest("Invalid Measure Type.");
            }

            if (!(new[] { 0, 1 }).Contains(characterisitcVm.DataType ??-1))
            {
                return BadRequest("Invalid Characteristic Type.");
            }

            if (characterisitcVm.Decimals == null || characterisitcVm.Decimals < 0)
            {
                return BadRequest("Decimals cannot be negative.");
            }

            var charExists = _context.Characteristics.FirstOrDefault(x =>
                x.Name == characterisitcVm.Name &&
                x.ProcessId == characterisitcVm.ProcessId &&
                x.Enabled == true &&
                x.Id != characterisitcVm.Id);

            if (charExists != null)
            {
                return BadRequest("Characteristic with the same name already exists in this process.");
            }

            characteristic.Name = characterisitcVm.Name;
            characteristic.MeaTypeId = characterisitcVm.MeaTypeId;
            characteristic.DataType = characterisitcVm.DataType;
            characteristic.Unit = characterisitcVm.Unit;
            characteristic.DefectRateLimit = characterisitcVm.DefectRateLimit;
            characteristic.EmailEventModel = characterisitcVm.EmailEventModel;
            characteristic.Decimals = characterisitcVm.Decimals;

            _context.Characteristics.Update(characteristic);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(new CharacteristicVm
                {
                    Id = characteristic.Id,
                    Name = characteristic.Name,
                    MeaTypeId = characteristic.MeaTypeId,
                    ProcessId = characteristic.ProcessId,
                    DataType = characteristic.DataType,
                    Unit = characteristic.Unit,
                    DefectRateLimit = characteristic.DefectRateLimit,
                    EmailEventModel = characteristic.EmailEventModel,
                    Decimals = characteristic.Decimals,
                    Enabled = characteristic.Enabled,
                });
            }
            else
            {
                return BadRequest("Failed to update characteristic.");
            }    
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteCharacteristic(int Id)
        {
            var characteristic = _context.Characteristics.FirstOrDefault(r => r.Id == Id && r.Enabled == true);
            
            if (characteristic == null)
            {
                return NotFound("Characteristic not found.");
            }

            characteristic.Enabled = false;
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(new CharacteristicVm { 
                    Id = characteristic.Id,
                    Name = characteristic.Name,
                    MeaTypeId = characteristic.MeaTypeId,
                    ProcessId = characteristic.ProcessId,
                    DataType = characteristic.DataType,
                    Unit = characteristic.Unit,
                    DefectRateLimit = characteristic.DefectRateLimit,
                    EmailEventModel = characteristic.EmailEventModel,
                    Decimals = characteristic.Decimals,
                    Enabled = characteristic.Enabled,
                });
            }
            else
            {
                return BadRequest("Failed to delete characteristic.");
            }
        }
    }
}
