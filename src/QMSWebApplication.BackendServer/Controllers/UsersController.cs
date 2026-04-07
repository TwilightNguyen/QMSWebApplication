using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.User;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class UsersController(UserManager<User> userManager, RoleManager<Roles> roleManager) : ControllerBase
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly RoleManager<Roles> _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));

        /// <summary>
        ///  URL: /api/users
        /// </summary>
        [HttpPost] 
        public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (string.IsNullOrWhiteSpace(request.EmailAddress))
            {
                return BadRequest("Email cannot be empty.");
            }

            //var userExists = _userManager.FindByEmailAsync(request.EmailAddress);
            var userExists = _userManager.Users.FirstOrDefault( x => x.Email == request.EmailAddress);

            if (userExists != null)
            {
                return Conflict("Email already exists.");
            }

            var maxIntuserId = _userManager.Users
                .Max(r => (int?)r.IntUserID) ?? 0;

            // Use IdentityRole instead of Role for RoleManager
            var user = new User
            {
                StrFullName = request.FullName,
                StrEmailAddress = request.EmailAddress,
                StrRoleID = request.RoleId,
                IntEnable = request.Enable,
                StrPassword = Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(request.Password))),
                IntUserID = maxIntuserId + 1,
                StrDepartment = request.Department,
                StrStaffID = request.StaffId,
                StrSelectedAreaID = request.SelectedAreaId,
                DtLastActivityTime = DateTime.UtcNow,

                Id = Guid.NewGuid().ToString(),
                UserName = request.FullName,
                Email = request.EmailAddress,
                NormalizedUserName = request.FullName.ToUpper(),
                NormalizedEmail = request.EmailAddress.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            return result.Succeeded ? Ok("User created successfully.") : BadRequest("User creation failed.");
        }

        /// <summary>
        /// Url: /api/users/
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            if (users == null) return NotFound("No users found.");

            var userVms = users.Select(user => new UserVm
            {
                EmailAddress = user.Email!,
                FullName = user.StrFullName??"",
                Department = user.StrDepartment!,
                StaffID = user.StrStaffID!,
                UserID = user.IntUserID,
                RoleID = user.StrRoleID,
                LastActivityTime = user.DtLastActivityTime,
                Enabled = user.IntEnable,
                SelectedAreaID = user.StrSelectedAreaID
            }).ToList();

            return Ok(userVms);
        }

        /// <summary>
        /// Url: /api/users/?filter=serchString&pageIndex=1&pageSize=10
        /// </summary>
        [HttpGet("Pagging")]
        public async Task<IActionResult> GetUsersPaging(string? filter, int pageIndex, int pageSize)
        {
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(r => r.StrEmailAddress!.Contains(filter) || (r.StrFullName != null && r.StrFullName.Contains(filter)));
            }

            List<UserVm> items = [.. query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(user => new UserVm
                {
                    EmailAddress = user.Email!,
                    Department = user.StrDepartment!,
                    FullName = user.StrFullName ?? string.Empty,
                    StaffID = user.StrStaffID!,
                    UserID = user.IntUserID,
                    RoleID = user.StrRoleID,
                    LastActivityTime = user.DtLastActivityTime,
                    Enabled = user.IntEnable,
                    SelectedAreaID = user.StrSelectedAreaID
                })];

            var paginaton = new Pagination<UserVm>()
            {
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(paginaton);
        }

        /// <summary>
        /// Url: /api/users/{Id}
        /// </summary>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var user = _userManager.Users.FirstOrDefault(r => r.IntUserID == Id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userVm = new UserVm
            {
                EmailAddress = user.Email!,
                FullName = user.StrFullName ?? string.Empty,
                Department = user.StrDepartment!,
                StaffID = user.StrStaffID!,
                UserID = user.IntUserID,
                RoleID = user.StrRoleID,
                LastActivityTime = user.DtLastActivityTime,
                Enabled = user.IntEnable,
                SelectedAreaID = user.StrSelectedAreaID
            };
            return Ok(userVm);
        }

        /// <summary>
        /// Url: /api/users/{Id}
        /// </summary>
        /// 
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateUser(int Id, UserVm userVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id < 0 || Id != userVm.UserID)
            {
                return BadRequest("User ID mismatch.");
            }

            var user = _userManager.Users.FirstOrDefault(r => r.IntUserID == Id);

            if (user == null)
            {
                return NotFound("User not found.");
            }
            
            user.StrRoleID = userVm.RoleID;
            user.StrFullName = userVm.FullName;
            user.StrEmailAddress = userVm.EmailAddress;
            user.IntEnable = userVm.Enabled;
            user.StrDepartment = userVm.Department;
            user.StrStaffID = userVm.StaffID;
            user.StrSelectedAreaID = userVm.SelectedAreaID;

            user.UserName = userVm.FullName;
            user.NormalizedUserName = userVm.FullName.ToUpper();
            user.Email = userVm.EmailAddress;
            user.NormalizedEmail = userVm.EmailAddress.ToUpper();

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            } 
        }

        /// <summary>
        /// Url: /api/users/ChangePasswrod
        /// </summary>
        /// 
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
             
            var user = _userManager.Users.FirstOrDefault(r => r.IntUserID == request.UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// Url: /api/users/ChangePasswrod
        /// </summary>
        /// 
        [HttpPut("AssignRole")]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {

            if (userId < 0)
            {
                return BadRequest("User ID mismatch.");
            }

            if (roleId < 0)
            {
                return BadRequest("Role ID mismatch.");
            }

            var user = _userManager.Users.FirstOrDefault(r => r.IntUserID == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var role = _roleManager.Roles.FirstOrDefault(r => r.IntLevel == roleId);
            
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            int[] roleIds = user.StrRoleID != null
                ? [.. user.StrRoleID.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)]
                : [];

            if (roleIds.Contains(role.IntRoleID))
            {
                return BadRequest("User already has this role.");
            }

            if(role.IntRoleUser == 0)
            {
                var maxLevel = _roleManager.Roles
                    .Where(r => r.IntRoleUser == 0)
                    .Max(r => (int?)r.IntLevel) ?? 0;

                if(role.IntLevel != maxLevel)
                {
                    var conflictingRoles = roleIds
                        .Select(rid => _roleManager.Roles.FirstOrDefault(r => r.IntRoleID == rid))
                        .Where(r => r != null && r.IntLevel < maxLevel && r.IntRoleUser == 0);  
                    
                    if (conflictingRoles.Any())
                        return BadRequest("Can't assign multiple roles to a single user.");
                }
            }

            _ = roleIds.Append(role.IntRoleID);

            user.StrRoleID = string.Join(",", roleIds);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        /// <summary>
        /// Url: /api/users/ChangePasswrod
        /// </summary>
        /// 
        [HttpPut("UnassignRole")]
        public async Task<IActionResult> UnassignRole(int userId, int roleId)
        {
            if (userId < 0)
            {
                return BadRequest("User ID mismatch.");
            }

            if (roleId < 0)
            {
                return BadRequest("Role ID mismatch.");
            }

            var user = _userManager.Users.FirstOrDefault(r => r.IntUserID == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }


            var role = _roleManager.Roles.FirstOrDefault(r => r.IntLevel == roleId);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            int[] roleIds = user.StrRoleID != null
                ? [.. user.StrRoleID.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)]
                : [];

            if (!roleIds.Contains(role.IntRoleID))
            {
                return BadRequest("User does not have this role.");
            }

            if (role.IntRoleUser == 0)
            {
                var maxLevel = _roleManager.Roles
                    .Where(r => r.IntRoleUser == 0)
                    .Max(r => (int?)r.IntLevel) ?? 0;

                 
                var GuestRole = _roleManager.Roles.FirstOrDefault(r => r != null && r.IntLevel == maxLevel && r.IntRoleUser == 0);
                _ = roleIds.Append(GuestRole!.IntRoleID);
            }

            _ = roleIds.Append(role.IntRoleID);

            user.StrRoleID = string.Join(",", roleIds);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }


        /// <summary>
        /// Url: /api/users/{id}
        /// </summary>
        /// 
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var user = _userManager.Users.FirstOrDefault(r => r.IntUserID == Id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var userVm = new UserVm
                {
                    EmailAddress = user.Email!,
                    FullName = user.StrFullName ?? string.Empty,
                    Department = user.StrDepartment!,
                    StaffID = user.StrStaffID!,
                    UserID = user.IntUserID,
                    RoleID = user.StrRoleID,
                    LastActivityTime = user.DtLastActivityTime,
                    Enabled = user.IntEnable,
                    SelectedAreaID = user.StrSelectedAreaID
                };

                return Ok(userVm);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
