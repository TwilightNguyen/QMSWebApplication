using AspNetCoreGeneratedDocument;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels.System.MeasData;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class SamplesController(ApplicationDbContext context) : ControllerBase
    {
        public readonly ApplicationDbContext _context = context;

        /// <summary>
        /// Url: /api/measdatas/
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CreateMeasData([FromBody] MeasDataCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var production = await _context.ProductionPlans
                .FirstOrDefaultAsync(x => x.Id == request.ProductionId && x.Enabled == true);
            
            if(production == null)
            {
                return BadRequest("Invalid Production.");
            }

            if(request.Values.Any(x => x.CharacteristicId < 0))
            {
                return BadRequest("Invalid Characteristic.");
            }


            var planType = await _context.InspPlanTypes
                .FirstOrDefaultAsync(x => x.Id == request.PlanTypeId);

            if (planType == null) {
                return BadRequest("Invalid Plan Type.");
            }

            var productionDatas = from j in _context.JobDatas.Where(x => x.Id == production.JobId && x.Enabled == true)
                          join m in _context.Products.Where(x => x.Enabled == true) on j.ProductId equals m.Id
                          join i in _context.InspectionPlans.Where(x => x.Enabled == true) on m.InspPlanId equals i.Id
                          join iSub in _context.InspectionPlanSubs.Where(x => x.Enabled == true && x.PlanTypeId == request.PlanTypeId && x.PlanState == 1) on i.Id equals iSub.InspPlanId
                          join iDe in _context.InspectionPlanDatas.Where(x => x.Enabled == true) on iSub.Id equals iDe.InspPlanSubId
                          join c in _context.Characteristics on iDe.CharacteristicId equals c.Id
                          into grouping
                          from inventory in grouping.DefaultIfEmpty()
                          select new
                          {
                            iDe.CharacteristicId,
                            Mold = m.MoldQuantity,
                            Cavity = m.CavityQuantity,
                            iDe.USL,
                            iDe.LSL,
                          };

            if (productionDatas == null || !productionDatas.Any()) {
                return NotFound("No Production Data found.");
            }

            if (request.MoldId <= 0 || request.MoldId > productionDatas.ToList()[0].Mold)
            {
                return BadRequest("Invalid Mold.");
            }

            if (request.CavityId <= 0 || request.CavityId > productionDatas.ToList()[0].Cavity)
            {
                return BadRequest("Invalid Cavity.");
            }

            // Typical claim types (depends on your token/issuer)
            var userId = User?.FindFirst("sub")?.Value
                     ?? User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var iuser = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            int lastSampleIndex = await _context.MeasDatas
                .Where(x => x.ProductionId == request.ProductionId)
                .Select(x => x.SampleIndex)
                .MaxAsync() ?? 0;

            List<Samples> NewMeasDatas = [];

            var NewMeasData = new Samples
            {
                LineId = production.LineId,
                DataCollection = 0,
                JobId = production.JobId,
                OutputQuantity = 0,
                Notes = request.Notes,
                SampleQuantity = request.SampleQunality,
                ProductionId = request.ProductionId,
                PlanTypeId = request.PlanTypeId,
                MoldId = request.MoldId,
                CavityId = request.CavityId,
                UserId = iuser?.IntUserID ?? -1,
                SampleIndex = lastSampleIndex + 1,
            };

            DateTimeOffset now = DateTimeOffset.Now;
            foreach(var item in request.Values)
            {
                NewMeasData.CharacteristicId = item.CharacteristicId;
                NewMeasData.MeasuredDateTime = now;

                var productionData = await productionDatas
                    .Where(x => x.CharacteristicId == item.CharacteristicId)
                    .FirstOrDefaultAsync();

                if (productionData == null) {
                    return NotFound("Not Found Characteristic In Production.");
                }
                
                int count = 0;
                foreach(var value in item.CharacteristicValue)
                {
                    NewMeasData.CharacteristicValue = value.ToString();
                    NewMeasData.MeasuredDateTime = now.AddSeconds(count);
                    NewMeasData.UploadedDateTime = NewMeasData.MeasuredDateTime;
                    if(value < productionData.LSL || value > productionData.USL)
                    {
                        NewMeasData.Status = 1;
                        NewMeasData.EmailSent = 0;
                    }

                    count++;
                    NewMeasDatas.Add(NewMeasData);
                }
            }

            _context.MeasDatas.AddRange(NewMeasDatas);
            var result = await _context.SaveChangesAsync();

            if(result > 0)
            {
                return CreatedAtAction("AddMeasData", lastSampleIndex + 1);
            }
            else
            {
                return BadRequest("Failed to add Measurement Data.");
            }
        }

        ///<summary>
        /// Url: /api/measdatas/GetProductionIdAndPlanTypeIdAndMoldIdAndCavityId
        /// </summary>
        /// 
        [HttpGet("/{ProductionId:int}/{CharacteristicId:int}/{PlanTypeId:int}/{MoldId:int}/{CavityId:int}")]
        public async Task<IActionResult> GetProductionIdAndCharacteristicIdAndPlanTypeIdAndMoldIdAndCavityId(
            int ProductionId,
            int CharacteristicId,
            int PlanTypeId,
            int MoldId,
            int CavityId
        )
        {
            if(ProductionId <= 0)
            {
                return BadRequest("Invalid Production Id.");
            }

            if (CharacteristicId <= 0)
            {
                return BadRequest("Invalid Characteristic Id.");
            }

            if (PlanTypeId <= 0) {
                return BadRequest("Invalid Plan Type Id.");
            }

            if (MoldId <= -2 || MoldId == 0)
            {
                return BadRequest("Invalid Mold Id.");
            }

            if (CavityId <= -2 || CavityId == 0) {
                return BadRequest("Invalid Cavity Id.");
            }

            var production = await _context.ProductionPlans
                .FirstOrDefaultAsync(x => x.Id == ProductionId && x.Enabled == true);

            if (production == null) {
                return NotFound("Production Data Id not found.");
            }

            var planType = await _context.InspPlanTypes
                .FirstOrDefaultAsync(x => x.Id == PlanTypeId);

            if (planType == null) {
                return NotFound("Plan Type not found.");
            }

            var productionDatas = from j in _context.JobDatas.Where(x => x.Id == production.JobId && x.Enabled == true)
                                  join m in _context.Products.Where(x => x.Enabled == true) on j.ProductId equals m.Id
                                  join i in _context.InspectionPlans.Where(x => x.Enabled == true) on m.InspPlanId equals i.Id
                                  join iSub in _context.InspectionPlanSubs.Where(x => x.Enabled == true && x.PlanTypeId == PlanTypeId && x.PlanState == 1) on i.Id equals iSub.InspPlanId
                                  join iDe in _context.InspectionPlanDatas.Where(x => x.Enabled == true && x.CharacteristicId == CharacteristicId) on iSub.Id equals iDe.InspPlanSubId
                                  join c in _context.Characteristics on iDe.CharacteristicId equals c.Id
                                  into grouping
                                  from inventory in grouping.DefaultIfEmpty()
                                  select new
                                  {
                                        iDe.CharacteristicId,
                                        Mold = m.MoldQuantity,
                                        Cavity = m.CavityQuantity,
                                        iDe.USL,
                                        iDe.LSL,
                                  };

            if(productionDatas == null || !productionDatas.Any())
            {
                return NotFound("Not Found Characteristic in Production.");
            }
            var productionData = await productionDatas.FirstOrDefaultAsync();

            if (MoldId > 0 && MoldId > productionData?.Mold)
            {
                return NotFound("Mold not found.");
            }

            if (CavityId > 0 && CavityId > productionData?.Cavity)
            {
                return NotFound("Cavity not found.");
            }

            var measDatas = _context.MeasDatas
                .Where(x => x.ProductionId == ProductionId && 
                    x.CharacteristicId == CharacteristicId && 
                    x.PlanTypeId == PlanTypeId && 
                    x.MoldId == MoldId && 
                    x.CavityId == CavityId);

            if (measDatas == null || !measDatas.Any()) {
                return NotFound("No Measurement data found.");
            }

            var measDataVms = measDatas.Select(measData => new MeasDataVm
            {
                Id = measData.Id,
                ProductionId = measData.ProductionId,
                CharacteristicId = measData.CharacteristicId,
                PlanTypeId = measData.PlanTypeId,
                MoldId = measData.MoldId,
                CavityId = measData.CavityId,
                LineId = measData.LineId,
                JobId = measData.JobId,
                CharacteristicRange = measData.CharacteristicRange,
                CharacteristicValue = measData.CharacteristicValue,
                DataCollection = measData.DataCollection,
                EmailSent = measData.EmailSent,
                Status = measData.Status,
                Notes = measData.Notes,
                OutputQuanlity = measData.OutputQuantity,
                SampleIndex = measData.SampleIndex,
                SampleQuanlity = measData.SampleQuantity,
                MeasuredDateTime = measData.MeasuredDateTime,
                UploadedDateTime = measData.UploadedDateTime,
                UserId = measData.UserId,
            });

            return Ok(measDataVms);
        }

        ///<summary>
        ///Url: /api/measdatas
        /// </summary>
        /// 

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteMeasData(int Id)
        {
            if(Id <= 0)
            {
                return BadRequest("Invalid measurement data.");
            }

            var measData = await _context.MeasDatas.FirstOrDefaultAsync(x => x.Id == Id);

            if (measData == null) {
                return BadRequest("Measurement data not found.");
            }

            _context.MeasDatas.Remove(measData);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0) {
                return Ok(new MeasDataVm
                {
                    Id = measData.Id,
                    ProductionId = measData.ProductionId,
                    CharacteristicId = measData.CharacteristicId,
                    PlanTypeId = measData.PlanTypeId,
                    MoldId = measData.MoldId,
                    CavityId = measData.CavityId,
                    LineId = measData.LineId,
                    JobId = measData.JobId,
                    CharacteristicRange = measData.CharacteristicRange,
                    CharacteristicValue = measData.CharacteristicValue,
                    DataCollection = measData.DataCollection,
                    EmailSent = measData.EmailSent,
                    Status = measData.Status,
                    Notes = measData.Notes,
                    OutputQuanlity = measData.OutputQuantity,
                    SampleIndex = measData.SampleIndex,
                    SampleQuanlity = measData.SampleQuantity,
                    MeasuredDateTime = measData.MeasuredDateTime,
                    UploadedDateTime = measData.UploadedDateTime,
                    UserId = measData.UserId,
                });
            }
            else
            {
                return BadRequest("Failed to delete Measurement Data.");
            }
        }
    }
}
