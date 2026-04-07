using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.InspectionPlan;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class InspectionPlansControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;

        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedProductionAreas(_context);
            InMemoryDbContext.SeedInspectionPlans(_context);
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            try { await _context.Database.EnsureDeletedAsync(); } catch { /* ignore */ }
            await _context.DisposeAsync();
        }


        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            // Act
            var controller = new InspectionPlansController(_context);
            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_HasData_Success()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var result = await controller.GetAll();
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var inspectionPlanList = okResult?.Value as IEnumerable<InspectionPlanVm>;
            Assert.Equal(3, inspectionPlanList?.Count());
            Assert.Contains(inspectionPlanList!, a => a.Name == "Inspection Plan A");
            Assert.Contains(inspectionPlanList!, a => a.Name == "Inspection Plan B");
            Assert.Contains(inspectionPlanList!, a => a.Name == "Inspection Plan C");
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            var controller = new InspectionPlansController(_context);

            // Act
            var result = await controller.GetById(1);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var inspectionPlan = okResult?.Value as InspectionPlanVm;
            Assert.NotNull(inspectionPlan);
            Assert.Equal("Inspection Plan A", inspectionPlan?.Name);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var result = await controller.GetById(999); // Assuming 999 does not exist
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Inspection Plan not found.", notFoundResult?.Value);
            Assert.Equal(404, notFoundResult?.StatusCode);
        }

        [Fact]
        public async Task GetByAreaId_HasData_Success()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var result = await controller.GetByAreaId(1); // Assuming Production Area Id 1 has data
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var inspectionPlanList = okResult?.Value as IEnumerable<InspectionPlanVm>;
            Assert.NotNull(inspectionPlanList);
            Assert.All(inspectionPlanList!, ip => Assert.Equal(1, ip.AreaId));
        }

        [Fact]
        public async Task GetByAreaId_NoData_ReturnsEmptyList()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var result = await controller.GetByAreaId(999); // Assuming Production Area Id 999 has no data
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("No inspection plan found for the specified production area.", notFoundResult?.Value);
            Assert.Equal(404, notFoundResult?.StatusCode);
        }

        [Fact]
        public async Task GetPaging_NoFilter_Success()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var result = await controller.GetPaging(null, 1, 2);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var pagedResult = okResult?.Value as Pagination<InspectionPlanVm>;
            Assert.NotNull(pagedResult);
            Assert.Equal(3, pagedResult!.TotalRecords);
            Assert.Equal(2, pagedResult.Items.Count);
        }

        [Fact]
        public async Task GetPaging_WithFilter_Success()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var result = await controller.GetPaging("Inspection Plan A", 1, 2);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var pagedResult = okResult?.Value as Pagination<InspectionPlanVm>;
            Assert.NotNull(pagedResult);
            Assert.Equal(1, pagedResult!.TotalRecords);
            Assert.Single(pagedResult.Items);
            Assert.Equal("Inspection Plan A", pagedResult.Items.First().Name);
        }

        [Fact]
        public async Task CreateInspectionPlan_ValidData_Sucess()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var newPlan = new InspectionPlanCreateRequest
            {
                Name = "Inspection Plan D",
                AreaId = 1,
            };
            var result = await controller.CreateInspectionPlan(newPlan);
            // Assert
            Assert.NotNull(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            var createdPlan = createdResult?.Value as InspectionPlanVm;
            Assert.NotNull(createdPlan);
            Assert.Equal("Inspection Plan D", createdPlan?.Name);
        }

        [Fact]
        public async Task CreateInspectionPlan_InvalidAreaId_Failure()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var newPlan = new InspectionPlanCreateRequest
            {
                Name = "Inspection Plan E",
                AreaId = 999, // Invalid AreaId
            };
            var result = await controller.CreateInspectionPlan(newPlan);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
            Assert.Equal("Invalid Production Area.", badRequestResult?.Value);
        }

        [Fact]
        public async Task CreateInspectionPlan_EmptyName_Failure()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var newPlan = new InspectionPlanCreateRequest
            {
                Name = "", // Empty Name
                AreaId = 1,
            };
            var result = await controller.CreateInspectionPlan(newPlan);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
            Assert.Equal("Inspection Plan name cannot be empty.", badRequestResult?.Value);
        }

        [Fact]
        public async Task CreateInspectionPlan_DuplicateName_Failure()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var newPlan = new InspectionPlanCreateRequest
            {
                Name = "Inspection Plan A", // Duplicate Name
                AreaId = 1,
            };
            var result = await controller.CreateInspectionPlan(newPlan);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
            Assert.Equal("Inspection Plan with the same name already exists in this Production Area.", badRequestResult?.Value);
        }

        [Fact]
        public async Task UpdateInspectionPlan_ValidData_Sucess()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var updatePlan = new InspectionPlanVm
            {
                Id = 1,
                Name = "Inspection Plan A Updated",
                AreaId = 1,
            };
            var result = await controller.UpdateInspectionPlan(1, updatePlan);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var updatedPlan = okResult?.Value as InspectionPlanVm;
            Assert.NotNull(updatedPlan);
            Assert.Equal("Inspection Plan A Updated", updatedPlan?.Name);
        }

        [Fact]
        public async Task UpdateInspectionPlan_InvalidId_Failure()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var updatePlan = new InspectionPlanVm
            {
                Id = 999, // Invalid Id
                Name = "Inspection Plan X",
                AreaId = 1,
            };
            var result = await controller.UpdateInspectionPlan(1, updatePlan);
            // Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult?.StatusCode);
            Assert.Equal("Invalid inspection plan Id.", badResult?.Value);
        }

        [Fact]
        public async Task UpdateInspectionPlan_NotFound_Failure()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var updatePlan = new InspectionPlanVm
            {
                Id = 999, // Non-existent Id
                Name = "Inspection Plan Y",
                AreaId = 1,
            };
            var result = await controller.UpdateInspectionPlan(999, updatePlan);
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult?.StatusCode);
            Assert.Equal("Inspection Plan not found.", notFoundResult?.Value);
        }

        [Fact]
        public async Task UpdateInspectionPlan_EmptyName_Failure()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var updatePlan = new InspectionPlanVm
            {
                Id = 2,
                Name = "", // Empty Name
                AreaId = 1,
            };
            var result = await controller.UpdateInspectionPlan(2, updatePlan);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
            Assert.Equal("Inspection Plan name cannot be empty.", badRequestResult?.Value);
        }

        [Fact]
        public async Task UpdateInspectionPlan_DuplicateName_Failure()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var updatePlan = new InspectionPlanVm
            {
                Id = 2,
                Name = "Inspection Plan A", // Duplicate Name
                AreaId = 1,
            };
            var result = await controller.UpdateInspectionPlan(2, updatePlan);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult?.StatusCode);
            Assert.Equal("Inspection plan with the same name already exists in this production area.", badRequestResult?.Value);
        }

        [Fact]
        public async Task DeleteInspectionPlan_ValidId_Sucess()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var result = await controller.DeleteInspectionPlan(3); // Assuming Id 3 exists
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var inspPlan = okResult?.Value as InspectionPlanVm;
            Assert.NotNull(inspPlan);
            Assert.Equal("Inspection Plan C", inspPlan?.Name);
        }

        [Fact]
        public async Task DeleteInspectionPlan_InvalidId_Failure()
        {
            var controller = new InspectionPlansController(_context);
            // Act
            var result = await controller.DeleteInspectionPlan(999); // Invalid Id
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult?.StatusCode);
            Assert.Equal("Inspection plan not found.", notFoundResult?.Value);
        }
    }
}
