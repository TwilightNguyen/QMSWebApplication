using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.Command;
using QMSWebApplication.ViewModels.System.Function;
using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class CommandsControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;

        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedCommands(_context);
            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            try { await _context.Database.EnsureDeletedAsync(); } catch { }
            await _context.DisposeAsync();
        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            /// act
            var controller = new CommandsController(_context);

            /// assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_ValidData_Success()
        {
            var controller = new CommandsController(_context);
            /// act
            var result = await controller.GetAll();
            /// assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var commands = okResult?.Value as IEnumerable<CommandVm>;
            Assert.NotNull(commands);
            Assert.Equal(3, commands.Count()); // Assuming 3 commands are seeded in InMemoryDbContext.SeedCommands
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsCommand()
        {
            var controller = new CommandsController(_context);
            /// act
            var result = await controller.GetById(1); // Assuming ID 1 exists in seeded data
            /// assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var command = okResult?.Value as CommandVm;
            Assert.NotNull(command);
            Assert.Equal(1, command.Id);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            var controller = new CommandsController(_context);
            /// act
            var result = await controller.GetById(999); // Assuming ID 999 does not exist
            /// assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal($"Command with Id {999} not found.", notFoundResult?.Value);
        }

        [Fact]
        public async Task CreateCommand_ValidData_Success()
        {
            var controller = new CommandsController(_context);

            /// act
            var result = await controller.CreateCommand(new CommandCreateRequest
            {
                Name = "Test Command",
                Notes = "Test Command Notes",
            });

            /// assert
            Assert.NotNull(result);
            var okResult = result as CreatedAtActionResult;
            Assert.NotNull(okResult);
            var command = okResult?.Value as CommandVm;
            Assert.NotNull(command);
            Assert.Equal("Test Command", command.Name);
            Assert.Equal("Test Command Notes", command.Notes);
        }

        [Fact]
        public async Task CreateCommand_DuplicateName_ReturnsBadRequest()
        {
            var controller = new CommandsController(_context);
            /// act
            var result = await controller.CreateCommand(new CommandCreateRequest
            {
                Name = "Command 1", // Assuming "Command 1" already exists in seeded data
                Notes = "Notes 1"
            });
            /// assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Command with the same name already exists.", badRequestResult?.Value);
        }


        [Fact]
        public async Task CreateCommand_MissingName_ReturnsBadRequest()
        {
            var controller = new CommandsController(_context);
            /// act
            var result = await controller.CreateCommand(new CommandCreateRequest
            {
                Name = "", // Missing name
                Notes = "Test Command Notes"
            });
            /// assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Command name is required.", badRequestResult?.Value);
        }

        [Fact]
        public async Task UpdateCommand_ValidData_Success()
        {
            var controller = new CommandsController(_context);

            /// act
            var result = await controller.UpdateCommand(2, new CommandCreateRequest
            {
                Name = "Updated Command 2",
                Notes = "Updated Command Notes"
            });

            /// assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var command = okResult?.Value as CommandVm;
            Assert.NotNull(command);
            Assert.Equal(2, command.Id);
            Assert.Equal("Updated Command 2", command.Name);
        }



        [Fact]
        public async Task UpdateCommand_DuplicateName_ReturnsBadRequest()
        {
            var controller = new CommandsController(_context);
            /// act
            var result = await controller.UpdateCommand(2, new CommandCreateRequest
            {
                Name = "Command 1", // Assuming "Command 1" already exists in seeded data
                Notes = "Notes 1"
            });
            /// assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Command with the same name already exists.", badRequestResult?.Value);
        }

        [Fact]
        public async Task UpdateCommand_MissingName_ReturnsBadRequest()
        {
            var controller = new CommandsController(_context);
            /// act
            var result = await controller.UpdateCommand(2, new CommandCreateRequest
            {
                Name = "", // Missing name
                Notes = "Test Command Notes"
            });
            /// assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Command name is required.", badRequestResult?.Value);
        }

        [Fact]
        public async Task DeleteCommand_ValidData_Success()
        {
            var controller = new CommandsController(_context);
            /// act
            var result = await controller.DeleteCommand(3); // Assuming ID 3 exists in seeded data
            /// assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var command = okResult?.Value as CommandVm;
            Assert.NotNull(command);
            Assert.Equal(3, command.Id);
        }

        [Fact]
        public async Task DeleteCommand_NonExistingId_ReturnsNotFound()
        {
            var controller = new CommandsController(_context);
            /// act
            var result = await controller.DeleteCommand(999); // Assuming ID 999 does not exist
            /// assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal($"Command with Id {999} not found.", notFoundResult?.Value);
        }
    }
}
