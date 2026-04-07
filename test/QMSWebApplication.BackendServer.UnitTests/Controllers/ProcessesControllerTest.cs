using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels.System.Process;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class ProcessesControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;

        //public ProcessesControllerTest()
        //{
        //    _context = InMemoryDbContext.GetApplicationDbContext();
        //    InMemoryDbContext.SeedProductionAreas(_context);
        //    InMemoryDbContext.SeedProcesses(_context);
        //}

        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedProductionAreas(_context);
            InMemoryDbContext.SeedProcesses(_context);
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
            var controller = new ProcessesController(_context);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_HasData_Success()
        {
            var controller = new ProcessesController(_context);

            // Act
            var result = await controller.GetAll();
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var processesList = okResult?.Value as IEnumerable<ProcessVm>;
            Assert.Equal(4, processesList?.Count());
            Assert.Contains(processesList!, a => a.Name == "Process A");
            Assert.Contains(processesList!, a => a.Name == "Process B");
            Assert.Contains(processesList!, a => a.Name == "Process C");
            Assert.Contains(processesList!, a => a.Name == "Process D");

        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            var controller = new ProcessesController(_context);

            // Act
            var result = await controller.GetById(1);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var process = okResult?.Value as ProcessVm;
            Assert.Equal(1, process?.Id);
        }

        [Fact]
        public async Task GetById_NotFound_Failed()
        {
            // Act
            var controller = new ProcessesController(_context);


            var result = await controller.GetById(999);

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Process not found.", notFoundRequest.Value);
        }

        [Fact]
        public async Task GetByAreaId_HasData_Success()
        {
            var controller = new ProcessesController(_context);

            // Act
            var result = await controller.GetByAreaId(1);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var processesList = okResult?.Value as IEnumerable<ProcessVm>;
            Assert.Equal(2, processesList?.Count());
            Assert.Contains(processesList!, a => a.Name == "Process A");
            Assert.Contains(processesList!, a => a.Name == "Process B");
        }

        [Fact]
        public async Task GetByAreaId_ProcessesNotFound_Failed()
        {
            // Act
            var controller = new ProcessesController(_context);

            var result = await controller.GetByAreaId(4);

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No processes found.", notFoundRequest.Value);
        }

        [Fact]
        public async Task GetByAreaId_ProductionAreaNotFound_Failed()
        {
            // Act
            var controller = new ProcessesController(_context);

            var result = await controller.GetByAreaId(999);

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Production Area not found.", notFoundRequest.Value);
        }
    }
}
