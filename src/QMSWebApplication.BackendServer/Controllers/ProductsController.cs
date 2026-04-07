using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.Product;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class ProductsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Url: /api/products
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (String.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Product name cannot be empty.");
            }

            var area = await _context.ProductionAreas.FirstOrDefaultAsync(a => a.Id == request.AreaId);

            if (area == null) {
                return BadRequest("Invalid Production Area.");
            }

            var inspPlan = await _context.InspectionPlans
                .FirstOrDefaultAsync(i => i.AreaId == request.AreaId && 
                i.Id == request.InspPlanId && 
                i.Enabled == true);

            if (inspPlan == null)
            {
                return BadRequest("Invalid Inspection Plan.");
            }

            var existsPro = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == request.Name && 
                p.AreaId == request.AreaId &&
                p.Enabled == true);

            if (existsPro != null) {
                return BadRequest("Product with the same name already exists in this Production Area.");
            }

            var product = new Products
            {
                AreaId = request.AreaId,
                InspPlanId = request.InspPlanId,
                Name = request.Name,
                ModelInternal = request.ModelInternal,
                MoldQuantity = request.MoldQuanlity,
                CavityQuantity = request.CavityQuanlity,
                Description = request.Description,
                Notes = request.Notes,
                CustomerName = request.CustomerName,
            };

            _context.Products.Add(product); 

            var result = await _context.SaveChangesAsync();

            if (result > 0) {
                return CreatedAtAction(
                    nameof(GetById),
                    new { Id =  request.InspPlanId},
                    new ProductVm
                    {
                        Id = product.Id,
                        AreaId = product.AreaId,
                        Name = product.Name,
                        Notes = product.Notes,
                        Description = product.Description,
                        InspPlanId = product.InspPlanId,
                        ModelInternal = product.ModelInternal,
                        CustomerName = product.CustomerName,
                        MoldQuanlity = product.MoldQuantity,
                        CavityQuanlity = product.CavityQuantity,
                    });
            }
            else
            {
                return BadRequest("Failed to create Product.");
            }

        }

        /// <summary>
        /// Url: /api/products/
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = _context.Products.Where(r => r.Enabled == true).ToList();

            if (products == null) return NotFound("No products found.");

            var productVms = products.Select(product => new ProductVm
            {
                Id = product.Id,
                Name = product.Name,
                AreaId = product.AreaId,
                InspPlanId = product.InspPlanId,
                ModelInternal = product.ModelInternal,
                CustomerName = product.CustomerName,
                Notes = product.Notes,
                MoldQuanlity = product.MoldQuantity,
                CavityQuanlity = product.CavityQuantity,
                Description = product.Description,
            });

            return Ok(productVms);
        }

        /// <summary
        /// Url: /api/product/GetById
        /// </summary>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == Id && p.Enabled == true);

            if (product == null) return NotFound("Product not found.");

            var productVm = new ProductVm
            {
                Id = product.Id,
                Name = product.Name,
                AreaId = product.AreaId,
                InspPlanId = product.InspPlanId,
                ModelInternal = product.ModelInternal,
                CustomerName = product.CustomerName,
                Notes = product.Notes,
                Description = product.Description,
                MoldQuanlity = product.MoldQuantity,
                CavityQuanlity = product.CavityQuantity,
            };

            return Ok(productVm);
        }

        /// <summary>
        /// Url: /api/products/GetByAreaId
        /// </summary>
        /// 
        [HttpGet("GetByAreaId/{AreaId:int}")]
        public async Task<IActionResult> GetByAreaId(int AreaId) {
            var products = _context.Products.Where(x => x.Enabled == true && x.AreaId == AreaId);
            if (products == null || !products.Any()) return NotFound("No product found for the specified production area.");

            var productVms = products?.Select(product => new ProductVm
            {
                Id = product.Id,
                Name = product.Name,
                AreaId = product.AreaId,
                InspPlanId = product.InspPlanId,
                CustomerName = product.CustomerName,
                Notes = product.Notes,
                Description = product.Description,
                ModelInternal = product.ModelInternal,
                MoldQuanlity = product.MoldQuantity,
                CavityQuanlity = product.CavityQuantity,
            });

            return Ok(productVms);
        }

        /// <summary>
        ///  Url: /api/products/pagging/
        /// </summary>
        /// 
        [HttpGet("GetPaging")]
        public async Task<IActionResult> GetPaging(string? filter, int pageIndex, int pageSize) {
            var query = _context.Products.Where(p => p.Enabled == true).AsQueryable();

            if(!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.Name!.Contains(filter));
            }

            List<ProductVm> items = [..query.Skip((pageIndex-1) * pageSize)
                .Take(pageSize)
                .Select(product => new ProductVm{
                    Id = product.Id,
                    Name = product.Name,
                    AreaId = product.AreaId,
                    InspPlanId= product.InspPlanId,
                    ModelInternal = product.ModelInternal,
                    CustomerName = product.CustomerName,
                    Notes = product.Notes,
                    Description = product.Description,
                    MoldQuanlity = product.MoldQuantity,
                    CavityQuanlity = product.CavityQuantity,
                })];

            var pagination = new Pagination<ProductVm>{ 
                Items = items,
                TotalRecords = query.Count()
            };

            return Ok(pagination);
        }

        /// <summary>
        /// Url:/api/products
        /// </summary>
        /// 
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int Id, ProductVm productVm) {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState);
            }

            if (Id < 0 || Id != productVm.Id)
            {
                return BadRequest("Invalid product Id.");
            }

            var product = _context.Products.FirstOrDefault(x => x.Id == Id && x.Enabled == true);

            if (product == null) { 
                return NotFound("Product not found.");
            }

            if (string.IsNullOrEmpty(productVm.Name)) {
                return BadRequest("Product Name cannot be empty.");
            }

            var productExists = _context.Products.FirstOrDefault(x => 
                x.Name == productVm.Name && 
                x.AreaId == product.AreaId &&
                x.Enabled == true
            );

            if (productExists != null) {
                return BadRequest("Product with the same name already exists in production area.");
            }

            product.Name = productVm.Name;
            product.Description = productVm.Description;
            product.ModelInternal = productVm.ModelInternal;
            product.Notes = productVm.Notes;
            product.CustomerName = productVm.CustomerName;
            product.MoldQuantity = productVm.MoldQuanlity;
            product.CavityQuantity = productVm.CavityQuanlity;
            
            _context.Products.Update(product);
            
            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return Ok(new ProductVm
                {
                    Id = product.Id,
                    Name = product.Name,
                    AreaId = product.AreaId,
                    InspPlanId = product.InspPlanId,
                    ModelInternal = product.ModelInternal,
                    MoldQuanlity = product.MoldQuantity,
                    CavityQuanlity = product.CavityQuantity,
                    Notes = product.Notes,
                    CustomerName = product.CustomerName,
                    Description = product.Description,
                });
            }
            else
            {
                return BadRequest("Failed to update product.");
            }
        }

        /// <summary>
        /// Url: /api/products
        /// </summary>
        /// 
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteProduct(int Id) { 
            var product = _context.Products.FirstOrDefault(x => x.Id == Id && x.Enabled == true);

            if(product == null)
            {
                return NotFound("Product not found.");
            }

            product.Enabled = false;

            _context.Products.Update(product);
            var result = await _context.SaveChangesAsync();

            if (result > 0) {
                return Ok(new ProductVm
                {
                    Id = product.Id,
                    Name = product.Name,
                    AreaId = product.AreaId,
                    InspPlanId= product.InspPlanId,
                    ModelInternal = product.ModelInternal,
                    MoldQuanlity = product.MoldQuantity,
                    CavityQuanlity = product.CavityQuantity,
                    Notes = product.Notes,
                    CustomerName = product.CustomerName,
                    Description = product.Description,
                });
            }
            else
            {
                return BadRequest("Failed to delete product.");
            }
        }
    }
}
