using Microsoft.AspNetCore.Mvc;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels.System.Function;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class FunctionsControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;

        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedFunctions(_context);
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
            var controller = new FunctionsController(_context);

            /// assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_ValidData_Success()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.GetAll();
            /// assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var functions = okResult?.Value as IEnumerable<FunctionVm>;
            Assert.NotNull(functions);
            Assert.Equal(3, functions.Count()); // Assuming 3 functions are seeded in InMemoryDbContext.SeedFunctions
        }

        [Fact]
        public async Task GetById_ExistingId_ReturnsFunction()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.GetById(1); // Assuming ID 1 exists in seeded data
            /// assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var func = okResult?.Value as FunctionVm;
            Assert.NotNull(func);
            Assert.Equal(1, func.Id);
        }

        [Fact]
        public async Task GetById_NonExistingId_ReturnsNotFound()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.GetById(999); // Assuming ID 999 does not exist
            /// assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal($"Function with Id {999} not found.", notFoundResult?.Value);
        }

        [Fact]
        public async Task CreateFunction_ValidData_Success()
        {
            var controller = new FunctionsController(_context);

            /// act
            var result = await controller.CreateFunction(new FunctionCreateRequest
            {
                Name = "Test Function",
                Url = "/api/tvdisplays",
                ParentId = -1
            });

            /// assert
            Assert.NotNull(result);
            var okResult = result as CreatedAtActionResult;
            Assert.NotNull(okResult);
            var func = okResult?.Value as FunctionVm;
            Assert.NotNull(func);
            Assert.Equal("Test Function", func.Name);
            Assert.Equal("/api/tvdisplays", func.Url);
            Assert.Equal(-1, func.ParentId);
        }

        [Fact]
        public async Task CreateFunction_DuplicateName_ReturnsBadRequest()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.CreateFunction(new FunctionCreateRequest
            {
                Name = "Function 1", // Assuming "Dashboard" already exists in seeded data
                Url = "/api/dashboard",
                ParentId = -1
            });
            /// assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Function with the same name already exists.", badRequestResult?.Value);
        }

        [Fact]
        public async Task CreateFunction_InvalidParentId_ReturnsBadRequest()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.CreateFunction(new FunctionCreateRequest
            {
                Name = "Test Function with Invalid Parent",
                Url = "/api/test",
                ParentId = 999 // Assuming this ID does not exist
            });
            /// assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Parent function not found.", badRequestResult?.Value);
        }

        [Fact]
        public async Task CreateFunction_MissingName_ReturnsBadRequest()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.CreateFunction(new FunctionCreateRequest
            {
                Name = "", // Missing name
                Url = "/api/test",
                ParentId = -1
            });
            /// assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Function name is required.", badRequestResult?.Value);
        }

        [Fact]
        public async Task Updatefunction_ValidData_Success()
        {
            var controller = new FunctionsController(_context);

            /// act
            var result = await controller.UpdateFunction(2, new FunctionCreateRequest
            {
                Name = "Updated Function 2",
                Url = "/api/updated",
                ParentId = -1
            });

            /// assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var func = okResult?.Value as FunctionVm;
            Assert.NotNull(func);
            Assert.Equal(2, func.Id);
            Assert.Equal("Updated Function 2", func.Name);
        }



        [Fact]
        public async Task UpdateFunction_DuplicateName_ReturnsBadRequest()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.UpdateFunction(2, new FunctionCreateRequest
            {
                Name = "Function 1", // Assuming "Dashboard" already exists in seeded data
                Url = "/api/dashboard",
                ParentId = -1
            });
            /// assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Function with the same name already exists.", badRequestResult?.Value);
        }

        [Fact]
        public async Task UpdateFunction_InvalidParentId_ReturnsBadRequest()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.UpdateFunction(2, new FunctionCreateRequest
            {
                Name = "Test Function with Invalid Parent",
                Url = "/api/test",
                ParentId = 999 // Assuming this ID does not exist
            });
            /// assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Parent function not found.", badRequestResult?.Value);
        }

        [Fact]
        public async Task UpdateFunction_MissingName_ReturnsBadRequest()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.UpdateFunction(2, new FunctionCreateRequest
            {
                Name = "", // Missing name
                Url = "/api/test",
                ParentId = -1
            });
            /// assert
            Assert.NotNull(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("Function name is required.", badRequestResult?.Value);
        }

        [Fact]
        public async Task DeleteFunction_ValidData_Success()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.DeleteFunction(3); // Assuming ID 3 exists in seeded data
            /// assert
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var func = okResult?.Value as FunctionVm;
            Assert.NotNull(func);
            Assert.Equal(3, func.Id);
        }

        [Fact]
        public async Task DeleteFunction_NonExistingId_ReturnsNotFound()
        {
            var controller = new FunctionsController(_context);
            /// act
            var result = await controller.DeleteFunction(999); // Assuming ID 999 does not exist
            /// assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal($"Function with Id {999} not found.", notFoundResult?.Value);
        }
    }
}
