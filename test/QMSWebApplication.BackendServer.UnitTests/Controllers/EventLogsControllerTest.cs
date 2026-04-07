using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.EventLog;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class EventLogsControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;

        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedMeasureTypes(_context);
            InMemoryDbContext.SeedProcesses(_context);
            InMemoryDbContext.SeedCharacteristics(_context);
            InMemoryDbContext.SeedEventLogs(_context);
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
            var controller = new EventLogController(_context);

            // Act
            var result = await controller.GetAll();
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var eventLogList = okResult?.Value as IEnumerable<EventLogVm>;
            Assert.Equal(3, eventLogList?.Count());
            Assert.Contains(eventLogList!, a => a.Description == "User Admin logged in");
            Assert.Contains(eventLogList!, a => a.Description == "User Admin logged out");
            Assert.Contains(eventLogList!, a => a.Description == "Error occurred");

        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            var controller = new EventLogController(_context);

            // Act
            var result = await controller.GetById(1);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var eventLog = okResult?.Value as EventLogVm;
            Assert.Equal(1, eventLog?.Id);
        }

        [Fact]
        public async Task GetById_NotFound_Failed()
        {
            // Act
            var controller = new EventLogController(_context);

            var result = await controller.GetById(4);

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Event log not found.", notFoundRequest.Value);
        }

        [Fact]
        public async Task GetEventLogPaging_NoFilter_ReturnSuccess()
        {

            // Act
            var controller = new EventLogController(_context);

            var result = await controller.GetPaging(null, 1, 2);
            var OkResult = result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var eventLogList = OkResult?.Value as Pagination<EventLogVm>;
            Assert.NotNull(eventLogList);
            Assert.Equal(3, eventLogList.TotalRecords);
            Assert.Equal(2, eventLogList.Items.Count);
        }

        [Fact]
        public async Task GetEventLogPaging_HasFilter_ReturnSuccess()
        {
            // Act
            var controller = new EventLogController(_context);

            var result = await controller.GetPaging("192.168.1.12", 1, 2);
            var OkResult = result as OkObjectResult;

            // Assert 
            Assert.NotNull(result);
            var EventLogList = OkResult?.Value as Pagination<EventLogVm>;
            Assert.NotNull(EventLogList);
            Assert.Equal(2, EventLogList.TotalRecords);
            Assert.Equal(2, EventLogList.Items.Count);
            //Assert.Single(EventLogList.Items);
        }

        [Fact]
        public async Task GetEventLogPaging_ThrowException_Failed()
        {
            // Act
            var controller = new EventLogController(_context);

            var result = await controller.GetPaging("", -1, -1);
            var notfoundResult = result as NotFoundObjectResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal("No event logs found.", notfoundResult?.Value);
        }
    }
}
