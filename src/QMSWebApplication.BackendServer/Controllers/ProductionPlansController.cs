using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System.ProductionPlan;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class ProductionPlansController(ApplicationDbContext context) : ControllerBase
    {
        public readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Url: /api/productionplans/
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateProductionPlan([FromBody] ProductionPlanCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var job = await _context.JobDatas.FirstOrDefaultAsync(j => j.Id == request.JobId && j.Enabled == true );
            if(job == null)
            {
                return BadRequest("Invalid Job.");
            }

            var line = await _context.ProcessLines.FirstOrDefaultAsync(l => l.Id == request.LineId);
            if(line == null)
            {
                return BadRequest("Invalid Process Line.");
            }

            var process = await _context.Processes.FirstOrDefaultAsync(p => p.Id == line.ProcessId);

            if (process == null || job.AreaId != process.AreaId) {
                return BadRequest("Job and Process Line are not in the same Production Area.");
            }

            // Typical claim types (depends on your token/issuer)
            var userId = User?.FindFirst("sub")?.Value
                     ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var iuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);


            // Nếu là DB quan hệ (SQL Server) thì mới tạo Transaction, In-memory sẽ bỏ qua
            using var transaction = _context.Database.IsRelational()
                ? await _context.Database.BeginTransactionAsync()
                : null;

            try
            {
                var productionRunning = await _context.ProductionPlans.FirstOrDefaultAsync(p => p.Id == request.LineId && p.EndTime == null && p.Enabled == true);
                DateTimeOffset now = DateTimeOffset.Now;
                if (productionRunning != null)
                {
                    productionRunning.EndTime = now;
                    _context.ProductionPlans.Update(productionRunning);
                }

                var production = new ProductionPlans
                {
                    JobId = request.JobId,
                    LineId = request.LineId,
                    ProductionDate = request.ProductionDate,
                    StartTime = now,
                    PlannedQuantity = request.PlannedQuantity,
                    LotInform = request.LotInform,
                    MaterialInform = request.MaterialInform,
                    Notes = request.Notes,
                    EndTime = null,
                    Enabled = true,
                    UserId = iuser?.IntUserID ?? -1,
                    UploadedDateTime = now,
                    ModifiedDateTime = null,
                };

                _context.ProductionPlans.Add(production);

                var result = await _context.SaveChangesAsync();

                if(result > 0)
                {
                    // Chỉ Commit nếu có transaction
                    if (transaction != null) await transaction.CommitAsync();

                    return CreatedAtAction(
                        nameof(GetById),
                        new {production.Id},
                        new ProductionPlanVm
                        {
                            Id = production.Id,
                            JobId = production.JobId,
                            LineId = production.LineId,
                            ProductionDate = production.ProductionDate,
                            PlannedQuanlity = production.PlannedQuantity,
                            LotInform = production.LotInform,
                            MaterialInform = production.MaterialInform,
                            Notes = production.Notes,
                            UserId = production.UserId ?? -1,
                            StartTime = production.StartTime,
                            EndTime = production.EndTime,
                        }
                    );
                }
                
            }
            catch (Exception) {

            }

            // 3. Nếu có lỗi, hoàn tác mọi thay đổi đã thực hiện ở trên
            if (transaction != null) await transaction.RollbackAsync();
            return BadRequest("Failed to create Production.");
        }

        ///<summary>
        /// Url: /api/productionplans/
        /// </summary>
        /// 
        [HttpGet]
        public async Task <IActionResult> GetAll()
        {
            var productions = await _context.ProductionPlans.Where(p => p.Enabled == true).ToListAsync();

            if(productions == null || productions.Count == 0)
            {
                return NotFound("No Production found.");
            }

            var productionVms = productions.Select(production => new ProductionPlanVm
            {
                Id = production.Id,
                JobId = production.JobId,
                LineId = production.LineId,
                ProductionDate = production.StartTime,
                PlannedQuanlity = production.PlannedQuantity,
                LotInform = production.LotInform,
                MaterialInform = production.MaterialInform,
                Notes = production.Notes,
                StartTime = production.StartTime,
                EndTime = production.EndTime,
                UserId = production.UserId,
            });

            return Ok( productionVms );
        }

        ///<summary>
        /// Url: /api/productionplans/paging/
        /// </summary>
        /// 
        [HttpGet("Paging")]
        public async Task<IActionResult> GetPaging(int pageIndex, int pageSize)
        {
            var query = _context.ProductionPlans.Where(p => p.Enabled == true).AsQueryable();
            
            List<ProductionPlanVm> items = [..query.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(production => new ProductionPlanVm{
                    Id = production.Id,
                    JobId = production.JobId,
                    LineId = production.LineId,
                    ProductionDate = production.StartTime,
                    PlannedQuanlity = production.PlannedQuantity,
                    LotInform = production.LotInform,
                    MaterialInform = production.MaterialInform,
                    Notes = production.Notes,
                    StartTime = production.StartTime,
                    EndTime = production.EndTime,
                    UserId = production.UserId,
                })];

            var pagination = new Pagination<ProductionPlanVm>
            {
                Items = items,
                TotalRecords = query.Count(),
            };

            return Ok(pagination);
        }


        /// <summary>
        /// Url: /api/productionplans/{id}
        /// </summary>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var production = await _context.ProductionPlans.FirstOrDefaultAsync(p => p.Id == Id && p.Enabled == true);
            
            if(production == null)
            {
                return NotFound("Production not found.");
            }

            var productionVm = new ProductionPlanVm
            {
                Id = production.Id,
                JobId = production.JobId,
                LineId = production.LineId,
                ProductionDate = production.StartTime,
                PlannedQuanlity = production.PlannedQuantity,
                LotInform = production.LotInform,
                MaterialInform = production.MaterialInform,
                Notes = production.Notes,
                StartTime = production.StartTime,
                EndTime = production.EndTime,
                UserId = production.UserId,
            };

            return Ok(productionVm);
        }

        ///<summary>
        /// Url: /api/productionplans/
        /// </summary>
        /// 
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateProduction(int Id, ProductionPlanVm productionVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(Id < 0 || Id != productionVm.Id)
            {
                return BadRequest("Invalid Production Id.");
            }

            var production = await _context.ProductionPlans.FirstOrDefaultAsync(p => p.Id == Id && p.Enabled == true);

            if(production == null)
            {
                return NotFound("Production not found.");
            }

            production.LotInform = productionVm.LotInform;
            production.MaterialInform = productionVm.MaterialInform;
            production.Notes = productionVm.Notes;
            production.PlannedQuantity = productionVm.PlannedQuanlity;
            production.ProductionDate = productionVm.ProductionDate;

            _context.ProductionPlans.Update(production);

            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return Ok(new ProductionPlanVm
                {
                    Id = production.Id,
                    JobId = production.JobId,
                    LineId = production.LineId,
                    ProductionDate = production.ProductionDate,
                    PlannedQuanlity = production.PlannedQuantity,
                    LotInform = production.LotInform,
                    MaterialInform = production.MaterialInform,
                    Notes = production.Notes,
                    StartTime = production.StartTime,
                    EndTime = production.EndTime,
                    UserId = production.UserId,
                });
            }
            else
            {
                return BadRequest("Failed to update production.");
            }
        }

        ///<summary>
        /// Url: /api/productionplans/endproductionplan
        /// </summary>
        /// 
        [HttpPut("EndProduction/{Id:int}")]
        public async Task<IActionResult> EndProductionPlan(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id < 0)
            {
                return BadRequest("Invalid Production Id.");
            }

            var production = await _context.ProductionPlans.FirstOrDefaultAsync(p => p.Id == Id && p.Enabled == true);

            if (production == null)
            {
                return NotFound("Production not found.");
            }

            production.EndTime = DateTimeOffset.Now;

            _context.ProductionPlans.Update(production);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(new ProductionPlanVm
                {
                    Id = production.Id,
                    JobId = production.JobId,
                    LineId = production.LineId,
                    ProductionDate = production.StartTime,
                    PlannedQuanlity = production.PlannedQuantity,
                    LotInform = production.LotInform,
                    MaterialInform = production.MaterialInform,
                    Notes = production.Notes,
                    StartTime = production.StartTime,
                    EndTime = production.EndTime,
                    UserId = production.UserId,
                });
            }
            else
            {
                return BadRequest("Failed to end production.");
            }
        }

        ///<summary>
        /// Url: /api/productionplans/
        /// </summary>
        /// 
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteProductionPlan(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (Id < 0)
            {
                return BadRequest("Invalid Production Id.");
            }

            var production = await _context.ProductionPlans.FirstOrDefaultAsync(p => p.Id == Id && p.Enabled == true);

            if (production == null)
            {
                return NotFound("Production not found.");
            }

            production.Enabled = true;

            _context.ProductionPlans.Update(production);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return Ok(new ProductionPlanVm
                {
                    Id = production.Id,
                    JobId = production.JobId,
                    LineId = production.LineId,
                    ProductionDate = production.StartTime,
                    PlannedQuanlity = production.PlannedQuantity,
                    LotInform = production.LotInform,
                    MaterialInform = production.MaterialInform,
                    Notes = production.Notes,
                    StartTime = production.StartTime,
                    EndTime = production.EndTime,
                    UserId = production.UserId,
                });
            }
            else
            {
                return BadRequest("Failed to delete production.");
            }
        }
    }
}
