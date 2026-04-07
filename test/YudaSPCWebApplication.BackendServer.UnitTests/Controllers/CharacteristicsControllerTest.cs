using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.Characteristic;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class CharacteristicControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;

        //public CharacteristicControllerTest()
        //{
        //    _context = InMemoryDbContext.GetApplicationDbContext();
        //    InMemoryDbContext.SeedMeasureTypes(_context);
        //    InMemoryDbContext.SeedCharacteristics(_context);
        //}

        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedMeasureTypes(_context);
            InMemoryDbContext.SeedProcesses(_context);
            InMemoryDbContext.SeedCharacteristics(_context);
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
            var controller = new CharacteristicsController(_context);
            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_HasData_Success()
        {
            var controller = new CharacteristicsController(_context);
            // Act
            var result = await controller.GetAll();
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var characteristicsList = okResult?.Value as IEnumerable<CharacteristicVm>;

            Assert.Equal(3, characteristicsList?.Count());
            Assert.Contains(characteristicsList!, a => a.Name == "Characteristic A");
            Assert.Contains(characteristicsList!, a => a.Name == "Characteristic B");
            Assert.Contains(characteristicsList!, a => a.Name == "Characteristic C");
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            var controller = new CharacteristicsController(_context);
            // Act
            var result = await controller.GetById(1);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var characteristic = okResult?.Value as CharacteristicVm;
            Assert.NotNull(characteristic);
            Assert.Equal("Characteristic A", characteristic?.Name);
        }

        [Fact]
        public async Task GetById_NotFound_Failure()
        {
            var controller = new CharacteristicsController(_context);
            // Act
            var result = await controller.GetById(999);
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult?.StatusCode);
        }

        [Fact]
        public async Task GetByProcessId_HasData_Success()
        {
            var controller = new CharacteristicsController(_context);
            // Act
            var result = await controller.GetByProcessId(1);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var characteristicsList = okResult?.Value as IEnumerable<CharacteristicVm>;
            Assert.NotNull(characteristicsList);
            Assert.Equal(2, characteristicsList?.Count());
            Assert.Contains(characteristicsList!, a => a.Name == "Characteristic A");
            Assert.Contains(characteristicsList!, a => a.Name == "Characteristic B");
        }

        [Fact]
        public async Task GetByProcessId_NotFound_Failure()
        {
            var controller = new CharacteristicsController(_context);
            // Act
            var result = await controller.GetByProcessId(999);
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("No characteristics found for the specified process.", notFoundResult?.Value);
        }


        [Fact]
        public async Task GetPaging_NoFilter_Success()
        {
            var controller = new CharacteristicsController(_context);
            // Act
            var result = await controller.GetPaging(null, 1, 2);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var pagedResult = okResult?.Value as Pagination<CharacteristicVm>;
            Assert.NotNull(pagedResult);
            Assert.Equal(3, pagedResult?.TotalRecords);
            Assert.Equal(2, pagedResult?.Items.Count);
        }

        [Fact]
        public async Task GetPaging_WithFilter_Success()
        {
            var controller = new CharacteristicsController(_context);
            // Act
            var result = await controller.GetPaging("Characteristic B", 1, 2);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var pagedResult = okResult?.Value as Pagination<CharacteristicVm>;
            Assert.NotNull(pagedResult);
            Assert.Equal(1, pagedResult?.TotalRecords);
            Assert.Equal(1, pagedResult?.Items?.Count);
            Assert.Equal("Characteristic B", pagedResult?.Items[0].Name);
        }

        [Fact]
        public async Task CreateCharaterictic_ValidData_Sucess()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic D",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 0,
                Unit = "Unit D",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 2
            };

            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            var characteristicVm = createdAtActionResult?.Value as CharacteristicVm;
            Assert.NotNull(characteristicVm);
            Assert.Equal("Characteristic D", characteristicVm?.Name);
        }

        [Fact]
        public async Task CreateCharaterictic_CharacteristicNameNotBeEmpty_Failure()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 0,
                Unit = "Unit D",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Characteristic name cannot be empty.", badRequestResult?.Value);
        }

        [Fact]
        public async Task CreateCharaterictic_InvalidMeaTypeId_Failure()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic D",
                MeaTypeId = 999,
                ProcessId = 1,
                DataType = 0,
                Unit = "Unit D",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Invalid Measure Type.", badRequestResult?.Value);
        }

        [Fact]
        public async Task CreateCharaterictic_InvalidProcessId_Failure()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic D",
                MeaTypeId = 1,
                ProcessId = 999,
                DataType = 0,
                Unit = "Unit D",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Invalid Process.", badRequestResult?.Value);
        }

        [Fact]
        public async Task CreateCharaterictic_InvalidCharacteristicType_Failure()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic D",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 999,
                Unit = "Unit D",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Invalid Characteristic Type.", badRequestResult?.Value);
        }

        [Fact]
        public async Task CreateCharaterictic_DuplicateCharacteristicName_Failure()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic A",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 0,
                Unit = "Unit D",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Characteristic with the same name already exists in this process.", badRequestResult?.Value);
        }

        [Fact]
        public async Task CreateCharaterictic_NegativeDecimals_Failure()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic D",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 1,
                Unit = "Unit D",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = -1
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Decimals cannot be negative.", badRequestResult?.Value);
        }

        [Fact]
        public async Task CreateCharaterictic_NullCharacteristicUnit_Success()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic E",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 1,
                Unit = null,
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            var characteristicVm = createdAtActionResult?.Value as CharacteristicVm;
            Assert.NotNull(characteristicVm);
            Assert.Equal("Characteristic E", characteristicVm?.Name);
            Assert.Null(characteristicVm?.Unit);
        }

        [Fact]
        public async Task CreateCharaterictic_ZeroDefectRateLimit_Success()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic F",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 0,
                Unit = "Unit F",
                DefectRateLimit = 0,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            var characteristicVm = createdAtActionResult?.Value as CharacteristicVm;
            Assert.NotNull(characteristicVm);
            Assert.Equal("Characteristic F", characteristicVm?.Name);
            Assert.Equal(0, characteristicVm?.DefectRateLimit);
        }

        [Fact]
        public async Task CreateCharaterictic_NullDefectRateLimit_Success()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic G",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 0,
                Unit = "Unit G",
                DefectRateLimit = null,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            var characteristicVm = createdAtActionResult?.Value as CharacteristicVm;
            Assert.NotNull(characteristicVm);
            Assert.Equal("Characteristic G", characteristicVm?.Name);
            Assert.Null(characteristicVm?.DefectRateLimit);
        }

        [Fact]
        public async Task CreateCharaterictic_ZeroEventEnable_Success()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic H",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 1,
                Unit = "Unit H",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            var characteristicVm = createdAtActionResult?.Value as CharacteristicVm;
            Assert.NotNull(characteristicVm);
            Assert.Equal("Characteristic H", characteristicVm?.Name);
        }

        [Fact]
        public async Task CreateCharaterictic_ZeroEmailEventModel_Success()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic I",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 0,
                Unit = "Unit I",
                DefectRateLimit = 5,
                EmailEventModel = 0,
                Decimals = 2
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            var characteristicVm = createdAtActionResult?.Value as CharacteristicVm;
            Assert.NotNull(characteristicVm);
            Assert.Equal("Characteristic I", characteristicVm?.Name);
            Assert.Equal(0, characteristicVm?.EmailEventModel);
        }

        [Fact]
        public async Task CreateCharaterictic_ZeroDecimals_Success()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicCreateRequest
            {
                Name = "Characteristic J",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 0,
                Unit = "Unit J",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 0
            };
            // Act
            var result = await controller.CreateCharacteristic(request);
            // Assert
            Assert.NotNull(result);
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            var characteristicVm = createdAtActionResult?.Value as CharacteristicVm;
            Assert.NotNull(characteristicVm);
            Assert.Equal("Characteristic J", characteristicVm?.Name);
            Assert.Equal(0, characteristicVm?.Decimals);
        }

        [Fact]
        public async Task UpdateCharacteristic_ValidData_Sucess()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicVm
            {
                Id = 1,
                Name = "Characteristic A Updated",
                MeaTypeId = 2,
                ProcessId = 1,
                DataType = 1,
                Unit = "Unit A Updated",
                DefectRateLimit = 10,
                EmailEventModel = 0,
                Decimals = 3
            };
            // Act
            var result = await controller.UpdateCharacteristic(1, request);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var characteristicVm = okResult?.Value as CharacteristicVm;
            Assert.NotNull(characteristicVm);
            Assert.Equal("Characteristic A Updated", characteristicVm?.Name);
        }

        [Fact]
        public async Task UpdateCharacteristic_NotFound_Failure()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicVm
            {
                Id = 999,
                Name = "Characteristic X",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 0,
                Unit = "Unit X",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.UpdateCharacteristic(999, request);
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("Characteristic not found.", notFoundResult?.Value);
        }

        [Fact]
        public async Task UpdateCharacteristic_DuplicateCharacteristicName_Failure()
        {
            var controller = new CharacteristicsController(_context);
            var request = new CharacteristicVm
            {
                Id = 1,
                Name = "Characteristic B",
                MeaTypeId = 1,
                ProcessId = 1,
                DataType = 0,
                Unit = "Unit A",
                DefectRateLimit = 5,
                EmailEventModel = 1,
                Decimals = 2
            };
            // Act
            var result = await controller.UpdateCharacteristic(1, request);
            // Assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Characteristic with the same name already exists in this process.", badRequestResult?.Value);
        }

        [Fact]
        public async Task DeleteCharacteristic_ValidId_Success()
        {
            var controller = new CharacteristicsController(_context);
            // Act
            var result = await controller.DeleteCharacteristic(1);
            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var characteristic = okResult?.Value as CharacteristicVm;
            Assert.NotNull(characteristic);
            Assert.Equal("Characteristic A", characteristic?.Name);
        }

        [Fact]
        public async Task DeleteCharacteristic_NotFound_Failure()
        {
            var controller = new CharacteristicsController(_context);
            // Act
            var result = await controller.DeleteCharacteristic(999);
            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult?.StatusCode);
        }
    }
}
