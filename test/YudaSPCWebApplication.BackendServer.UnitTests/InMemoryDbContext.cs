using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using YudaSPCWebApplication.BackendServer.Data;
using YudaSPCWebApplication.BackendServer.Data.Entities;

namespace YudaSPCWebApplication.BackendServer.UnitTest
{
    public static class InMemoryDbContext
    {
        public static ApplicationDbContext GetApplicationDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }

        public static void DisposeDbContext(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        public static void SeedProductionAreas(ApplicationDbContext context)
        {
            context.ProductionAreas.AddRange(new List<ProductionArea>
            {
                new() { IntID = 1, StrNameArea = "Tape" },
                new() { IntID = 2, StrNameArea = "Layout" },
                new() { IntID = 3, StrNameArea = "Block Vial" },
                new() { IntID = 4, StrNameArea = "Print" }
            });
            context.SaveChanges();
        }

        public static void SeedRoles(ApplicationDbContext context)
        {
            context.Roles.AddRange(new List<Role>
            {
                new() { Id = Guid.NewGuid().ToString(), Name = "Admin", IntRoleID = 1, StrRoleName = "Admin", StrDescription = "Administrator Role", IntLevel = 1, IntRoleUser = 5 },
                new() { Id = Guid.NewGuid().ToString(), Name = "User",  IntRoleID = 2, StrRoleName = "User", StrDescription = "User Role", IntLevel = 2, IntRoleUser = 10 },
                new() { Id = Guid.NewGuid().ToString(), Name = "Manager",  IntRoleID = 3, StrRoleName = "Manager", StrDescription = "Manager Role", IntLevel = 3, IntRoleUser = 3 },
                new() { Id = Guid.NewGuid().ToString(), Name = "Assistant",  IntRoleID = 4, StrRoleName = "Assistant", StrDescription = "Assistant Role", IntLevel = 4, IntRoleUser = 7 },
                new() { Id = Guid.NewGuid().ToString(), Name = "Supervisor",  IntRoleID = 5, StrRoleName = "Supervisor", StrDescription = "Supervisor Role", IntLevel = 5, IntRoleUser = 2 },
            });
            context.SaveChanges();
        }

        public static void SeedUsers(ApplicationDbContext context)
        {
            context.Users.AddRange(new List<User>
            {
                new() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    Email = "admin@gmal.com",
                    StrFullName = "System Administrator",
                    IntUserID = 1,
                    IntEnable = 1,
                    StrRoleID = "1",
                    StrPassword = "Admin@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "admin@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "A001",
                    DtLastActivityTime = DateTime.UtcNow
                },
                new() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "super",
                    NormalizedUserName = "SUPER",
                    NormalizedEmail = "SUPER@GMAIL.COM",
                    Email = "super@gmal.com",
                    StrFullName = "System Supervisor",
                    IntUserID = 2,
                    IntEnable = 1,
                    StrRoleID = "2",
                    StrPassword = "Super@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "super@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "S001",
                    DtLastActivityTime = DateTime.UtcNow
                },
                new() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "manager",
                    NormalizedUserName = "MANAGER",
                    NormalizedEmail = "MANAGER@GMAIL.COM",
                    Email = "manager@gmal.com",
                    StrFullName = "System Manager",
                    IntUserID = 3,
                    IntEnable = 1,
                    StrRoleID = "3",
                    StrPassword = "Manager@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "manager@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "M001",
                    DtLastActivityTime = DateTime.UtcNow
                },

                new() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "technician",
                    NormalizedUserName = "TECHNICIAN",
                    NormalizedEmail = "TECHNICIAN@GMAIL.COM",
                    Email = "technician@gmal.com",
                    StrFullName = "System Technician",
                    IntUserID = 4,
                    IntEnable = 1,
                    StrRoleID = "4",
                    StrPassword = "Technician@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "technician@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "T001",
                    DtLastActivityTime = DateTime.UtcNow
                },
                new() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "operator",
                    NormalizedUserName = "OPERATOR",
                    NormalizedEmail = "OPERATOR@GMAIL.COM",
                    Email = "operator@gmal.com",
                    StrFullName = "System Operator",
                    IntUserID = 5,
                    IntEnable = 1,
                    StrRoleID = "5",
                    StrPassword = "Operator@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "operator@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "O001",
                    DtLastActivityTime = DateTime.UtcNow
                },
                new() {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "guest",
                    NormalizedUserName = "GUEST",
                    NormalizedEmail = "GUEST@GMAIL.COM",
                    Email = "guest@gmal.com",
                    StrFullName = "System Guest",
                    IntUserID = 6,
                    IntEnable = 1,
                    StrRoleID = "6",
                    StrPassword = "Guest@123",
                    StrDepartment = "IT",
                    StrEmailAddress = "guest@gmal.com",
                    StrSelectedAreaID = "1",
                    StrStaffID = "G001",
                    DtLastActivityTime = DateTime.UtcNow
                },
            });
            context.SaveChanges();
        }

        public static void SeedProcesses(ApplicationDbContext context)
        {
            context.Processes.AddRange(new List<Process>
            {
                new() { IntID = 1, StrProcessName = "Process A", IntAreaID = 1 },
                new() { IntID = 2, StrProcessName = "Process B", IntAreaID = 1 },
                new() { IntID = 3, StrProcessName = "Process C", IntAreaID = 2 },
                new() { IntID = 4, StrProcessName = "Process D", IntAreaID = 3 },
            });
            context.SaveChanges();
        }

        public static void SeedProcessLines(ApplicationDbContext context)
        {
            context.ProcessLines.AddRange(new List<ProcessLine>
            {
                new() { IntID = 1, StrProcessLineName = "Process A Line A", StrProcessLineCode = "A0A", IntProcessID = 1 },
                new() { IntID = 2, StrProcessLineName = "Process A Line B", StrProcessLineCode = "A0B", IntProcessID = 1 },
                new() { IntID = 3, StrProcessLineName = "Process B Line C", StrProcessLineCode = "B0C", IntProcessID = 2 },
                new() { IntID = 4, StrProcessLineName = "Process C Line D", StrProcessLineCode = "C0D", IntProcessID = 3 }
            });
            context.SaveChanges();
        }

        public static void SeedEventLogs(ApplicationDbContext context)
        {
            context.EventLogs.AddRange(new List<EventLog>
            {
                new() { IntEventID = 1, DtEventTime = DateTime.UtcNow.AddHours(-2), StrEventCode = "E001", StrEvent = "User Admin logged in", StrStation = "192.168.1.12" },
                new() { IntEventID = 2, DtEventTime = DateTime.UtcNow.AddHours(-1), StrEventCode = "E002", StrEvent = "User Admin logged out", StrStation = "192.168.1.13" },
                new() { IntEventID = 3, DtEventTime = DateTime.UtcNow, StrEventCode = "E003", StrEvent = "Error occurred", StrStation = "192.168.1.12" }
            });
            context.SaveChanges();
        }

        public static void SeedMeasureTypes(ApplicationDbContext context)
        {
            context.MeasureTypes.AddRange(new List<MeasureType>
            {
                new() { IntID = 1, StrMeaType = "Length" },
                new() { IntID = 2, StrMeaType = "Weight" },
                new() { IntID = 3, StrMeaType = "Temperature" }
            });
            context.SaveChanges();
        }

        public static void SeedCharacteristics(ApplicationDbContext context)
        {
            context.Characteristics.AddRange(new List<Characteristic>
            {
                new() { IntID = 1, StrCharacteristicName = "Characteristic A", IntProcessID = 1, IntMeaTypeID = 1, IntCharacteristicType = 0, IntDecimals = 2, StrCharacteristicUnit = "G", IntDefectRateLimit = 10, IntEmailEventModel = 2, IntEventEnable = 1,BoolDeleted = false},
                new() { IntID = 2, StrCharacteristicName = "Characteristic B", IntProcessID = 1, IntMeaTypeID = 1, IntCharacteristicType = 0, IntDecimals = 2, StrCharacteristicUnit = "KG", IntDefectRateLimit = null, IntEmailEventModel = 0, IntEventEnable = 0,BoolDeleted = false},
                new() { IntID = 3, StrCharacteristicName = "Characteristic C", IntProcessID = 2, IntMeaTypeID = 1, IntCharacteristicType = 0, IntDecimals = 2, StrCharacteristicUnit = "ML", IntDefectRateLimit = null, IntEmailEventModel = 1, IntEventEnable = 1,BoolDeleted = false},
            });
            context.SaveChanges();
        }

        public static void SeedInspectionPlans(ApplicationDbContext context)
        {
            context.InspectionPlans.AddRange(new List<InspectionPlan>
            {
                new() { IntID = 1, StrInspPlanName = "Inspection Plan A", IntAreaID = 1, BoolDeleted = false },
                new() { IntID = 2, StrInspPlanName = "Inspection Plan B", IntAreaID = 1, BoolDeleted = false },
                new() { IntID = 3, StrInspPlanName = "Inspection Plan C", IntAreaID = 2,  BoolDeleted = false }
            });
            context.SaveChanges();
        }

        public static void SeedInspPlanTypes(ApplicationDbContext context)
        {
            context.InspPlanTypes.AddRange(new List<InspectionPlanType>
            {
                new() { IntID = -1, StrPlanTypeName = "[ None ]" },
                new() { IntID = 1, StrPlanTypeName = "FPI" },
                new() { IntID = 2, StrPlanTypeName = "IPQC" },
                new() { IntID = 3, StrPlanTypeName = "OQC" }
            });
            context.SaveChanges();
        }

        public static void SeedInspectionPlanSubs(ApplicationDbContext context)
        {
            context.InspectionPlanSubs.AddRange(new List<InspectionPlanSub>
            {
                new() { IntID = 1, IntInspPlanID = 1, IntPlanTypeID = 1, DtCreateTime = DateTime.UtcNow, BoolDeleted = false },
                new() { IntID = 2, IntInspPlanID = 1, IntPlanTypeID = 2, DtCreateTime = DateTime.UtcNow, BoolDeleted = false },
                new() { IntID = 3, IntInspPlanID = 2, IntPlanTypeID = 1, DtCreateTime = DateTime.UtcNow, BoolDeleted = false }
            });
            context.SaveChanges();
        }

        public static void SeedInspectionPlanDatas(ApplicationDbContext context)
        {
            context.InspectionPlanDatas.AddRange(new List<InspectionPlanData>
            {
                new() { IntID = 1, IntInspPlanSubID = 1, IntCharacteristicID = 1, FtLSL = 10, FtUSL = 20, FtLCL = 12, FtUCL = 18, BoolSPCChart = true, BoolDataEntry = true, IntPlanState = 1, FtCpkMax = 1.33, FtCpkMin = 1.00, BoolSpkControl = true, StrSampleSize = "5", FtPercentControlLimit = 95, BoolDeleted = false },
                new() { IntID = 2, IntInspPlanSubID = 1, IntCharacteristicID = 2, FtLSL = 5, FtUSL = 15, FtLCL = 7, FtUCL = 13, BoolSPCChart = false, BoolDataEntry = true, IntPlanState = 1, FtCpkMax = 1.50, FtCpkMin = 1.20, BoolSpkControl = false, StrSampleSize = "3", FtPercentControlLimit = 90, BoolDeleted = false },
                new() { IntID = 3, IntInspPlanSubID = 2, IntCharacteristicID = 3, FtLSL = 100, FtUSL = 200, FtLCL = 120, FtUCL = 180, BoolSPCChart = true, BoolDataEntry = false, IntPlanState = 2, FtCpkMax = 1.25, FtCpkMin = 1.10, BoolSpkControl = true, StrSampleSize = "4", FtPercentControlLimit = 92, BoolDeleted = false }
            });
            context.SaveChanges();
        }

        public static void SeedProducts(ApplicationDbContext context)
        {
            context.Products.AddRange(new List<ProductName>
            {
                new () { IntID = 1, StrNameProduct = "Product 01", IntAreaID = 1, IntInspPlanID = 1, IntMoldQty = 2, IntCavityQty = 4, StrModelInternal = "Model Test 01", StrDescription = "Description 01", StrCustomerName = "Customer 01", StrNotes = "Notes 01", BoolDeleted = false },
                new () { IntID = 2, StrNameProduct = "Product 02", IntAreaID = 1, IntInspPlanID = 1, IntMoldQty = 3, IntCavityQty = 5, StrModelInternal = "Model Test 02", StrDescription = "Description 02", StrCustomerName = "Customer 02", StrNotes = "Notes 02", BoolDeleted = false },
                new () { IntID = 3, StrNameProduct = "Product 03", IntAreaID = 2, IntInspPlanID = 3, IntMoldQty = 4, IntCavityQty = 6, StrModelInternal = "Model Test 03", StrDescription = "Description 03", StrCustomerName = "Customer 03", StrNotes = "Notes 03", BoolDeleted = false },
                new () { IntID = 4, StrNameProduct = "Product 04", IntAreaID = 2, IntInspPlanID = 3, IntMoldQty = 4, IntCavityQty = 6, StrModelInternal = "Model Test 04", StrDescription = "Description 04", StrCustomerName = "Customer 04", StrNotes = "Notes 04", BoolDeleted = false },
            });
            context.SaveChanges();
        }

        public static void SeedJobDecisions(ApplicationDbContext context)
        {
            context.JobDecisions.AddRange(new List<JobDecision>
            {
                new() { IntID = 1, StrDecision = "Not yet decision", IntColorCode = 16777215 },
                new() { IntID = 2, StrDecision = "Pass", IntColorCode = 33280 },
                new() { IntID = 3, StrDecision = "Sorting", IntColorCode = 16776960 },
                new() { IntID = 4, StrDecision = "Rework", IntColorCode = 16776960 },
                new() { IntID = 5, StrDecision = "AOD", IntColorCode = 16776960 },
                new() { IntID = 6, StrDecision = "Reject", IntColorCode = 16776960 },
            });
            context.SaveChanges();
        }

        public static void SeedJobs(ApplicationDbContext context) { 
            context.JobDatas.AddRange(new List<JobData> {
                new() { 
                    IntID = 1, 
                    StrJobCode = "Job Code 1", 
                    StrPOCode = "PO Code 1", 
                    StrSOCode = "SO Code 1", 
                    IntAreaID = 1, 
                    IntProductID = 1, 
                    IntJobDecisionID = 1, 
                    IntJobQty = 1000, 
                    IntOutputQty = 1000, 
                    DtCreateTime = DateTime.Now, 
                    BoolDeleted = false, 
                    IntUserID = 1 
                },
                new() {
                    IntID = 2,
                    StrJobCode = "Job Code 2",
                    StrPOCode = "PO Code 2",
                    StrSOCode = "SO Code 2",
                    IntAreaID = 1,
                    IntProductID = 2,
                    IntJobDecisionID = 1,
                    IntJobQty = 1000,
                    IntOutputQty = 1000,
                    DtCreateTime = DateTime.Now,
                    BoolDeleted = false,
                    IntUserID = 1
                },
                new() {
                    IntID = 3,
                    StrJobCode = "Job Code 3",
                    StrPOCode = "PO Code 3",
                    StrSOCode = "SO Code 3",
                    IntAreaID = 2,
                    IntProductID = 3,
                    IntJobDecisionID = 1,
                    IntJobQty = 1000,
                    IntOutputQty = 1000,
                    DtCreateTime = DateTime.Now,
                    BoolDeleted = false,
                    IntUserID = 1
                },
                new() {
                    IntID = 4,
                    StrJobCode = "Job Code 4",
                    StrPOCode = "PO Code 4",
                    StrSOCode = "SO Code 4",
                    IntAreaID = 1,
                    IntProductID = 1,
                    IntJobDecisionID = 1,
                    IntJobQty = 1000,
                    IntOutputQty = 1000,
                    DtCreateTime = DateTime.Now,
                    BoolDeleted = false,
                    IntUserID = 1
                },
            });
            context.SaveChanges();
        }

        public static void SeedShifts(ApplicationDbContext context)
        {
            context.Shifts.AddRange(new List<Shift>{
                new() { IntID = 1, StrNameShift = "Shift 1", DtStartTime = new TimeSpan(6,0,0), DtEndTime = new TimeSpan(18,0,0) },
                new() { IntID = 2, StrNameShift = "Shift 2", DtStartTime = new TimeSpan(18,0,0), DtEndTime = new TimeSpan(6,0,0) }
            });

            context.SaveChanges();
        }

        public static void SeedProductions(ApplicationDbContext context) {
            context.ProductionDatas.AddRange(new List<ProductionData>
            {
                new(){ 
                    IntID = 1, 
                    IntJobID = 1, 
                    IntLineID = 1, 
                    IntProductionQty = 1000, 
                    DtProductionDate = DateTime.Now.AddDays(-9), 
                    StrLotInform = "Lot Inform 1",
                    StrMaterialInform = "Material Inform 1",
                    StrNotes = "Note 1",
                    DtStartTime = DateTime.Now.AddDays(-10),
                    DtEndTime = DateTime.Now.AddDays(-8),
                    IntUserID = 1,
                    BoolDeleted = false,
                    IntCNCLatheMachine = null,
                },
                new(){
                    IntID = 2,
                    IntJobID = 1,
                    IntLineID = 1,
                    IntProductionQty = 2000,
                    DtProductionDate = DateTime.Now.AddDays(-1),
                    StrLotInform = "Lot Inform 2",
                    StrMaterialInform = "Material Inform 2",
                    StrNotes = "Note 2",
                    DtStartTime = DateTime.Now.AddDays(-2),
                    DtEndTime = null,
                    IntUserID = 1,
                    BoolDeleted = false,
                    IntCNCLatheMachine = null,
                },
                new(){
                    IntID = 3,
                    IntJobID = 2,
                    IntLineID = 2,
                    IntProductionQty = 3000,
                    DtProductionDate = DateTime.Now.AddDays(-1),
                    StrLotInform = "Lot Inform 3",
                    StrMaterialInform = "Material Inform 3",
                    StrNotes = "Note 3",
                    DtStartTime = DateTime.Now.AddDays(-2),
                    DtEndTime = null,
                    IntUserID = 1,
                    BoolDeleted = false,
                    IntCNCLatheMachine = null,
                },
                new(){
                    IntID = 4,
                    IntJobID = 3,
                    IntLineID = 3,
                    IntProductionQty = 3000,
                    DtProductionDate = DateTime.Now.AddDays(-1),
                    StrLotInform = "Lot Inform 4",
                    StrMaterialInform = "Material Inform 4",
                    StrNotes = "Note 4",
                    DtStartTime = DateTime.Now.AddDays(-2),
                    DtEndTime = null,
                    IntUserID = 1,
                    BoolDeleted = false,
                    IntCNCLatheMachine = null,
                },
            });

            context.SaveChanges();
        }

        public static void SeedTvDisplay(ApplicationDbContext context)
        {
            context.TVDisplays.AddRange(new List<TVDisplay> {
                new()
                {
                    IntID = 1,
                    StrTVName = "Tapes",
                    StrProductionID = "1",
                    StrCharacteristicID = "1",
                    StrPlanTypeID = "1",
                    StrMoldID = "1",
                    StrCavityID = "1",

                },
                new()
                {
                    IntID = 2,
                    StrTVName = "Layout",
                    StrProductionID = null,
                    StrCharacteristicID = null,
                    StrPlanTypeID = null,
                    StrMoldID = null,
                    StrCavityID = null,

                }
            });

            context.SaveChanges();
        }

        public static void SeedMeasData(ApplicationDbContext context)
        {
            context.MeasDatas.AddRange(new List<MeasData3_01>
            {
                new() {
                    IntID = 1,
                    IntProductionID = 1,
                    IntJobID = 1,
                    IntCharacteristicID = 1,
                    IntLineID = 1,
                    VarCharacteristicValue = "15",
                    DtTimeMeasure = DateTime.Now.AddDays(-9).AddMinutes(1),
                    DtTimeStamp = DateTime.Now.AddDays(-9).AddMinutes(1),
                    IntMoldID = 1,
                    IntCavityID = 1,
                    IntDataCollection = 1,
                    IntEmailSent = 0,
                    IntOKNG = 0,
                    IntOutputQty = 1,
                    IntPlanTypeID = 1,
                    IntSampleIndex = 5,
                    IntSampleQty = 5,
                    IntUserID = 1,
                    StrOutputNotes = "Note 1"
                },
                new() {
                    IntID = 2,
                    IntProductionID = 1,
                    IntJobID = 1,
                    IntCharacteristicID = 2,
                    IntLineID = 1,
                    VarCharacteristicValue = "10",
                    DtTimeMeasure = DateTime.Now.AddDays(-9).AddMinutes(2),
                    DtTimeStamp = DateTime.Now.AddDays(-9).AddMinutes(2),
                    IntMoldID = 1,
                    IntCavityID = 1,
                    IntDataCollection = 1,
                    IntEmailSent = 0,
                    IntOKNG = 0,
                    IntOutputQty = 1,
                    IntPlanTypeID = 1,
                    IntSampleIndex = 5,
                    IntSampleQty = 5,
                    IntUserID = 1,
                    StrOutputNotes = "Note 1"
                },
                new() {
                    IntID = 3,
                    IntProductionID = 1,
                    IntJobID = 1,
                    IntCharacteristicID = 1,
                    IntLineID = 1,
                    VarCharacteristicValue = "14",
                    DtTimeMeasure = DateTime.Now.AddDays(-9).AddMinutes(3),
                    DtTimeStamp = DateTime.Now.AddDays(-9).AddMinutes(3),
                    IntMoldID = 1,
                    IntCavityID = 1,
                    IntDataCollection = 1,
                    IntEmailSent = 0,
                    IntOKNG = 0,
                    IntOutputQty = 1,
                    IntPlanTypeID = 1,
                    IntSampleIndex = 5,
                    IntSampleQty = 5,
                    IntUserID = 1,
                    StrOutputNotes = "Note 1"
                },
                new() {
                    IntID = 4,
                    IntProductionID = 1,
                    IntJobID = 1,
                    IntCharacteristicID = 2,
                    IntLineID = 1,
                    VarCharacteristicValue = "12",
                    DtTimeMeasure = DateTime.Now.AddDays(-9).AddMinutes(4),
                    DtTimeStamp = DateTime.Now.AddDays(-9).AddMinutes(4),
                    IntMoldID = 1,
                    IntCavityID = 1,
                    IntDataCollection = 1,
                    IntEmailSent = 0,
                    IntOKNG = 0,
                    IntOutputQty = 1,
                    IntPlanTypeID = 1,
                    IntSampleIndex = 5,
                    IntSampleQty = 5,
                    IntUserID = 1,
                    StrOutputNotes = "Note 1"
                },
                new() {
                    IntID = 5,
                    IntProductionID = 1,
                    IntJobID = 1,
                    IntCharacteristicID = 1,
                    IntLineID = 1,
                    VarCharacteristicValue = "16",
                    DtTimeMeasure = DateTime.Now.AddDays(-9).AddMinutes(5),
                    DtTimeStamp = DateTime.Now.AddDays(-9).AddMinutes(5),
                    IntMoldID = 1,
                    IntCavityID = 1,
                    IntDataCollection = 1,
                    IntEmailSent = 0,
                    IntOKNG = 0,
                    IntOutputQty = 1,
                    IntPlanTypeID = 1,
                    IntSampleIndex = 5,
                    IntSampleQty = 5,
                    IntUserID = 1,
                    StrOutputNotes = "Note 1"
                },
                new() {
                    IntID = 6,
                    IntProductionID = 1,
                    IntJobID = 1,
                    IntCharacteristicID = 2,
                    IntLineID = 1,
                    VarCharacteristicValue = "11",
                    DtTimeMeasure = DateTime.Now.AddDays(-9).AddMinutes(6),
                    DtTimeStamp = DateTime.Now.AddDays(-9).AddMinutes(6),
                    IntMoldID = 1,
                    IntCavityID = 1,
                    IntDataCollection = 1,
                    IntEmailSent = 0,
                    IntOKNG = 0,
                    IntOutputQty = 1,
                    IntPlanTypeID = 1,
                    IntSampleIndex = 5,
                    IntSampleQty = 5,
                    IntUserID = 1,
                    StrOutputNotes = "Note 1"
                },
                new() {
                    IntID = 7,
                    IntProductionID = 1,
                    IntJobID = 1,
                    IntCharacteristicID = 1,
                    IntLineID = 1,
                    VarCharacteristicValue = "13",
                    DtTimeMeasure = DateTime.Now.AddDays(-9).AddMinutes(7),
                    DtTimeStamp = DateTime.Now.AddDays(-9).AddMinutes(7),
                    IntMoldID = 1,
                    IntCavityID = 1,
                    IntDataCollection = 1,
                    IntEmailSent = 0,
                    IntOKNG = 0,
                    IntOutputQty = 1,
                    IntPlanTypeID = 1,
                    IntSampleIndex = 5,
                    IntSampleQty = 5,
                    IntUserID = 1,
                    StrOutputNotes = "Note 1"
                },
                new() {
                    IntID = 8,
                    IntProductionID = 1,
                    IntJobID = 1,
                    IntCharacteristicID = 2,
                    IntLineID = 1,
                    VarCharacteristicValue = "6",
                    DtTimeMeasure = DateTime.Now.AddDays(-9).AddMinutes(8),
                    DtTimeStamp = DateTime.Now.AddDays(-9).AddMinutes(8),
                    IntMoldID = 1,
                    IntCavityID = 1,
                    IntDataCollection = 1,
                    IntEmailSent = 0,
                    IntOKNG = 0,
                    IntOutputQty = 1,
                    IntPlanTypeID = 1,
                    IntSampleIndex = 5,
                    IntSampleQty = 5,
                    IntUserID = 1,
                    StrOutputNotes = "Note 1"
                },
                new() {
                    IntID = 9,
                    IntProductionID = 1,
                    IntJobID = 1,
                    IntCharacteristicID = 1,
                    IntLineID = 1,
                    VarCharacteristicValue = "18",
                    DtTimeMeasure = DateTime.Now.AddDays(-9).AddMinutes(9),
                    DtTimeStamp = DateTime.Now.AddDays(-9).AddMinutes(9),
                    IntMoldID = 1,
                    IntCavityID = 1,
                    IntDataCollection = 1,
                    IntEmailSent = 0,
                    IntOKNG = 0,
                    IntOutputQty = 1,
                    IntPlanTypeID = 1,
                    IntSampleIndex = 5,
                    IntSampleQty = 5,
                    IntUserID = 1,
                    StrOutputNotes = "Note 1"
                },
                new() {
                    IntID = 10,
                    IntProductionID = 1,
                    IntJobID = 1,
                    IntCharacteristicID = 2,
                    IntLineID = 1,
                    VarCharacteristicValue = "14",
                    DtTimeMeasure = DateTime.Now.AddDays(-9).AddMinutes(10),
                    DtTimeStamp = DateTime.Now.AddDays(-9).AddMinutes(10),
                    IntMoldID = 1,
                    IntCavityID = 1,
                    IntDataCollection = 1,
                    IntEmailSent = 0,
                    IntOKNG = 0,
                    IntOutputQty = 1,
                    IntPlanTypeID = 1,
                    IntSampleIndex = 5,
                    IntSampleQty = 5,
                    IntUserID = 1,
                    StrOutputNotes = "Note 1"
                },
            });
        }
    }
}
