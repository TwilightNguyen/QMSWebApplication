using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using QMSWebApplication.ViewModels.System;
using QMSWebApplication.ViewModels.System.InspectionPlanData;

namespace QMSWebApplication.BackendServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Bearer")]
    public class InspectionPlanDatasController(ApplicationDbContext context): ControllerBase
    {
        // Controller methods would go here
        private readonly ApplicationDbContext _context = context;

        /// <summary>
        ///  URL: /api/inspectionplandatas/
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateInspectionPlanData([FromBody] InspectionPlanDataCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inspPlanSub = _context.InspectionPlanSubs.FirstOrDefault(p => p.Id == request.InspPlanSubId && p.Enabled == true);

            if (inspPlanSub == null)
            {
                return BadRequest("Invalid Inspection Plan Sub.");
            }

            var characteristic = _context.Characteristics.FirstOrDefault(p => p.Id == request.CharacteristicId);

            if (characteristic == null)
            {
                return BadRequest("Invalid Characteristic.");
            }

            var inspectionPlanData = _context.InspectionPlanDatas.FirstOrDefault(x =>
                x.InspPlanSubId == request.InspPlanSubId &&
                x.CharacteristicId == request.CharacteristicId &&
                x.Enabled == true);

            bool HasSpecLimits = request.USL != null && request.LSL != null && request.PercentControlLimit != null;
            double SpecLimitRange = (double)((request.USL != null && request.LSL != null) ? (request.USL - request.LSL) : 0);
            
            bool IsCreate = inspectionPlanData == null;

            if (inspectionPlanData != null)
            {
                inspectionPlanData.LSL = request.LSL;
                inspectionPlanData.USL = request.USL;
                inspectionPlanData.LCL = HasSpecLimits ? request.USL - (request.PercentControlLimit / 100.0) * SpecLimitRange : (double?)null;
                inspectionPlanData.EnabledSPCChart = request.SPCChart;
                inspectionPlanData.DataEntry = request.DataEntry;
                //inspectionPlanData.PlanState = request.PlanState;
                inspectionPlanData.CpkMax = request.CpkMax;
                inspectionPlanData.CpkMin = request.CpkMin;
                inspectionPlanData.EnabledCpkControl = request.CpkControl;
                inspectionPlanData.Notes = request.SampleSize;
                inspectionPlanData.PercentControlLimit = request.PercentControlLimit;

                _context.InspectionPlanDatas.Update(inspectionPlanData);
            }
            else
            {
                inspectionPlanData = new InspectionPlanData
                {
                    InspPlanSubId = request.InspPlanSubId,
                    CharacteristicId = request.CharacteristicId,
                    LSL = request.LSL,
                    USL = request.USL,
                    LCL = HasSpecLimits ? request.LSL + (request.PercentControlLimit / 100.0) * SpecLimitRange : (double?)null,
                    UCL = HasSpecLimits ? request.USL - (request.PercentControlLimit / 100.0) * SpecLimitRange : (double?)null,
                    EnabledSPCChart = request.SPCChart,
                    DataEntry = request.DataEntry,
                    //PlanState = request.PlanState,
                    CpkMax = request.CpkMax,
                    CpkMin = request.CpkMin,
                    EnabledCpkControl = request.CpkControl,
                    Notes = request.SampleSize,
                    PercentControlLimit = request.PercentControlLimit,
                    Enabled = true,
                };

                _context.InspectionPlanDatas.Add(inspectionPlanData);
            }

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return CreatedAtAction(
                    nameof(GetById),
                    new { inspectionPlanData.Id, IsCreate },
                    new InspectionPlanDataVm
                    {
                        Id = inspectionPlanData.Id,
                        InspPlanSubId = inspectionPlanData.InspPlanSubId,
                        CharacteristicId = inspectionPlanData.CharacteristicId,
                        LSL = inspectionPlanData.LSL,
                        USL = inspectionPlanData.USL,
                        LCL = inspectionPlanData.LCL,
                        UCL = inspectionPlanData.UCL,
                        SPCChart = inspectionPlanData.Enabled,
                        DataEntry = inspectionPlanData.DataEntry,
                        //PlanState = inspectionPlanData.PlanState,
                        CpkMax = inspectionPlanData.CpkMax,
                        CpkMin = inspectionPlanData.CpkMin,
                        CpkControl = inspectionPlanData.EnabledCpkControl,
                        SampleSize = inspectionPlanData.Notes,
                        PercentControlLimit = inspectionPlanData.PercentControlLimit,
                        CharacteristicName = characteristic.Name,
                    }
                );
            }
            else
            {
                return BadRequest("Failed to create Inspection Plan Sub.");
            }
        }

        /// <summary>
        /// Url: /api/inspectionplandatas/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var inspectionPlanData = _context.InspectionPlanDatas.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if (inspectionPlanData == null)
            {
                return NotFound("Inspection Plan Data not found.");
            }

            var inspectionPlanDataVm = new InspectionPlanDataVm
            {
                Id = inspectionPlanData.Id,
                InspPlanSubId = inspectionPlanData.InspPlanSubId,
                CharacteristicId = inspectionPlanData.CharacteristicId,
                LSL = inspectionPlanData.LSL,
                USL = inspectionPlanData.USL,
                LCL = inspectionPlanData.LCL,
                UCL = inspectionPlanData.UCL,
                SPCChart = inspectionPlanData.EnabledSPCChart,
                DataEntry = inspectionPlanData.DataEntry,
                //PlanState = inspectionPlanData.PlanState,
                CpkMax = inspectionPlanData.CpkMax,
                CpkMin = inspectionPlanData.CpkMin,
                CpkControl = inspectionPlanData.EnabledCpkControl,
                SampleSize = inspectionPlanData.Notes,
                PercentControlLimit = inspectionPlanData.PercentControlLimit,
            };

            return Ok(inspectionPlanDataVm);
        }


        /// <summary>
        /// Url: /api/inspectionplandatas/GetByInsPlanSubId/{Id}
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("/GetByInsPlanSubIdAndPlanState/{InsPlanSubId:int}/{PlanState:int}")]
        public async Task<IActionResult> GetByInsPlanSubIdAndPlanState(int InsPlanSubId, int? PlanState)
        {
            var inspectionPlanSubs = _context.InspectionPlanSubs.Where(r => r.Id == InsPlanSubId && r.PlanState == PlanState && r.Enabled == true);

            if (inspectionPlanSubs == null || inspectionPlanSubs?.Count() == 0)
            {
                return NotFound("Inspection Plan Sub not found.");
            }

            var InspectionPlanDataVms = _context.InspectionPlanDatas
                .Where(r => 
                    r.InspPlanSubId == InsPlanSubId && 
                    //(r.PlanState == PlanState || PlanState == -1 || PlanState == null) &&
                    r.Enabled == true
                )
                .Select(inspectionPlanData => new InspectionPlanDataVm
                {
                    Id = inspectionPlanData.Id,
                    InspPlanSubId = inspectionPlanData.InspPlanSubId,
                    CharacteristicId = inspectionPlanData.CharacteristicId,
                    LSL = inspectionPlanData.LSL,
                    USL = inspectionPlanData.USL,
                    LCL = inspectionPlanData.LCL,
                    UCL = inspectionPlanData.UCL,
                    SPCChart = inspectionPlanData.Enabled,
                    DataEntry = inspectionPlanData.DataEntry,
                    //PlanState = inspectionPlanData.PlanState,
                    CpkMax = inspectionPlanData.CpkMax,
                    CpkMin = inspectionPlanData.CpkMin,
                    CpkControl = inspectionPlanData.EnabledCpkControl,
                    SampleSize = inspectionPlanData.Notes,
                    PercentControlLimit = inspectionPlanData.PercentControlLimit,
                }).ToList();

            return Ok(InspectionPlanDataVms);
        }

        /// <summary>
        /// Url: /api/inspectionplandatas/{Id}
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteInspectionPlanData(int Id)
        {
            var inspectionPlanData = _context.InspectionPlanDatas.FirstOrDefault(r => r.Id == Id && r.Enabled == true);

            if (inspectionPlanData == null)
            {
                return NotFound("Inspection Plan Data not found.");
            }

            inspectionPlanData.Enabled = false;
            
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
            {
                var characteristic = _context.Characteristics.FirstOrDefault(p => p.Id == inspectionPlanData.CharacteristicId);

                return Ok(new InspectionPlanDataVm
                {
                    Id = inspectionPlanData.Id,
                    InspPlanSubId = inspectionPlanData.InspPlanSubId,
                    CharacteristicId = inspectionPlanData.CharacteristicId,
                    LSL = inspectionPlanData.LSL,
                    USL = inspectionPlanData.USL,
                    LCL = inspectionPlanData.LCL,
                    UCL = inspectionPlanData.UCL,
                    SPCChart = inspectionPlanData.EnabledSPCChart,
                    DataEntry = inspectionPlanData.DataEntry,
                    //PlanState = inspectionPlanData.PlanState,
                    CpkMax = inspectionPlanData.CpkMax,
                    CpkMin = inspectionPlanData.CpkMin,
                    CpkControl = inspectionPlanData.EnabledCpkControl,
                    SampleSize = inspectionPlanData.Notes,
                    PercentControlLimit = inspectionPlanData.PercentControlLimit,

                    CharacteristicName = characteristic?.Name,
                });
            }
            else
            {
                return BadRequest("Failed to delete Inspection Plan Data.");
            }
        }
    }
}
