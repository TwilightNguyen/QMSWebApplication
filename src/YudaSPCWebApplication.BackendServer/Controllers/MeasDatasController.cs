using AspNetCoreGeneratedDocument;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YudaSPCWebApplication.BackendServer.Data;
using YudaSPCWebApplication.BackendServer.Data.Entities;
using YudaSPCWebApplication.ViewModels.System.MeasData;

namespace YudaSPCWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class MeasDatasController(ApplicationDbContext context) : ControllerBase
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

            var production = await _context.ProductionDatas
                .FirstOrDefaultAsync(x => x.IntID == request.ProductionId && x.BoolDeleted == false);
            
            if(production == null)
            {
                return BadRequest("Invalid Production.");
            }

            if(request.Values.Any(x => x.CharacteristicId < 0))
            {
                return BadRequest("Invalid Characteristic.");
            }


            var planType = await _context.InspPlanTypes
                .FirstOrDefaultAsync(x => x.IntID == request.PlanTypeId);

            if (planType == null) {
                return BadRequest("Invalid Plan Type.");
            }

            var productionDatas = from j in _context.JobDatas.Where(x => x.IntID == production.IntJobID && x.BoolDeleted == false)
                          join m in _context.Products.Where(x => x.BoolDeleted == false) on j.IntProductID equals m.IntID
                          join i in _context.InspectionPlans.Where(x => x.BoolDeleted == false) on m.IntInspPlanID equals i.IntID
                          join iSub in _context.InspectionPlanSubs.Where(x => x.BoolDeleted == false && x.IntPlanTypeID == request.PlanTypeId) on i.IntID equals iSub.IntInspPlanID
                          join iDe in _context.InspectionPlanDatas.Where(x => x.BoolDeleted == false && x.IntPlanState == 1) on iSub.IntID equals iDe.IntInspPlanSubID
                          join c in _context.Characteristics on iDe.IntCharacteristicID equals c.IntID
                          into grouping
                          from inventory in grouping.DefaultIfEmpty()
                          select new
                          {
                              CharacteristicId = iDe.IntCharacteristicID,
                              Mold = m.IntMoldQty,
                              Cavity = m.IntCavityQty,
                              USL = iDe.FtUSL,
                              LSL = iDe.FtLSL,
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
                .Where(x => x.IntProductionID == request.ProductionId)
                .Select(x => x.IntSampleIndex)
                .MaxAsync() ?? 0;

            List<MeasData3_01> NewMeasDatas = [];

            var NewMeasData = new MeasData3_01
            {
                IntLineID = production.IntLineID,
                IntDataCollection = 0,
                IntJobID = production.IntJobID,
                IntOutputQty = 0,
                StrOutputNotes = request.OutputNotes,
                IntSampleQty = request.SampleQty,
                IntProductionID = request.ProductionId,
                IntPlanTypeID = request.PlanTypeId,
                IntMoldID = request.MoldId,
                IntCavityID = request.CavityId,
                IntUserID = iuser?.IntUserID ?? -1,
                IntSampleIndex = lastSampleIndex + 1,
            };

            DateTime now = DateTime.Now;
            foreach(var item in request.Values)
            {
                NewMeasData.IntCharacteristicID = item.CharacteristicId;
                NewMeasData.DtTimeStamp = now;

                var productionData = await productionDatas
                    .Where(x => x.CharacteristicId == item.CharacteristicId)
                    .FirstOrDefaultAsync();

                if (productionData == null) {
                    return NotFound("Not Found Characteristic In Production.");
                }
                
                int count = 0;
                foreach(var value in item.CharacteristicValue)
                {
                    NewMeasData.VarCharacteristicValue = value.ToString();
                    NewMeasData.DtTimeMeasure = now.AddSeconds(count);
                    NewMeasData.DtTimeStamp = NewMeasData.DtTimeMeasure;
                    if(value < productionData.LSL || value > productionData.USL)
                    {
                        NewMeasData.IntOKNG = 1;
                        NewMeasData.IntEmailSent = 0;
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

            var production = await _context.ProductionDatas
                .FirstOrDefaultAsync(x => x.IntID == ProductionId && x.BoolDeleted == false);

            if (production == null) {
                return NotFound("Production Data Id not found.");
            }

            var planType = await _context.InspPlanTypes
                .FirstOrDefaultAsync(x => x.IntID == PlanTypeId);

            if (planType == null) {
                return NotFound("Plan Type not found.");
            }

            var productionDatas = from j in _context.JobDatas.Where(x => x.IntID == production.IntJobID && x.BoolDeleted == false)
                                  join m in _context.Products.Where(x => x.BoolDeleted == false) on j.IntProductID equals m.IntID
                                  join i in _context.InspectionPlans.Where(x => x.BoolDeleted == false) on m.IntInspPlanID equals i.IntID
                                  join iSub in _context.InspectionPlanSubs.Where(x => x.BoolDeleted == false && x.IntPlanTypeID == PlanTypeId) on i.IntID equals iSub.IntInspPlanID
                                  join iDe in _context.InspectionPlanDatas.Where(x => x.BoolDeleted == false && x.IntPlanState == 1 && x.IntCharacteristicID == CharacteristicId) on iSub.IntID equals iDe.IntInspPlanSubID
                                  join c in _context.Characteristics on iDe.IntCharacteristicID equals c.IntID
                                  into grouping
                                  from inventory in grouping.DefaultIfEmpty()
                                  select new
                                  {
                                      CharacteristicId = iDe.IntCharacteristicID,
                                      Mold = m.IntMoldQty,
                                      Cavity = m.IntCavityQty,
                                      USL = iDe.FtUSL,
                                      LSL = iDe.FtLSL,
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
                .Where(x => x.IntProductionID == ProductionId && 
                    x.IntCharacteristicID == CharacteristicId && 
                    x.IntPlanTypeID == PlanTypeId && 
                    x.IntMoldID == MoldId && 
                    x.IntCavityID == CavityId);

            if (measDatas == null || !measDatas.Any()) {
                return NotFound("No Measurement data found.");
            }

            var measDataVms = measDatas.Select(measData => new MeasDataVm
            {
                Id = measData.IntID,
                ProductionId = measData.IntProductionID,
                CharacteristicId = measData.IntCharacteristicID,
                PlanTypeId = measData.IntPlanTypeID,
                MoldId = measData.IntMoldID,
                CavityId = measData.IntCavityID,
                LineId = measData.IntLineID,
                JobId = measData.IntJobID,
                CharacteristicRange = measData.VarCharacteristicRange,
                CharacteristicValue = measData.VarCharacteristicValue,
                DataCollection = measData.IntDataCollection,
                EmailSent = measData.IntEmailSent,
                OkNg = measData.IntOKNG,
                OutputNotes = measData.StrOutputNotes,
                OutputQty = measData.IntOutputQty,
                SampleIndex = measData.IntSampleIndex,
                SampleQty = measData.IntSampleQty,
                TimeMeasure = measData.DtTimeMeasure,
                TimeStamp = measData.DtTimeStamp,
                UserId = measData.IntUserID,
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

            var measData = await _context.MeasDatas.FirstOrDefaultAsync(x => x.IntID == Id);

            if (measData == null) {
                return BadRequest("Measurement data not found.");
            }

            _context.MeasDatas.Remove(measData);
            var result = await _context.SaveChangesAsync();
            
            if (result > 0) {
                return Ok(new MeasDataVm
                {
                    Id = measData.IntID,
                    ProductionId = measData.IntProductionID,
                    CharacteristicId = measData.IntCharacteristicID,
                    PlanTypeId = measData.IntPlanTypeID,
                    MoldId = measData.IntMoldID,
                    CavityId = measData.IntCavityID,
                    LineId = measData.IntLineID,
                    JobId = measData.IntJobID,
                    CharacteristicRange = measData.VarCharacteristicRange,
                    CharacteristicValue = measData.VarCharacteristicValue,
                    DataCollection = measData.IntDataCollection,
                    EmailSent = measData.IntEmailSent,
                    OkNg = measData.IntOKNG,
                    OutputNotes = measData.StrOutputNotes,
                    OutputQty = measData.IntOutputQty,
                    SampleIndex = measData.IntSampleIndex,
                    SampleQty = measData.IntSampleQty,
                    TimeMeasure = measData.DtTimeMeasure,
                    TimeStamp = measData.DtTimeStamp,
                    UserId = measData.IntUserID,
                });
            }
            else
            {
                return BadRequest("Failed to delete Measurement Data.");
            }
        }
    }
}
