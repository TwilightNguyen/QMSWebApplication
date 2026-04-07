using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.InspectionPlanSub;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class InspectionPlanSubsControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;
        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedInspPlanTypes(_context);
            InMemoryDbContext.SeedInspectionPlans(_context);
            InMemoryDbContext.SeedInspectionPlanSubs(_context);
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
            var controller = new InspectionPlanSubsController(_context);
            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_HasData_Sucess()
        {
            // Act
            var controller = new InspectionPlanSubsController(_context);

            var result = await controller.GetAll();
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var inspectionPlanSubList = okResult?.Value as IEnumerable<InspectionPlanSubVm>;
            Assert.Equal(3, inspectionPlanSubList?.Count());
            Assert.Contains(inspectionPlanSubList!, a => a.InspPlanId == 1 && a.PlanTypeId == 1);
            Assert.Contains(inspectionPlanSubList!, a => a.InspPlanId == 1 && a.PlanTypeId == 2);
            Assert.Contains(inspectionPlanSubList!, a => a.InspPlanId == 2 && a.PlanTypeId == 1);

        }

        [Fact]
        public async Task GetAll_NoData_ReturnsEmptyList()
        {
            // Arrange
            var context = InMemoryDbContext.GetApplicationDbContext();
            var controller = new InspectionPlanSubsController(context);
            // Act
            var result = await controller.GetAll();
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var inspectionPlanSubList = okResult?.Value as IEnumerable<InspectionPlanSubVm>;
            Assert.NotNull(inspectionPlanSubList);
            Assert.Empty(inspectionPlanSubList!);
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            var controller = new InspectionPlanSubsController(_context);
            // Act
            var result = await controller.GetById(1);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var inspectionPlanSub = okResult?.Value as InspectionPlanSubVm;
            Assert.NotNull(inspectionPlanSub);
            Assert.Equal(1, inspectionPlanSub!.InspPlanId);
            Assert.Equal(1, inspectionPlanSub.PlanTypeId);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            var controller = new InspectionPlanSubsController(_context);
            // Act
            var result = await controller.GetById(999); // Assuming 999 does not exist
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult!.StatusCode);
            Assert.Equal("Inspection Plan Sub not found.", notFoundResult!.Value);
        }

        [Fact]
        public async Task Create_ValidRequest_Success()
        {
            // Arrange
            var controller = new InspectionPlanSubsController(_context);
            var request = new InspectionPlanSubCreateRequest
            {
                InspPlanId = 2,
                PlanTypeId = 2
            };
            // Act
            var result = await controller.CreateInspectionPlanSub(request);
            // Assert
            Assert.NotNull(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            var createdInspectionPlanSub = createdAtActionResult!.Value as InspectionPlanSubVm;
            Assert.NotNull(createdInspectionPlanSub);
            Assert.Equal(request.InspPlanId, createdInspectionPlanSub!.InspPlanId);
            Assert.Equal(request.PlanTypeId, createdInspectionPlanSub.PlanTypeId);
        }

        [Fact]
        public async Task Create_DuplicateRequest_ReturnsBadRequest()
        {
            // Arrange
            var controller = new InspectionPlanSubsController(_context);
            var request = new InspectionPlanSubCreateRequest
            {
                InspPlanId = 1,
                PlanTypeId = 1
            };
            // Act
            var result = await controller.CreateInspectionPlanSub(request);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult!.StatusCode);
            Assert.Equal("Inspection Plan Sub with the same Inspection Plan and Plan Type already exists.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_InvalidInspPlanId_ReturnsBadRequest()
        {
            // Arrange
            var controller = new InspectionPlanSubsController(_context);
            var request = new InspectionPlanSubCreateRequest
            {
                InspPlanId = 999, // Invalid ID
                PlanTypeId = 1
            };
            // Act
            var result = await controller.CreateInspectionPlanSub(request);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult!.StatusCode);
            Assert.Equal("Invalid Inspection Plan.", badRequestResult.Value);
        }

        [Fact]
        public async Task Create_InvalidPlanTypeId_ReturnsBadRequest()
        {
            // Arrange
            var controller = new InspectionPlanSubsController(_context);
            var request = new InspectionPlanSubCreateRequest
            {
                InspPlanId = 1,
                PlanTypeId = 999 // Invalid ID
            };
            // Act
            var result = await controller.CreateInspectionPlanSub(request);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult!.StatusCode);
            Assert.Equal("Invalid Inspection Plan Type.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetByInsPlanID_ValidId_ReturnsSucess()
        {
            // Arrange
            var controller = new InspectionPlanSubsController(_context);
            int inspPlanId = 1;
            // Act
            var result = await controller.GetByInsPlanId(inspPlanId);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var inspectionPlanSubList = okResult?.Value as IEnumerable<InspectionPlanSubVm>;
            Assert.NotNull(inspectionPlanSubList);
            Assert.Single(inspectionPlanSubList!);
            Assert.All(inspectionPlanSubList, item => Assert.Equal(inspPlanId, item.InspPlanId));
        }

        [Fact]
        public async Task GetByInsPlanID_InvalidId_ReturnsEmptyList()
        {
            // Arrange
            var controller = new InspectionPlanSubsController(_context);
            int inspPlanId = 999; // Assuming this ID does not exist
            // Act
            var result = await controller.GetByInsPlanId(inspPlanId);
            // Assert
            Assert.NotNull(result);
            var notfoundResult = result as NotFoundObjectResult;
            Assert.Equal(404, notfoundResult!.StatusCode);
            Assert.Equal("No Inspection Plan Subs found for the given Inspection Plan.", notfoundResult!.Value);
        }

        [Fact]
        public async Task DeleteInspectionPlanSub_ValidId_ReturnSucess()
        {
            var controller = new InspectionPlanSubsController(_context);
            // Act
            var result = await controller.DeleteInspectionPlanSub(1);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult!.StatusCode);

            var inspectionPlanSubList = okResult.Value as InspectionPlanSubVm;
            Assert.NotNull(inspectionPlanSubList);
            Assert.Equal(1, inspectionPlanSubList!.Id);
        }

        [Fact]
        public async Task DeleteInspectionPlanSub_InvalidId_ReturnsNotFound()
        {
            var controller = new InspectionPlanSubsController(_context);
            // Act
            var result = await controller.DeleteInspectionPlanSub(999); // Assuming 999 does not exist
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult!.StatusCode);
            Assert.Equal("Inspection Plan Sub not found.", notFoundResult!.Value);
        }
    }
}
