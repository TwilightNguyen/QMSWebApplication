using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.InspectionPlanType;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class InspectionPlanTypesControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;
        public Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedInspPlanTypes(_context);
            InMemoryDbContext.SeedInspectionPlans(_context);
            InMemoryDbContext.SeedInspectionPlanSubs(_context);
            return Task.CompletedTask;
        }
        public async Task DisposeAsync()
        {
            try { await _context.Database.EnsureDeletedAsync(); } catch { }

            await _context.DisposeAsync();
        }

        [Fact]
        public async Task ShouldCreateInstance_NotNull_Success()
        {
            // Act
            var controller = new InspectionPlanTypesController(_context);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_ValidData_Success()
        {
            var controller = new InspectionPlanTypesController(_context);

            // Act 
            var result = await controller.GetAll();

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var inspPlanTypes = okResult?.Value as IEnumerable<InspectionPlanTypeVm>;

            Assert.NotNull(inspPlanTypes);
            Assert.Equal(4, inspPlanTypes.Count());
        }

        [Fact]
        public async Task GetByInspPlanId_ValidData_Success()
        {
            var controller = new InspectionPlanTypesController(_context);

            // Act
            var result = await controller.GetByInspPlanId(1);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var inspPlanTypes = okResult?.Value as IEnumerable<InspectionPlanTypeVm>;
            Assert.NotNull(inspPlanTypes);
            Assert.Equal(2, inspPlanTypes.Count());
        }

        [Fact]
        public async Task GetByInspPlanId_NotFound_Failure()
        {
            var controller = new InspectionPlanTypesController(_context);

            // Act 
            var result = await controller.GetByInspPlanId(999);

            // Assert
            Assert.NotNull(result); 
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal("Inspection Plan Type not found.", notFoundResult?.Value);
            Assert.Equal(404, notFoundResult?.StatusCode);
        }
    }
}

