using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.Shift;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class ShiftsControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;
        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedShifts(_context);
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            try { await _context.Database.EnsureDeletedAsync(); } catch { }
            await _context.DisposeAsync();
        }

        [Fact]
        public async Task ShouldCreateInstance_NotNull_Success() {
            // Act
            var controller = new ShiftsController(_context);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact] 
        public async Task GetAll_HasData_Success()
        {
            var controller = new ShiftsController(_context);

            // Act 
            var result = await controller.GetAll();

            // Assert 
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var shifts = okResult?.Value as IEnumerable<ShiftVm>;
            Assert.NotNull(shifts);
            Assert.Equal(2, shifts.Count());
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            var controller = new ShiftsController(_context);

            // Act 
            var result = await controller.GetById(1);

            // Assert 
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var shift = okResult?.Value as ShiftVm;
            Assert.NotNull(shift);
            Assert.Equal(1, shift.Id);
            Assert.Equal("Shift 1",shift.Name);
            Assert.Equal(new TimeSpan(6, 0, 0), shift.StartTime);
            Assert.Equal(new TimeSpan(18, 0, 0), shift.EndTime);
        }

        [Fact]
        public async Task GetById_NotFound_Failure()
        {
            var controller = new ShiftsController(_context);

            // Act
            var result = await controller.GetById(999);

            // Assert
            Assert.NotNull(result);
            var notFound = result as NotFoundObjectResult;
            Assert.NotNull(notFound);
            Assert.Equal(404, notFound?.StatusCode);
            Assert.Equal("Shift not found.", notFound?.Value);
        }
    }
}
