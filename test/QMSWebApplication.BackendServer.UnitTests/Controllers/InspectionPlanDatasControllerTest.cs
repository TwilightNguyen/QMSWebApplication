using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.InspectionPlanData;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class InspectionPlanDatasControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;
        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedInspectionPlanSubs(_context);
            InMemoryDbContext.SeedInspPlanTypes(_context);
            InMemoryDbContext.SeedInspectionPlans(_context);
            InMemoryDbContext.SeedCharacteristics(_context);
            InMemoryDbContext.SeedInspectionPlanDatas(_context);
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
            var controller = new InspectionPlanDatasController(_context);
            // Assert
            Assert.NotNull(controller);
        }


        [Fact]
        public async Task CreateInspectionPlanData_ValidData_Exist_Success()
        {
            // Arrange: use in-memory EF Core or mocks; seed any required data
            var controller = new InspectionPlanDatasController(_context);

            var request = new InspectionPlanDataCreateRequest
            {
                CharacteristicId = 1,
                InspPlanSubId = 1,
                SPCChart = true,
                DataEntry = true,
                CpkControl = true,
                // Prefer correct type for SampleSize; if int:
                SampleSize = "10"
            };

            // If you rely on DataAnnotations, ensure ModelState is valid
            // No manual action needed if the object is valid

            // Act
            var result = await controller.CreateInspectionPlanData(request);

            // Assert (depending on your action's return type)
            Assert.NotNull(result);
            var okResult = result as CreatedAtActionResult;

            var createdData = okResult?.Value as InspectionPlanDataVm;
            Assert.NotNull(createdData);
            // Optionally cast Value and assert fields
            Assert.Equal(request.CharacteristicId, createdData.CharacteristicId);
            Assert.Equal(request.InspPlanSubId, createdData.InspPlanSubId);
        }

        [Fact]
        public async Task CreateInspectionPlanData_ValidData_NotExist_Success()
        {
            // Arrange: use in-memory EF Core or mocks; seed any required data
            var controller = new InspectionPlanDatasController(_context);

            var request = new InspectionPlanDataCreateRequest
            {
                CharacteristicId = 3,
                InspPlanSubId = 1,
                SPCChart = true,
                DataEntry = true, 
                CpkControl = true,
                // Prefer correct type for SampleSize; if int:
                SampleSize = "10"
            };

            // If you rely on DataAnnotations, ensure ModelState is valid
            // No manual action needed if the object is valid

            // Act
            var result = await controller.CreateInspectionPlanData(request);

            // Assert (depending on your action's return type)
            Assert.NotNull(result);
            var okResult = result as CreatedAtActionResult;

            var createdData = okResult?.Value as InspectionPlanDataVm;
            Assert.NotNull(createdData);
            // Optionally cast Value and assert fields
            Assert.Equal(request.CharacteristicId, createdData.CharacteristicId);
            Assert.Equal(request.InspPlanSubId, createdData.InspPlanSubId);
        }

        [Fact]
        public async Task CreateInspectionPlanData_InvalidInspectionPlanSub_Failure()
        {
            // Arrange: use in-memory EF Core or mocks; seed any required data
            var controller = new InspectionPlanDatasController(_context);
            var request = new InspectionPlanDataCreateRequest
            {
                CharacteristicId = 1,
                InspPlanSubId = 999, // Non-existent InspPlanSubId
                SPCChart = true,
                DataEntry = true,
                CpkControl = true,
                SampleSize = "10"
            };

            // Act
            var result = await controller.CreateInspectionPlanData(request);
            
            // Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);
            Assert.Equal("Invalid Inspection Plan Sub.", badResult.Value);

        }

        [Fact]
        public async Task GetById_ValidData_Sucess()
        {
            var controller = new InspectionPlanDatasController(_context);

            // Action 
            var result = await controller.GetById(1);

            //Asert
            Assert.NotNull(result);
            var okReuslt = result as OkObjectResult;
            Assert.NotNull(okReuslt);
            Assert.Equal(200, okReuslt.StatusCode);

            var data = okReuslt?.Value as InspectionPlanDataVm;
            Assert.NotNull(data);
            Assert.Equal(1, data.Id);
        }

        [Fact]
        public async Task GetById_NotFound_Failure()
        {

            var controller = new InspectionPlanDatasController(_context);

            // Action 
            var result = await controller.GetById(999);
            
            //Assert 
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Inspection Plan Data not found.", notFoundResult.Value);

        }

        [Fact]
        public async Task GetByInsPlanSubIdAndPlanState_ValidData_Sucess()
        {
            var controller = new InspectionPlanDatasController(_context);

            // Action 
            var result = await controller.GetByInsPlanSubIdAndPlanState(1, 1);

            // Assert 
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var datas = okResult.Value as IEnumerable<InspectionPlanDataVm>;
            Assert.Equal(2, datas?.Count());
        }

        [Fact]
        public async Task GetByInsPlanSubIdAndPlanState_InspectionPlanSubNotFound_Failure()
        {
            var controller = new InspectionPlanDatasController (_context);

            // Action
            var result = await controller.GetByInsPlanSubIdAndPlanState(999, 1);
            
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult?.StatusCode);
            Assert.Equal("Inspection Plan Sub not found.", notFoundResult!.Value);
        }

        [Fact]
        public async Task DeleteInspectionPlanData_ValidData_Success()
        {
            var controller = new InspectionPlanDatasController(_context);

            //Action 
            var result = await controller.DeleteInspectionPlanData(3);
            
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var data = okResult?.Value as InspectionPlanDataVm;
            Assert.Equal(3, data?.Id);
        }

        [Fact]
        public async Task DeleteInspectionPlanData_InspectionPlanDataNotFound_Failure()
        {
            var controller = new InspectionPlanDatasController(_context);

            //Action 
            var result = await controller.DeleteInspectionPlanData(999);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult?.StatusCode);
            Assert.Equal("Inspection Plan Data not found.", notFoundResult?.Value);
        }
    }
}
