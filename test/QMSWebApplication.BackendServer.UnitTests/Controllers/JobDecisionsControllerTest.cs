using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.JobDecision;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class JobDecisionsControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;
        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedJobDecisions(_context);

            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            try { await _context.Database.EnsureDeletedAsync(); } catch { /* ignore */ }
            await _context.DisposeAsync();
        }

        [Fact]
        public async Task ShouldCreateInstance_NotNull_Success()
        {
            // Act
            var controller = new JobDecisionsController(_context);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_ValidData_Success()
        {
            var controller = new JobDecisionsController(_context);

            // Act
            var result = await controller.GetAll();
           
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var jobDecisions = okResult?.Value as IEnumerable<JobDecisionVm>;
            Assert.NotNull(jobDecisions);
            Assert.Equal(6, jobDecisions.Count());
        }
    }
}
