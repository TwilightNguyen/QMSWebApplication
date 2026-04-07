using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.Command;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class CommandsController(ApplicationDbContext context) : ControllerBase
    {
        public readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Url: /api/commands/
        /// </summary>
        /// 
        [HttpPost]
        public async Task<IActionResult> CreateCommand([FromBody] CommandCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (String.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Command name is required.");
            }

            int existingCommandCount = _context.Commands.Count(f => f.Name == request.Name);
            if (existingCommandCount != 0)
            {
                return BadRequest("Command with the same name already exists.");
            }


            int maxOrderNumber = _context.Commands.Any() ? _context.Commands.Max(f => f.DisplayOrder) : 0;

            var newCommand = new Commands
            {
                Name = request.Name,
                Notes = request.Notes,
                DisplayOrder = maxOrderNumber + 1,
                UploadedDateTime = DateTimeOffset.Now,
                Enabled = true
            };

            _context.Commands.Add(newCommand);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return CreatedAtAction(
                    nameof(GetById),
                    new { newCommand.Id },
                    new CommandVm
                    {
                        Id = newCommand.Id,
                        Name = newCommand.Name,
                        Notes = newCommand.Notes,
                        UploadedDateTime = newCommand.UploadedDateTime,
                        ModifiedDateTime = newCommand.ModifiedDateTime,
                        Enabled = newCommand.Enabled,
                        DisplayOrder = newCommand.DisplayOrder
                    }
                );
            }
            else
            {
                return BadRequest("Failed to create command.");
            }
        }


        /// <summary>
        /// Url: /api/commands/
        /// </summary>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var commands = await _context.Commands.OrderBy(x => x.DisplayOrder).ToListAsync();

            if (commands == null || commands.Count == 0)
            {
                return NotFound("No command found.");
            }
            var commandVms = commands.Select(command => new CommandVm
            {
                Id = command.Id,
                Name = command.Name,
                Notes = command.Notes,
                DisplayOrder = command.DisplayOrder, 
                UploadedDateTime = command.UploadedDateTime,
                ModifiedDateTime = command.ModifiedDateTime,
                Enabled = command.Enabled,
            });
            return Ok(commandVms);

        }

        /// <summary>
        /// Url: /api/commands/{id}
        /// </summary>
        /// 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var command = _context.Commands.FirstOrDefault(f => f.Id == id);
            if (command == null)
            {
                return NotFound($"Command with Id {id} not found.");
            }
            var commandVm = new CommandVm
            {
                Id = command.Id,
                Name = command.Name,
                Notes = command.Notes,
                DisplayOrder = command.DisplayOrder,
                UploadedDateTime = command.UploadedDateTime,
                ModifiedDateTime = command.ModifiedDateTime,
                Enabled = command.Enabled
            };
            return Ok(commandVm);
        }

        /// <summary>
        /// Url: /api/commands/pagging/?filter=serchString&pageIndex=1&pageSize=10
        /// </summary>
        /// <returns></returns>
        [HttpGet("Pagging")]
        public async Task<IActionResult> GetPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _context.Commands.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(r => r.Name!.Contains(filter));
            }

            List<CommandVm> items = [.. query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(x => x.DisplayOrder)
                .Select(command => new CommandVm
                {
                    Id = command.Id,
                    Name = command.Name,
                    DisplayOrder = command.DisplayOrder,
                    Notes = command.Notes,
                    UploadedDateTime = command.UploadedDateTime,
                    ModifiedDateTime = command.ModifiedDateTime,
                    Enabled = command.Enabled,

                })];

            var paginaton = new Pagination<CommandVm>()
            {
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(paginaton);
        }


        /// <summary>
        /// Url: /api/commands/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateCommand(int Id, CommandCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (String.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Command name is required.");
            }

            var command = _context.Commands.FirstOrDefault(r => r.Id == Id);

            if (command == null)
            {
                return NotFound($"Commnad with Id {Id} not found.");
            }

            var funcExists = _context.Commands.FirstOrDefault(x =>
                x.Name == request.Name && x.Id != Id);

            if (funcExists != null)
            {
                return BadRequest("Command with the same name already exists.");
            }


            command.Name = request.Name;
            command.Notes = request.Notes;
            command.ModifiedDateTime = DateTimeOffset.Now;

            _context.Commands.Update(command);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(new CommandVm
                {
                    Id = command.Id,
                    Name = command.Name,
                    Notes = command.Notes,
                    UploadedDateTime = command.UploadedDateTime,
                    ModifiedDateTime = command.ModifiedDateTime,
                    DisplayOrder = command.DisplayOrder,
                    Enabled = command.Enabled,
                });
            }
            else
            {
                return BadRequest("Failed to update command.");
            }
        }

        /// <summary>
        /// Url: /api/commands/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteCommand(int Id)
        {
            var command = _context.Commands.FirstOrDefault(r => r.Id == Id);

            if (command == null)
            {
                return NotFound($"Command with Id {Id} not found.");
            }

            _context.Commands.Remove(command);

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(new CommandVm
                {
                    Id = command.Id,
                    Name = command.Name,
                    Notes = command.Notes,
                    UploadedDateTime = command.UploadedDateTime,
                    ModifiedDateTime = command.ModifiedDateTime,
                    DisplayOrder = command.DisplayOrder,
                    Enabled = command.Enabled,
                });
            }
            else
            {
                return BadRequest("Failed to delete command.");
            }
        }
    }
}
