using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.User;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class UsersControllerTest
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly List<User> _usersSource;

        private readonly Mock<RoleManager<Roles>> _mockRoleManager;
        private readonly List<Roles> _rolesSource;

        public UsersControllerTest()
        {
            // Arrange
            // Mock UserManager
            var uStore = new Mock<IUserStore<User>>();
            var uOptions = Options.Create(new IdentityOptions());
            var uPasswordHasher = new Mock<IPasswordHasher<User>>();
            var uValidators = new List<IUserValidator<User>>(); // can stay empty
            var uKeyNormalizer = new Mock<ILookupNormalizer>();
            var uErrorDescriber = new IdentityErrorDescriber();
            var uLogger = new Mock<ILogger<UserManager<User>>>();
            var uPasswordValidators = new List<IPasswordValidator<User>>       // 5. IEnumerable<IPasswordValidator<User>>
            {
                new PasswordValidator<User>()
            };

            var uServices = new Mock<IServiceProvider>();

            _mockUserManager = new Mock<UserManager<User>>(
                uStore.Object,
                uOptions,
                uPasswordHasher.Object,
                uValidators,
                uPasswordValidators,
                uKeyNormalizer.Object,
                uErrorDescriber,
                uServices.Object,
                uLogger.Object
            )
            {
                CallBase = true
            };

            // _mockUserManager = new Mock<UserManager<User>>(
            //    uStore.Object,
            //    uValidators,
            //    uKeyNormalizer.Object,s
            //    uErrorDescriber,
            //    uLogger.Object
            //);


            _usersSource =
            [
                new User() { 
                    Id = Guid.NewGuid().ToString(), 
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    Email = "admin@gmal.com",
                    StrFullName = "System Administrator",
                    IntUserID = 1,
                    IntEnable = 1,
                    StrRoleID = "1",
                    StrPassword = "Admin@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "admin@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "A001",
                    DtLastActivityTime = DateTime.UtcNow
                },
                new User() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "super",
                    NormalizedUserName = "SUPER",
                    NormalizedEmail = "SUPER@GMAIL.COM",
                    Email = "super@gmal.com",
                    StrFullName = "System Supervisor",
                    IntUserID = 2,
                    IntEnable = 1,
                    StrRoleID = "2",
                    StrPassword = "Super@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "super@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "S001",
                    DtLastActivityTime = DateTime.UtcNow
                },
                new User() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "manager",
                    NormalizedUserName = "MANAGER",
                    NormalizedEmail = "MANAGER@GMAIL.COM",
                    Email = "manager@gmal.com",
                    StrFullName = "System Manager",
                    IntUserID = 3,
                    IntEnable = 1,
                    StrRoleID = "3",
                    StrPassword = "Manager@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "manager@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "M001",
                    DtLastActivityTime = DateTime.UtcNow
                },

                new User() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "technician",
                    NormalizedUserName = "TECHNICIAN",
                    NormalizedEmail = "TECHNICIAN@GMAIL.COM",
                    Email = "technician@gmal.com",
                    StrFullName = "System Technician",
                    IntUserID = 4,
                    IntEnable = 1,
                    StrRoleID = "4",
                    StrPassword = "Technician@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "technician@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "T001",
                    DtLastActivityTime = DateTime.UtcNow
                },
                new User() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "operator",
                    NormalizedUserName = "OPERATOR",
                    NormalizedEmail = "OPERATOR@GMAIL.COM",
                    Email = "operator@gmal.com",
                    StrFullName = "System Operator",
                    IntUserID = 5,
                    IntEnable = 1,
                    StrRoleID = "5",
                    StrPassword = "Operator@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "operator@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "O001",
                    DtLastActivityTime = DateTime.UtcNow
                },
                new User() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "guest",
                    NormalizedUserName = "GUEST",
                    NormalizedEmail = "GUEST@GMAIL.COM",
                    Email = "guest@gmal.com",
                    StrFullName = "System Guest",
                    IntUserID = 6,
                    IntEnable = 1,
                    StrRoleID = "6",
                    StrPassword = "Guest@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "guest@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "G001",
                    DtLastActivityTime = DateTime.UtcNow
                },
            ];

            // Mock RoleManager
            var rStore = new Mock<IRoleStore<Roles>>();
            var rRoleValidators = new List<IRoleValidator<Roles>>(); // can stay empty
            var rKeyNormalizer = new Mock<ILookupNormalizer>();
            var rErrorDescriber = new IdentityErrorDescriber();
            var rLogger = new Mock<ILogger<RoleManager<Roles>>>();

            _mockRoleManager = new Mock<RoleManager<Roles>>(
                rStore.Object,
                rRoleValidators,
                rKeyNormalizer.Object,
                rErrorDescriber,
                rLogger.Object
            );

            _rolesSource =
            [
                new() { Id = Guid.NewGuid().ToString(), Name = "Admin", IntRoleID = 1, StrRoleName = "Admin", StrDescription = "Administrator Role", IntLevel = 1, IntRoleUser = 0 },
                new() { Id = Guid.NewGuid().ToString(), Name = "User",  IntRoleID = 2, StrRoleName = "User", StrDescription = "User Role", IntLevel = 2, IntRoleUser = 0 },
                new() { Id = Guid.NewGuid().ToString(), Name = "Manager",  IntRoleID = 3, StrRoleName = "Manager", StrDescription = "Manager Role", IntLevel = 3, IntRoleUser = 0 },
                new() { Id = Guid.NewGuid().ToString(), Name = "Assistant",  IntRoleID = 4, StrRoleName = "Assistant", StrDescription = "Assistant Role", IntLevel = 4, IntRoleUser = 0 },
                new() { Id = Guid.NewGuid().ToString(), Name = "Supervisor",  IntRoleID = 5, StrRoleName = "Supervisor", StrDescription = "Supervisor Role", IntLevel = 5, IntRoleUser = 0 },
            ];
        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            // Assert
            Assert.NotNull(controller);

        }

        [Fact]
        public async Task CreateUser_ValidInput_Success()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());

            _mockUserManager.Setup(rm => rm.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.CreateUser(new UserCreateRequest
            {
                EmailAddress = "user01@gmail.com",
                FullName = "User 01",
                Department = "IT",
                StaffId = "U001",
                Password = "Admin@123",
                RoleId = "6",
                SelectedAreaId = "1",
                Enable = 1
            });

            // Assert
            Assert.NotNull(result);

            var successRequest = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User created successfully.", successRequest.Value);

            // Verify UserManager was NEVER called
            _mockUserManager.Verify(rm => rm.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateUser_ValidInput_Failed()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());

            _mockUserManager.Setup(rm => rm.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.CreateUser(new UserCreateRequest
            {
                EmailAddress = string.Empty,
                FullName = "User 1",
                Department = "IT",
                StaffId = "U001",
                Password = "Admin@123",
                RoleId = "6",
                SelectedAreaId = "1",
                Enable = 1
            });

            // Assert
            Assert.NotNull(result);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Email cannot be empty.", badRequest.Value);

            // Verify UserManager was NEVER called
            _mockUserManager.Verify(rm => rm.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetAllUser_HasData_ReturnSuccess()
        {

            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.GetAllUsers();
            var OkResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);

            Assert.True(OkResult?.Value is not null);
            var userList = OkResult.Value as IEnumerable<UserVm>;
            Assert.NotNull(userList);
            Assert.True(userList?.Count() > 0);
        }

        [Fact]
        public async Task GetAllUser_ThrowException_Failed()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Throws<Exception>();

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await controller.GetAllUsers());
        }

        [Fact]
        public async Task GetUsersPaging_NoFilter_ReturnSuccess()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.GetUsersPaging(null, 1, 2);
            var OkResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var userList = OkResult?.Value as Pagination<UserVm>;
            Assert.NotNull(userList);
            Assert.Equal(6, userList.TotalRecords);
            Assert.Equal(2, userList.Items.Count);
        }

        [Fact]
        public async Task GetUsersPaging_HasFilter_ReturnSuccess()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());

            // Act
            var controller = new UsersController( _mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.GetUsersPaging("Admin", 1, 2);
            var OkResult = result as OkObjectResult;

            // Assert 
            Assert.NotNull(result);
            var userList = OkResult?.Value as Pagination<UserVm>;
            Assert.NotNull(userList);
            Assert.Equal(1, userList.TotalRecords);
            Assert.Single(userList.Items);
        }

        [Fact]
        public async Task GetUsersPaging_ThrowException_Failed()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Throws<Exception>();

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await controller.GetUsersPaging("", -1, -1));
        }

        [Fact]
        public async Task GetUserById_HasData_ReturnSuccess()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.GetById(1);
            var OkResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var userList = OkResult?.Value as UserVm;
            Assert.NotNull(userList);
            Assert.Equal(1, userList.UserID);
        }

        [Fact]
        public async Task GetUserById_ThrowException_Failed()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Throws<Exception>();

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await controller.GetById(-1));
        }

        [Fact]
        public async Task UpdateUser_ValidInput_Success()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());

            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.UpdateUser(1, new UserVm
            {
                FullName = "Admin 1",
                Department = "OT",
                StaffID = "A002",
                EmailAddress = "admin1@gmail.com",
                Enabled = 1,
                UserID = 1,
            });

            // Assert
            Assert.NotNull(result);

            var successRequest = Assert.IsType<NoContentResult>(result);

            // Verify UserManager was called

            _mockUserManager.VerifyGet(rm => rm.Users, Times.Once);
            //_mockRoleManager.Verify(rm => rm.Roles.FirstOrDefault(It.IsAny<Role>()), Times.Once);
            _mockUserManager.Verify(rm => rm.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUser_ValidInput_Failed_UserIDMismatch()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.UpdateUser(10, new UserVm
            {
                FullName = "Admin 1",
                Department = "OT",
                StaffID = "A002",
                EmailAddress = "admin1@gmail.com",
                Enabled = 1,
                UserID = 1,
            });

            // Assert
            Assert.NotNull(result);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User ID mismatch.", badRequest.Value);

            // Verify UserManager was NEVER called
            _mockUserManager.VerifyGet(rm => rm.Users, Times.Never);
            _mockUserManager.Verify(rm => rm.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UpdateUser_ValidInput_Failed_RoleNotFound()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.UpdateUser(10, new UserVm
            {
                FullName = "Admin 1",
                Department = "OT",
                StaffID = "A002",
                EmailAddress = "admin1@gmail.com",
                Enabled = 1,
                UserID = 10,
            });

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundRequest.Value);

            // Verify UserManager was called
            _mockUserManager.VerifyGet(rm => rm.Users, Times.Once);
            // Verify UserManager was NEVER called
            _mockUserManager.Verify(rm => rm.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_ValidInput_Success()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());

            _mockUserManager.Setup(rm => rm.DeleteAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.DeleteUser(1);

            // Assert
            Assert.NotNull(result);

            var successRequest = Assert.IsType<OkObjectResult>(result);

            // Verify UserManager was called

            _mockUserManager.VerifyGet(rm => rm.Users, Times.Once);
            //_mockRoleManager.Verify(rm => rm.Roles.FirstOrDefault(It.IsAny<Role>()), Times.Once);
            _mockUserManager.Verify(rm => rm.DeleteAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ValidInput_Failed_UserNotFound()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.DeleteAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.DeleteUser(10);

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundRequest.Value);

            // Verify UserManager was called
            _mockUserManager.VerifyGet(rm => rm.Users, Times.Once);
            // Verify UserManager was NEVER called
            _mockUserManager.Verify(rm => rm.DeleteAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task ChangePassword_ValidInput_Success()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.ChangePassword(new UserChangePasswordRequest
            {
                CurrentPassword = "Admin@123",
                NewPassword = "Admin@1234",
                UserId = 1
            });

            // Assert
            Assert.NotNull(result);

            var successRequest = Assert.IsType<NoContentResult>(result);

            // Verify UserManager was called
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            _mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ChangePassword_ValidInput_Failed_UserNotFound()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.ChangePassword(new UserChangePasswordRequest
            {
                CurrentPassword = "Admin@123",
                NewPassword = "Admin@1234",
                UserId = 10
            });

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundRequest.Value);

            // Verify UserManager was called
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            _mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ChangePassword_ValidInput_Failed_CurrentPasswordIncorrect()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.ChangePassword(new UserChangePasswordRequest
            {
                CurrentPassword = "Wrong@123",
                NewPassword = "Admin@1234",
                UserId = 1
            });

            // Assert
            Assert.NotNull(result);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            // Verify UserManager was called
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            _mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AssignRole_ValidInput_Success()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.AssignRole(6, 1);

            // Assert
            Assert.NotNull(result);

            var successRequest = Assert.IsType<NoContentResult>(result);

            // Verify UserManager was called
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            _mockRoleManager.VerifyGet(um => um.Roles, Times.AtLeast(2));
            _mockUserManager.Verify(um => um.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task AssignRole_ValidInput_Failed_UserIdMismatch()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.AssignRole(-1, 1);

            // Assert
            Assert.NotNull(result);

            var badFoundRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User ID mismatch.", badFoundRequest.Value);

            // Verify UserManager was called
            //_mockUserManager.VerifyGet(um => um.Users, Times.Once);
            //_mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AssignRole_ValidInput_Failed_RoleIdMismatch()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.AssignRole(1, -1);

            // Assert
            Assert.NotNull(result);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Role ID mismatch.", badRequest.Value);

            // Verify UserManager was called
            //_mockUserManager.VerifyGet(um => um.Users, Times.Once);
            //_mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AssignRole_ValidInput_Failed_UserNotNound()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.AssignRole(10, 1);

            // Assert
            Assert.NotNull(result);

            var notFoundequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundequest.Value);

            // Verify UserManager was called
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            //_mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AssignRole_ValidInput_Failed_RoleNotNound()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.AssignRole(6, 10);

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Role not found.", notFoundRequest.Value);

            // Verify UserManager was called
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            _mockRoleManager.VerifyGet(um => um.Roles, Times.Once);
            //_mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AssignRole_ValidInput_Failed_RoleAlreadyAssigned()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.AssignRole(1, 1);

            // Assert
            Assert.NotNull(result);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User already has this role.", badRequest.Value);

            // Verify UserManager was called
            _mockRoleManager.VerifyGet(um => um.Roles, Times.Once);
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            //_mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AssignRole_ValidInput_Failed_AssignMultipleSystemRole()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.AssignRole(1, 2);

            // Assert
            Assert.NotNull(result);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Can't assign multiple roles to a single user.", badRequest.Value);

            // Verify UserManager was called
            _mockRoleManager.VerifyGet(um => um.Roles, Times.AtLeast(2));
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            //_mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }


        [Fact]
        public async Task UnassignRole_ValidInput_Success()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.UnassignRole(1, 1);

            // Assert
            Assert.NotNull(result);

            var successRequest = Assert.IsType<NoContentResult>(result);

            // Verify UserManager was called
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            _mockRoleManager.VerifyGet(um => um.Roles, Times.AtLeast(2));
            _mockUserManager.Verify(um => um.UpdateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task UnassignRole_ValidInput_Failed_UserIdMismatch()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.UnassignRole(-1, 1);

            // Assert
            Assert.NotNull(result);

            var badFoundRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User ID mismatch.", badFoundRequest.Value);

            // Verify UserManager was called
            //_mockUserManager.VerifyGet(um => um.Users, Times.Once);
            //_mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UnassignRole_ValidInput_Failed_RoleIdMismatch()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.UnassignRole(1, -1);

            // Assert
            Assert.NotNull(result);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Role ID mismatch.", badRequest.Value);

            // Verify UserManager was called
            //_mockUserManager.VerifyGet(um => um.Users, Times.Once);
            //_mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UnassignRole_ValidInput_Failed_UserNotNound()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.UnassignRole(10, 1);

            // Assert
            Assert.NotNull(result);

            var notFoundequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User not found.", notFoundequest.Value);

            // Verify UserManager was called
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            //_mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UnassignRole_ValidInput_Failed_RoleNotNound()
        {
            _mockUserManager.Setup(rm => rm.Users)
                .Returns(_usersSource.BuildMock());
            _mockUserManager.Setup(rm => rm.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            _mockRoleManager.Setup(rm => rm.Roles)
               .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new UsersController(_mockUserManager.Object, _mockRoleManager.Object);

            var result = await controller.UnassignRole(6, 10);

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Role not found.", notFoundRequest.Value);

            // Verify UserManager was called
            _mockUserManager.VerifyGet(um => um.Users, Times.Once);
            _mockRoleManager.VerifyGet(um => um.Roles, Times.Once);
            //_mockUserManager.Verify(um => um.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

    }
}
