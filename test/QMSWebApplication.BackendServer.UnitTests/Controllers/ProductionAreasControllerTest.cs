using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.ProductionArea;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class ProductionAreasControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;

        //public ProductionAreasControllerTest()
        //{
        //    _context = InMemoryDbContext.GetApplicationDbContext();

        //    InMemoryDbContext.SeedProductionAreas(_context);
        //}


        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedProductionAreas(_context);
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
            var controller = new ProductionAreasController(_context);

            // Assert
            Assert.NotNull(controller);

        }

        [Fact]
        public async Task GetAll_HasData_Success()
        {
            var controller = new ProductionAreasController(_context);
             
            // Act
            var result = await controller.GetAll();
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var areasList = okResult?.Value as IEnumerable<ProductionAreaVm>;
            Assert.Equal(4, areasList?.Count());
            Assert.Contains(areasList!, a => a.Name == "Tape");
            Assert.Contains(areasList!, a => a.Name == "Layout");
            Assert.Contains(areasList!, a => a.Name == "Block Vial");
            Assert.Contains(areasList!, a => a.Name == "Print");
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            var controller = new ProductionAreasController(_context);

            // Act
            var result = await controller.GetById(1);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var areasList = okResult?.Value as ProductionAreaVm;
            Assert.Equal(1, areasList?.Id);
        }

        [Fact]
        public async Task GetById_NotFound_Failed()
        {
            // Act
            var controller = new ProductionAreasController(_context);
            

            var result = await controller.GetById(999);

            // Assert
            Assert.NotNull(result);

            var notFoundRequest = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Production Area not found.", notFoundRequest.Value);
        }
    }
}
