using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.Role;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class RolesController(RoleManager<Roles> roleManager) : ControllerBase
    {
        private readonly RoleManager<Roles> _roleManager = roleManager;

        /// <summary>
        ///  URL: /api/roles
        /// </summary>
        /// <param name="roleVm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody]RoleCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Role name cannot be empty.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(request.Name);
            if (roleExists)
            {
                return Conflict("Role already exists.");
            }


            var maxIntRoleId = _roleManager.Roles
                .Max(r => (int?)r.IntRoleID) ?? 0;

            // Use IdentityRole instead of Role for RoleManager
            var role = new Roles
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                NormalizedName = request.Name.ToUpperInvariant(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),

                StrRoleName = request.Name,
                StrDescription = request.Description,
                IntRoleUser = request.RoleUser,
                IntLevel = request.Level,
                IntRoleID = maxIntRoleId + 1
            };

            var result = await _roleManager.CreateAsync(role);
            
            return result.Succeeded ? Ok("Role created successfully.") : BadRequest("Role creation failed.");
        }

        /// <summary>
        /// Url: /api/roles/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            if (roles == null) return NotFound("No roles found.");

            var roleVms = roles.Select(role => new RoleVm
            {
                Name = role.StrRoleName ?? string.Empty,
                Description = role.StrDescription,
                RoleUser = role.IntRoleUser ?? -1,
                Level = role.IntLevel,
                RoleId = role.IntRoleID
            }).ToList();

            return Ok(roleVms);
        }

        /// <summary>
        /// Url: /api/roles/?filter=serchString&pageIndex=1&pageSize=10
        /// </summary>
        /// <returns></returns>
        [HttpGet("Pagging")]
        public async Task<IActionResult> GetRolesPaging(string? filter,int pageIndex, int pageSize)
        {
            var query = _roleManager.Roles.AsQueryable();

            if(!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(r => r.StrRoleName!.Contains(filter) || (r.StrDescription != null && r.StrDescription.Contains(filter)));
            }

            List<RoleVm> items = [.. query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(role => new RoleVm
                {
                    Name = role.StrRoleName ?? string.Empty,
                    Description = role.StrDescription,
                    RoleUser = role.IntRoleUser ?? -1,
                    Level = role.IntLevel,
                    RoleId = role.IntRoleID
                })];

            var paginaton = new Pagination<RoleVm>()
            {
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(paginaton);
        }

        /// <summary>
        /// Url: /api/roles/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var role = _roleManager.Roles.FirstOrDefault(r => r.IntRoleID == Id);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var roleVm = new RoleVm
            {
                RoleId = role.IntRoleID,
                Name = role.StrRoleName ?? string.Empty,
                Description = role.StrDescription,
                RoleUser = role.IntRoleUser??-1,
                Level = role.IntLevel
            };
            return Ok(roleVm);
        }


        /// <summary>
        /// Url: /api/roles/{Id}
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// 
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateRole(int Id, RoleVm roleVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id < 0 || Id != roleVm.RoleId)
            {
                return BadRequest("Role ID mismatch.");
            }

            var role = _roleManager.Roles.FirstOrDefault(r => r.IntRoleID == Id);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            role.Name = roleVm.Name;
            role.NormalizedName = roleVm.Name.ToUpperInvariant();

            role.StrRoleName = roleVm.Name;
            role.StrDescription = roleVm.Description;
            role.IntRoleUser = roleVm.RoleUser;
            role.IntLevel = roleVm.Level;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteRole(int Id)
        {
            var role = _roleManager.Roles.FirstOrDefault(r => r.IntRoleID == Id);
            if (role == null)
            {
                return NotFound("Role not found.");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                var roleVm = new RoleVm
                {
                    Name = role.StrRoleName ?? string.Empty,
                    Description = role.StrDescription,
                    RoleUser = role.IntRoleUser ?? -1,
                    Level = role.IntLevel,
                    RoleId = role.IntRoleID
                };

                return Ok(roleVm);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
