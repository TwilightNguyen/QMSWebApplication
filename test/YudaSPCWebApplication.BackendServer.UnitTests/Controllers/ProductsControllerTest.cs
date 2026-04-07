using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.Product;
using static Azure.Core.HttpHeader;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class ProductsControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;

        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();
            InMemoryDbContext.SeedProductionAreas(_context);
            InMemoryDbContext.SeedRoles(_context);
            InMemoryDbContext.SeedUsers(_context);
            InMemoryDbContext.SeedProcesses(_context);
            InMemoryDbContext.SeedProcessLines(_context);
            InMemoryDbContext.SeedMeasureTypes(_context);
            InMemoryDbContext.SeedCharacteristics(_context);
            InMemoryDbContext.SeedEventLogs(_context);
            InMemoryDbContext.SeedInspPlanTypes(_context);
            InMemoryDbContext.SeedInspectionPlans(_context);
            InMemoryDbContext.SeedInspectionPlanSubs(_context);
            InMemoryDbContext.SeedInspectionPlanDatas(_context);
            InMemoryDbContext.SeedProducts(_context);
        }

        public async Task DisposeAsync()
        {
            try { await _context.Database.EnsureCreatedAsync(); } catch { }
            await _context.DisposeAsync();
        }

        [Fact]
        public void ShouldCreateInstance_NotNull_Success()
        {
            // Act
            var controller = new ProductsController(_context);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_HasData_Success()
        {
            var controllers = new ProductsController(_context);

            // Act
            var result = await controllers.GetAll();

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var productList = okResult?.Value as IEnumerable<ProductVm>;
            Assert.Equal(4, productList?.Count());
            Assert.Contains(productList!, p => p.Name == "Product 01");
            Assert.Contains(productList!, p => p.Name == "Product 02");
            Assert.Contains(productList!, p => p.Name == "Product 03");
            Assert.Contains(productList!, p => p.Name == "Product 04");
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            var controller = new ProductsController(_context);

            // Act 
            var result = await controller.GetById(1);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var product = okResult?.Value as ProductVm;
            Assert.Equal(1, product?.Id);
            Assert.Equal("Product 01", product?.Name);
        }

        [Fact]
        public async Task GetById_NotFound_ReturnNotFound()
        {
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.GetById(999);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal("Product not found.", notFoundResult?.Value);
            Assert.Equal(404, notFoundResult?.StatusCode);
        }

        [Fact]
        public async Task GetByAreaId_HasData_Success()
        {
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.GetByAreaId(1);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var productList = okResult?.Value as IEnumerable<ProductVm>;
            Assert.Equal(2, productList?.Count());
            Assert.All(productList!, p => Assert.Equal(1, p.AreaId));
        }

        [Fact]
        public async Task GetByAreaId_NotFound_ReturnNotFound()
        {
            var controller = new ProductsController(_context);
            // Act
            var result = await controller.GetByAreaId(999);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal("No product found for the specified production area.", notFoundResult?.Value);
            Assert.Equal(404, notFoundResult?.StatusCode);

        }

        [Fact]
        public async Task GetPaging_NoFilter_Success()
        {
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.GetPaging(null, 1, 2);

            // Assert 
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var pagedResult = okResult?.Value as Pagination<ProductVm>;
            Assert.NotNull(pagedResult);
            Assert.Equal(4, pagedResult!.TotalRecords);
            Assert.Equal(2, pagedResult.Items.Count);
        }

        [Fact] 
        public async Task GetPaging_WithFilter_Success()
        {
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.GetPaging("Product 01", 1, 2);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var pagedResult = okResult?.Value as Pagination<ProductVm>;
            Assert.NotNull(pagedResult);
            Assert.Equal(1, pagedResult!.TotalRecords);
            Assert.Single(pagedResult.Items);
            Assert.Equal("Product 01", pagedResult.Items.First().Name);
        }

        [Fact]
        public async Task CreateProduct_ValidData_Success()
        {
            var controller = new ProductsController(_context);

            // Act
            var newProduct = new ProductCreateRequest
            {
                AreaId = 1,
                InspPlanId = 1,
                Name = "Product New 01",
                ModelInternal = "Model New 01",
                MoldQuanlity = 2,
                CavityQuanlity = 4,
                Notes = "Note New 01",
                Description = "Description New 01",
                CustomerName = "Customer name New 01"
            };

            var result = await controller.CreateProduct(newProduct);

            // Assert
            Assert.NotNull(result);
            var createResult = result as CreatedAtActionResult;
            var product = createResult?.Value as ProductVm;

            Assert.NotNull(product);
            Assert.Equal( 1, product.AreaId );
            Assert.Equal( 1,product.InspPlanId);
            Assert.Equal("Product New 01", product.Name);
            Assert.Equal("Model New 01", product.ModelInternal);
            Assert.Equal(2, product.MoldQuanlity);
            Assert.Equal(4, product.CavityQuanlity);
            Assert.Equal("Note New 01", product.Notes);
            Assert.Equal("Description New 01", product.Description);
            Assert.Equal("Customer name New 01", product.CustomerName);
        }

        [Fact]
        public async Task CreateProduct_EmptyName_Failure()
        {
            var controller = new ProductsController(_context);

            var newProduct = new ProductCreateRequest
            {
                AreaId = 1,
                InspPlanId = 1,
                Name = string.Empty,
                ModelInternal = "Model New 02",
                MoldQuanlity = 2,
                CavityQuanlity = 4,
                Notes = "Note New 02",
                Description = "Description New 02",
                CustomerName = "Customer name New 02"
            };
            // Act 
            var result = await controller.CreateProduct(newProduct);

            // Assert 
            Assert.NotNull(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.Equal(400, badRequest?.StatusCode);
            Assert.Equal("Product name cannot be empty.", badRequest?.Value);
        }

        [Fact]
        public async Task CreateProduct_DuplicateName_Failure()
        {
            var controller = new ProductsController(_context);

            var newProduct = new ProductCreateRequest
            {
                AreaId = 2,
                InspPlanId = 3,
                Name = "Product 03",
                ModelInternal = "Model New 03",
                MoldQuanlity = 2,
                CavityQuanlity = 4,
                Notes = "Note New 03",
                Description = "Description New 03",
                CustomerName = "Customer name New 03"
            };

            // Act 
            var result = await controller.CreateProduct(newProduct);
            Assert.NotNull(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.Equal(400, badRequest?.StatusCode);
            Assert.Equal("Product with the same name already exists in this Production Area.", badRequest?.Value);
        }

        [Fact]
        public async Task UpdateProduct_ValidData_Success()
        {
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.UpdateProduct( 1, 
                new ProductVm { 
                    Id = 1, 
                    AreaId = 1,
                    InspPlanId = 1,
                    Name = "Product 01 Updated",
                    ModelInternal = "Model Internal 01 Updated",
                    MoldQuanlity = 3,
                    CavityQuanlity = 5,
                    Description = "Description 01 Updated",
                    Notes = "Notes 01 Updated",
                    CustomerName = "Customer Name 01 Updated",
                });

            // Assert
            Assert.NotNull(result);
            var okRequet = result as OkObjectResult;
            Assert.NotNull(okRequet);
            var product = okRequet?.Value as ProductVm;
            Assert.NotNull(product);
            Assert.Equal("Product 01 Updated", product?.Name);
            Assert.Equal("Model Internal 01 Updated", product?.ModelInternal); 
            Assert.Equal(3, product?.MoldQuanlity); 
            Assert.Equal(5, product?.CavityQuanlity);
            Assert.Equal("Description 01 Updated", product?.Description); 
            Assert.Equal("Notes 01 Updated", product?.Notes); 
            Assert.Equal("Customer Name 01 Updated", product?.CustomerName); 
        }

        [Fact]
        public async Task UpdateProduct_InvalidId_Failure() {
            var controller = new ProductsController(_context);

            // Fact 
            var result = await controller.UpdateProduct(1,
                new ProductVm {
                    Id = 2,
                    AreaId = 1,
                    InspPlanId = 1,
                    Name = "Product 02 Updated",
                    ModelInternal = "Model Internal 02 Updated",
                    MoldQuanlity = 3,
                    CavityQuanlity = 5,
                    Description = "Description 02 Updated",
                    Notes = "Notes 02 Updated",
                    CustomerName = "Customer Name 02 Updated",
                });


            // Assert
            Assert.NotNull(result);
            var badRequet = result as BadRequestObjectResult;
            Assert.NotNull(badRequet);
            Assert.Equal(400, badRequet?.StatusCode);
            Assert.Equal("Invalid product Id.", badRequet?.Value);
        }

        [Fact]
        public async Task UpdateProduct_NotFound_Failure() { 
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.UpdateProduct(999,
                new ProductVm
                {
                    Id = 999,
                    AreaId = 1,
                    InspPlanId = 1,
                    Name = "Product 999 Updated",
                    ModelInternal = "Model Internal 999 Updated",
                    MoldQuanlity = 3,
                    CavityQuanlity = 5,
                    Description = "Description 999 Updated",
                    Notes = "Notes 999 Updated",
                    CustomerName = "Customer Name 999 Updated",
                });

            // Asert
            Assert.NotNull(result);
            var notFoundRequet = result as NotFoundObjectResult;
            Assert.NotNull(notFoundRequet);
            Assert.Equal(404, notFoundRequet?.StatusCode);
            Assert.Equal("Product not found.", notFoundRequet?.Value);
        }

        [Fact]
        public async Task UpdateProduct_DuplicateName_Failure()
        {
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.UpdateProduct(1, 
                new ProductVm {
                    Id = 1,
                    AreaId = 1,
                    InspPlanId = 1,
                    Name = "Product 02",
                    ModelInternal = "Model Internal 02 Updated",
                    MoldQuanlity = 3,
                    CavityQuanlity = 5,
                    Description = "Description 02 Updated",
                    Notes = "Notes 02 Updated",
                    CustomerName = "Customer Name 02 Updated",
                });

            // Assert
            Assert.NotNull(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.Equal(400, badRequest?.StatusCode);
            Assert.Equal("Product with the same name already exists in production area.", badRequest?.Value);
        }

        [Fact]
        public async Task UpdateProduct_EmptyName_Failure() { 
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.UpdateProduct( 3,
                new ProductVm {
                    Id = 3,
                    AreaId = 1,
                    InspPlanId = 1,
                    Name = string.Empty,
                    ModelInternal = "Model Internal 03 Updated",
                    MoldQuanlity = 3,
                    CavityQuanlity = 5,
                    Description = "Description 03 Updated",
                    Notes = "Notes 03 Updated",
                    CustomerName = "Cusmomer Name 03 Updated",
                });

            // Assert
            Assert.NotNull(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.Equal(400, badRequest?.StatusCode);
            Assert.Equal("Product Name cannot be empty.", badRequest?.Value);
        }

        [Fact]
        public async Task DeletedProduct_ValidData_Success()
        {
            var controller = new ProductsController(_context);

            // Act
            var result = await controller.DeleteProduct(4);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var product = okResult?.Value as ProductVm;
            Assert.NotNull(okResult);
            Assert.NotNull(product);
            Assert.Equal(4, product?.Id);
            Assert.Equal(2, product?.AreaId);
            Assert.Equal(3, product?.InspPlanId);
            Assert.Equal("Product 04", product?.Name);
            Assert.Equal("Model Test 04", product?.ModelInternal);
            Assert.Equal(4, product?.MoldQuanlity);
            Assert.Equal(6, product?.CavityQuanlity);
            Assert.Equal("Description 04", product?.Description);
            Assert.Equal("Notes 04", product?.Notes);
            Assert.Equal("Customer 04", product?.CustomerName);
        }

        [Fact]
        public async Task DeleteProduct_EmptyName_Failure()
        {
            var controller = new ProductsController(_context);

            // Act 
            var result = await controller.DeleteProduct(999);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult?.StatusCode);
            Assert.Equal("Product not found.", notFoundResult?.Value);
        }
    }
}
