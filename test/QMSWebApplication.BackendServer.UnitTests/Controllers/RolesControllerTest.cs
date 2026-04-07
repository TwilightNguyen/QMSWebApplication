using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using MockQueryable;
using QMSWebApplication.ViewModels.System.Role;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class RolesControllerTest
    { 
        private readonly Mock<RoleManager<Roles>> _mockRoleManager;
        private readonly List<Roles> _rolesSource;

        public RolesControllerTest() {
            // Arrange
            var store = new Mock<IRoleStore<Roles>>();
            var roleValidators = new List<IRoleValidator<Roles>>(); // can stay empty
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errorDescriber = new IdentityErrorDescriber();
            var logger = new Mock<ILogger<RoleManager<Roles>>>();

            _mockRoleManager = new Mock<RoleManager<Roles>>(
                store.Object,
                roleValidators,
                keyNormalizer.Object,
                errorDescriber,
                logger.Object
            );

            _rolesSource =
            [
                new() { Id = Guid.NewGuid().ToString(), Name = "Admin", IntRoleID = 1, StrRoleName = "Admin", StrDescription = "Administrator Role", IntLevel = 1, IntRoleUser = 5 },
                new() { Id = Guid.NewGuid().ToString(), Name = "User",  IntRoleID = 2, StrRoleName = "User", StrDescription = "User Role", IntLevel = 2, IntRoleUser = 10 },
                new() { Id = Guid.NewGuid().ToString(), Name = "Manager",  IntRoleID = 3, StrRoleName = "Manager", StrDescription = "Manager Role", IntLevel = 3, IntRoleUser = 3 },
                new() { Id = Guid.NewGuid().ToString(), Name = "Assistant",  IntRoleID = 4, StrRoleName = "Assistant", StrDescription = "Assistant Role", IntLevel = 4, IntRoleUser = 7 },
                new() { Id = Guid.NewGuid().ToString(), Name = "Supervisor",  IntRoleID = 5, StrRoleName = "Supervisor", StrDescription = "Supervisor Role", IntLevel = 5, IntRoleUser = 2 },
            ];
        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            
            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            // Assert
            Assert.NotNull(controller);

        }

        [Fact]
        public async Task CreateRole_ValidInput_Success()
        {
            _mockRoleManager.Setup(rm => rm.CreateAsync(It.IsAny<Roles>()))
                .ReturnsAsync(IdentityResult.Success);
            
            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.CreateRole(new RoleCreateRequest
            {
                Name = "Test",
                Description = "Test Role",
                Level = 10,
                RoleUser = 10
            });

            // Assert
            Assert.NotNull(result);
            
            var successRequest = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Role created successfully.", successRequest.Value);

            // Verify RoleManager was NEVER called
            _mockRoleManager.Verify(rm => rm.RoleExistsAsync(It.IsAny<string>()), Times.Once);
            _mockRoleManager.Verify(rm => rm.CreateAsync(It.IsAny<Roles>()), Times.Once);
        }

        [Fact]
        public async Task CreateRole_ValidInput_Failed()
        {
            _mockRoleManager.Setup(rm => rm.CreateAsync(It.IsAny<Roles>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.CreateRole(new RoleCreateRequest
            {
                Name = "",
                Description = "",
                Level = -1,
                RoleUser = -1
            });

            // Assert
            Assert.NotNull(result);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Role name cannot be empty.", badRequest.Value);

            // Verify RoleManager was NEVER called
            _mockRoleManager.Verify(rm => rm.RoleExistsAsync(It.IsAny<string>()), Times.Never);
            _mockRoleManager.Verify(rm => rm.CreateAsync(It.IsAny<Roles>()), Times.Never);
        }

        [Fact]
        public async Task GetAllRole_HasData_ReturnSuccess()
        {

            _mockRoleManager.Setup(rm => rm.Roles)
                .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.GetAllRoles(); 
            var OkResult = result as OkObjectResult; 

            // Assert
            Assert.NotNull(result); 

            Assert.True(OkResult?.Value is not null);
            var roleList = OkResult.Value as IEnumerable<RoleVm>;
            Assert.NotNull(roleList);
            Assert.True(roleList?.Count() > 0);
        }

        [Fact]
        public async Task GetAllRole_ThrowException_Failed()
        {
            _mockRoleManager.Setup(rm => rm.Roles)
                .Throws<Exception>();

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await controller.GetAllRoles());
        }

        [Fact]
        public async Task GetRolesPaging_NoFilter_ReturnSuccess()
        { 
            _mockRoleManager.Setup(rm => rm.Roles)
                .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.GetRolesPaging(null, 1, 2);
            var OkResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var roleList = OkResult?.Value as Pagination<RoleVm>;
            Assert.NotNull(roleList);
            Assert.Equal(5, roleList.TotalRecords);
            Assert.Equal(2, roleList.Items.Count); 
        }


        [Fact]
        public async Task GetRolesPaging_HasFilter_ReturnSuccess()
        {
            _mockRoleManager.Setup(rm => rm.Roles)
                .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.GetRolesPaging("Admin", 1, 2);
            var OkResult = result as OkObjectResult;

            // Assert 
            Assert.NotNull(result);
            var roleList = OkResult?.Value as Pagination<RoleVm>;
            Assert.NotNull(roleList);
            Assert.Equal(1, roleList.TotalRecords);
            Assert.Single(roleList.Items);
        }

        [Fact]
        public async Task GetRolesPaging_ThrowException_Failed()
        {
            _mockRoleManager.Setup(rm => rm.Roles)
                .Throws<Exception>();

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await controller.GetRolesPaging("", -1, -1));
        }

        [Fact]
        public async Task GetRoleById_HasData_ReturnSuccess()
        { 
            _mockRoleManager.Setup(rm => rm.Roles)
                .Returns(_rolesSource.BuildMock());

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.GetById(1);
            var OkResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var roleList = OkResult?.Value as RoleVm;
            Assert.NotNull(roleList);
            Assert.Equal(1, roleList.RoleId);
        }

        [Fact]
        public async Task GetRoleById_ThrowException_Failed()
        {
            _mockRoleManager.Setup(rm => rm.Roles)
                .Throws<Exception>();

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await controller.GetById(-1));
        }

        [Fact]
        public async Task UpdateRole_ValidInput_Success()
        {
            _mockRoleManager.Setup(rm => rm.Roles)
                .Returns(_rolesSource.BuildMock());

            _mockRoleManager.Setup(rm => rm.UpdateAsync(It.IsAny<Roles>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.UpdateRole( 1,new RoleVm
            {
                Name = "Admin 1",
                Description = "Admin 1",
                Level = 2,
                RoleId = 1,
                RoleUser = 0
            });

            // Assert
            Assert.NotNull(result);

            var successRequest = Assert.IsType<NoContentResult>(result);

            // Verify RoleManager was called

            _mockRoleManager.VerifyGet(rm => rm.Roles, Times.Once);
            //_mockRoleManager.Verify(rm => rm.Roles.FirstOrDefault(It.IsAny<Role>()), Times.Once);
            _mockRoleManager.Verify(rm => rm.UpdateAsync(It.IsAny<Roles>()), Times.Once);
        }

        [Fact]
        public async Task UpdateRole_ValidInput_Failed_RoleIDMismatch()
        {
            _mockRoleManager.Setup(rm => rm.Roles)
                .Returns(_rolesSource.BuildMock());
            _mockRoleManager.Setup(rm => rm.UpdateAsync(It.IsAny<Roles>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.UpdateRole(10, new RoleVm
            {
                Name = "Admin 1",
                Description = "Admin 1",
                Level = 2,
                RoleId = 1,
                RoleUser = 0
            });

            // Assert
            Assert.NotNull(result);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Role ID mismatch.", badRequest.Value);

            // Verify RoleManager was NEVER called
            _mockRoleManager.VerifyGet(rm => rm.Roles, Times.Never);
            _mockRoleManager.Verify(rm => rm.UpdateAsync(It.IsAny<Roles>()), Times.Never);
        }

        [Fact]
        public async Task UpdateRole_ValidInput_Failed_RoleNotFound()
        {
            _mockRoleManager.Setup(rm => rm.Roles)
                .Returns(_rolesSource.BuildMock());
            _mockRoleManager.Setup(rm => rm.UpdateAsync(It.IsAny<Roles>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.UpdateRole(10, new RoleVm
            {
                Name = "Admin 1",
                Description = "Admin 1",
                Level = 2,
                RoleId = 10,
                RoleUser = 0
            });

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Role not found.", notFoundRequest.Value);

            // Verify RoleManager was called
            _mockRoleManager.VerifyGet(rm => rm.Roles, Times.Once);
            // Verify RoleManager was NEVER called
            _mockRoleManager.Verify(rm => rm.UpdateAsync(It.IsAny<Roles>()), Times.Never);
        }

        [Fact]
        public async Task DeleteRole_ValidInput_Success()
        {
            _mockRoleManager.Setup(rm => rm.Roles)
                .Returns(_rolesSource.BuildMock());

            _mockRoleManager.Setup(rm => rm.DeleteAsync(It.IsAny<Roles>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.DeleteRole(1);

            // Assert
            Assert.NotNull(result);

            var successRequest = Assert.IsType<OkObjectResult>(result);

            // Verify RoleManager was called

            _mockRoleManager.VerifyGet(rm => rm.Roles, Times.Once);
            //_mockRoleManager.Verify(rm => rm.Roles.FirstOrDefault(It.IsAny<Role>()), Times.Once);
            _mockRoleManager.Verify(rm => rm.DeleteAsync(It.IsAny<Roles>()), Times.Once);
        }


        [Fact]
        public async Task DeleteRole_ValidInput_Failed_RoleNotFound()
        {
            _mockRoleManager.Setup(rm => rm.Roles)
                .Returns(_rolesSource.BuildMock());
            _mockRoleManager.Setup(rm => rm.DeleteAsync(It.IsAny<Roles>()))
                .ReturnsAsync(IdentityResult.Failed([]));

            // Act
            var controller = new RolesController(_mockRoleManager.Object);

            var result = await controller.DeleteRole(10);

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Role not found.", notFoundRequest.Value);

            // Verify RoleManager was called
            _mockRoleManager.VerifyGet(rm => rm.Roles, Times.Once);
            // Verify RoleManager was NEVER called
            _mockRoleManager.Verify(rm => rm.DeleteAsync(It.IsAny<Roles>()), Times.Never);
        }
    }
}
