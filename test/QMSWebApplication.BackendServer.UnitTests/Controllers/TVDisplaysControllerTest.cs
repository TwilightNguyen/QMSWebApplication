using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.Product;
using QMSWebApplication.ViewModels.System.TVDisplay;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class TVDisplaysControllerTest : IAsyncLifetime
    {

        public required ApplicationDbContext _context;
        public async Task InitializeAsync()
        {

            _context = InMemoryDbContext.GetApplicationDbContext();

            InMemoryDbContext.SeedUsers(_context);
            InMemoryDbContext.SeedRoles(_context);
            InMemoryDbContext.SeedProductionAreas(_context);
            InMemoryDbContext.SeedProcesses(_context);
            InMemoryDbContext.SeedProcessLines(_context);
            InMemoryDbContext.SeedCharacteristics(_context);
            InMemoryDbContext.SeedEventLogs(_context);
            InMemoryDbContext.SeedInspPlanTypes(_context);
            InMemoryDbContext.SeedInspectionPlans(_context);
            InMemoryDbContext.SeedInspectionPlanSubs(_context);
            InMemoryDbContext.SeedInspectionPlanDatas(_context);
            InMemoryDbContext.SeedProducts(_context);
            InMemoryDbContext.SeedJobDecisions(_context);
            InMemoryDbContext.SeedJobs(_context);
            InMemoryDbContext.SeedProductionPlans(_context);
            InMemoryDbContext.SeedTvDisplay(_context);

            await _context.SaveChangesAsync();
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
            var controller = new TVDisplaysController(_context);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_HasData_Success() {
            var controller = new TVDisplaysController(_context);

            // Act
            var result = await controller.GetAll();

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var tvDisplays = okResult?.Value as IEnumerable<TVDisplayVm>;

            Assert.NotNull(tvDisplays);
            Assert.Equal(2, tvDisplays.Count());
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            var controller = new TVDisplaysController(_context);

            // Act 
            var result = await controller.GetById(1);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var product = okResult?.Value as TVDisplayVm;
            Assert.Equal(1, product?.Id);
            Assert.Equal("Tapes", product?.Name);
        }

        [Fact]
        public async Task GetById_NotFound_Failure()
        {
            var controller = new TVDisplaysController(_context);

            // Act
            var result = await controller.GetById(999);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal("Tv Display not found.", notFoundResult?.Value);
            Assert.Equal(404, notFoundResult?.StatusCode);
        }
    }
}
