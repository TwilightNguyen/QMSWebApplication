using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.MeasData;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class MeasDatasControllerTest : IAsyncLifetime
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
            InMemoryDbContext.SeedMeasDatas(_context);

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
            /// Act
            var controller = new SamplesController(_context);

            /// Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task CreateMeasData_ValidData_Success()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.CreateMeasData(new MeasDataCreateRequest
            {
                ProductionId = 3,
                PlanTypeId = 1,
                MoldId = 1, 
                CavityId = 1,
                SampleQunality = 3,
                Notes = "Create Note 1",
                CharacteristicRange = "Create Range 1",
                Values = [ 
                    new MeasDataValue{CharacteristicId = 1, CharacteristicValue = [10.3f, 9f ,10f]}
                ]
            });

            /// Assert
            Assert.NotNull(result);
            var actionResult = result as CreatedAtActionResult;
            Assert.NotNull(actionResult);
            var sample = actionResult?.Value as int? ?? 0;
            Assert.NotEqual(0, sample);
        }

        [Fact]
        public async Task CreateMeasData_InvalidProduction_Failed()
        {
            var controller = new SamplesController(_context); 

            /// Act
            var result = await controller.CreateMeasData(new MeasDataCreateRequest
            {
                ProductionId = -1,
                PlanTypeId = 1,
                MoldId = 1,
                CavityId = 1,
                SampleQunality = 3,
                Notes = "Create Note 2",
                CharacteristicRange = "Create Range 2",
                Values = [
                    new() {CharacteristicId = 1, CharacteristicValue = [10.3f, 9f ,10f]}
                ]
            });

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid Production.", badResult?.Value);
        }

        [Fact]
        public async Task CreateMeasData_InvalidCharacteristic_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.CreateMeasData(new MeasDataCreateRequest
            {
                ProductionId = 1,
                PlanTypeId = 1,
                MoldId = 1,
                CavityId = 1,
                SampleQunality = 3,
                Notes = "Create Note 3",
                CharacteristicRange = "Create Range 3",
                Values = [
                    new() {CharacteristicId = -1, CharacteristicValue = [10.3f, 9f ,10f]}
                ]
            });

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid Characteristic.", badResult?.Value);
        }

        [Fact]
        public async Task CreateMeasData_InvalidPlantype_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.CreateMeasData(new MeasDataCreateRequest
            {
                ProductionId = 1,
                PlanTypeId = -2,
                MoldId = 1,
                CavityId = 1,
                SampleQunality = 3,
                Notes = "Create Note 4",
                CharacteristicRange = "Create Range 4",
                Values = [
                    new() {CharacteristicId = 1, CharacteristicValue = [10.3f, 9f ,10f]}
                ]
            });

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid Plan Type.", badResult?.Value);
        }

        [Fact]
        public async Task CreateMeasData_NotFoundProduction_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.CreateMeasData(new MeasDataCreateRequest
            {
                ProductionId = 1,
                PlanTypeId = 3,
                MoldId = 1,
                CavityId = 1,
                SampleQunality = 3,
                Notes = "Create Note 5",
                CharacteristicRange = "Create Range 5",
                Values = [
                    new() {CharacteristicId = 1, CharacteristicValue = [10.3f, 9f ,10f]}
                ]
            });

            /// Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("No Production Data found.", notFoundResult?.Value);
        }

        [Fact]
        public async Task CreateMeasData_InvalidMold_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.CreateMeasData(new MeasDataCreateRequest
            {
                ProductionId = 1,
                PlanTypeId = 1,
                MoldId = 999,
                CavityId = 1,
                SampleQunality = 3,
                Notes = "Create Note 6",
                CharacteristicRange = "Create Range 6",
                Values = [
                    new() {CharacteristicId = 1, CharacteristicValue = [10.3f, 9f ,10f]}
                ]
            });

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid Mold.", badResult?.Value);
        }

        [Fact]
        public async Task CreateMeasData_InvalidCavity_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.CreateMeasData(new MeasDataCreateRequest
            {
                ProductionId = 1,
                PlanTypeId = 1,
                MoldId = 1,
                CavityId = 999,
                SampleQunality = 3,
                Notes = "Create Note 7",
                CharacteristicRange = "Create Range 7",
                Values = [
                    new() {CharacteristicId = 1, CharacteristicValue = [10.3f, 9f ,10f]}
                ]
            });

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid Cavity.", badResult?.Value);
        }

        [Fact]
        public async Task CreateMeasData_NotFoundCharacteristicInProduction_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.CreateMeasData(new MeasDataCreateRequest
            {
                ProductionId = 1,
                PlanTypeId = 1,
                MoldId = 1,
                CavityId = 1,
                SampleQunality = 3,
                Notes = "Create Note 8",
                CharacteristicRange = "Create Range 8",
                Values = [
                    new() {CharacteristicId = 999, CharacteristicValue = [10.3f, 9f ,10f]}
                ]
            });

            /// Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Not Found Characteristic In Production.", notFoundResult?.Value);
        } 

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_ValidData_Success()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                1, 1, 1, 1, 1
            );

            /// Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var measDatas = okResult?.Value as IEnumerable<MeasDataVm>;
            Assert.NotNull(measDatas);
            Assert.Equal(5, measDatas.Count());

        }

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_InvalidProduction_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                -1, 1, 1, 1, 1
            );

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid Production Id.", badResult?.Value);
        }

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_InvalidCharacteristic_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                1, -1, 1, 1, 1
            );

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid Characteristic Id.", badResult?.Value);
        }

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_InvalidPlantype_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                1, 1, -1, 1, 1
            );

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid Plan Type Id.", badResult?.Value);
        }

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_InvalidMold_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                1, 1, 1, -2, 1
            );

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid Mold Id.", badResult?.Value);
        }

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_InvalidCavity_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                1, 1, 1, 1, -2
            );

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid Cavity Id.", badResult?.Value);
        }

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_NotFoundCharacteristicInProduction_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                1, 999, 1, 1, 1
            );

            /// Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Not Found Characteristic in Production.", notFoundResult?.Value);
        }

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_NotFoundProduction_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                999, 1, 1, 1, 1
            );

            /// Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Production Data Id not found.", notFoundResult?.Value);
        }

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_NotFoundPlanType_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                1, 1, 999, 1, 1
            );

            /// Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Plan Type not found.", notFoundResult?.Value);
        }

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_NotFoundMoldInProduction_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                1, 1, 1, 999, 1
            );

            /// Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Mold not found.", notFoundResult?.Value);
        }

        [Fact]
        public async Task GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId_NotFoundCavityInProduction_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
                1, 1, 1, 1, 999
            );

            /// Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Cavity not found.", notFoundResult?.Value);
        }

        [Fact]
        public async Task DeleteMeasData_ValidData_Success()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.DeleteMeasData(1);

            /// Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var measData = okResult.Value as MeasDataVm;
            Assert.NotNull(measData);
            Assert.Equal(1, measData.Id);
        }

        [Fact]
        public async Task DeleteMeasData_InvalidMeasurementData_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.DeleteMeasData(-1);

            /// Assert
            Assert.NotNull(result );
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Invalid measurement data.", badResult?.Value);
        }

        [Fact]
        public async Task DeleteMeasData_NotFoundMeasurementData_Failed()
        {
            var controller = new SamplesController(_context);

            /// Act
            var result = await controller.DeleteMeasData(999);

            /// Assert
            Assert.NotNull(result);
            var badResult = result as BadRequestObjectResult;
            Assert.NotNull(badResult);
            Assert.Equal("Measurement data not found.", badResult?.Value);

        }
    }
}
