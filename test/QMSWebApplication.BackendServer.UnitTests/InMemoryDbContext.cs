using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;

namespace QMSWebApplication.BackendServer.UnitTest
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
            context.ProductionAreas.AddRange(new List<ProductionAreas>
            {
                new() { Id = 1, Name = "Tape" },
                new() { Id = 2, Name = "Layout" },
                new() { Id = 3, Name = "Block Vial" },
                new() { Id = 4, Name = "Print" }
            });
            context.SaveChanges();
        }

        public static void SeedRoles(ApplicationDbContext context)
        {
            context.Roles.AddRange(new List<Roles>
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
            context.Processes.AddRange(new List<Processes>
            {
                new() { Id = 1, Name = "Process A", AreaId = 1 },
                new() { Id = 2, Name = "Process B", AreaId = 1 },
                new() { Id = 3, Name = "Process C", AreaId = 2 },
                new() { Id = 4, Name = "Process D", AreaId = 3 },
            });
            context.SaveChanges();
        }

        public static void SeedProcessLines(ApplicationDbContext context)
        {
            context.ProcessLines.AddRange(new List<ProcessLines>
            {
                new() { Id = 1, Name = "Process A Line A", LineCode = "A0A", ProcessId = 1 },
                new() { Id = 2, Name = "Process A Line B", LineCode = "A0B", ProcessId = 1 },
                new() { Id = 3, Name = "Process B Line C", LineCode = "B0C", ProcessId = 2 },
                new() { Id = 4, Name = "Process C Line D", LineCode = "C0D", ProcessId = 3 }
            });
            context.SaveChanges();
        }

        public static void SeedEventLogs(ApplicationDbContext context)
        {
            context.EventLogs.AddRange(new List<EventLogs>
            {
                new() { Id = 1, EventTime = DateTimeOffset.Now.AddHours(-2), EventCode = "E001", Description = "User Admin logged in", Station = "192.168.1.12" },
                new() { Id = 2, EventTime = DateTimeOffset.Now.AddHours(-1), EventCode = "E002", Description = "User Admin logged out", Station = "192.168.1.13" },
                new() { Id = 3, EventTime = DateTimeOffset.Now, EventCode = "E003", Description = "Error occurred", Station = "192.168.1.12" }
            });
            context.SaveChanges();
        }

        public static void SeedMeasureTypes(ApplicationDbContext context)
        {
            context.MeasureTypes.AddRange(new List<SampleTypes>
            {
                new() { Id = 1, Name = "Length" },
                new() { Id = 2, Name = "Weight" },
                new() { Id = 3, Name = "Temperature" }
            });
            context.SaveChanges();
        }

        public static void SeedCharacteristics(ApplicationDbContext context)
        {
            context.Characteristics.AddRange(new List<Characteristics>
            {
                new() { Id = 1, Name = "Characteristic A", ProcessId = 1, MeaTypeId = 1, DataType = 0, Decimals = 2, Unit = "G", DefectRateLimit = 10, EmailEventModel = 2, Enabled = true},
                new() { Id = 2, Name = "Characteristic B", ProcessId = 1, MeaTypeId = 1, DataType = 0, Decimals = 2, Unit = "KG", DefectRateLimit = null, EmailEventModel = 0, Enabled = true},
                new() { Id = 3, Name = "Characteristic C", ProcessId = 2, MeaTypeId = 1, DataType = 0, Decimals = 2, Unit = "ML", DefectRateLimit = null, EmailEventModel = 1, Enabled = true},
            });
            context.SaveChanges();
        }

        public static void SeedInspectionPlans(ApplicationDbContext context)
        {
            context.InspectionPlans.AddRange(new List<InspectionPlans>
            {
                new() { Id = 1, Name = "Inspection Plan A", AreaId = 1, Enabled = true },
                new() { Id = 2, Name = "Inspection Plan B", AreaId = 1, Enabled = true },
                new() { Id = 3, Name = "Inspection Plan C", AreaId = 2, Enabled = true }
            });
            context.SaveChanges();
        }

        public static void SeedInspPlanTypes(ApplicationDbContext context)
        {
            context.InspPlanTypes.AddRange(new List<InspectionPlanTypes>
            {
                new() { Id = -1, Name = "[ None ]" },
                new() { Id = 1, Name = "FPI" },
                new() { Id = 2, Name = "IPQC" },
                new() { Id = 3, Name = "OQC" }
            });
            context.SaveChanges();
        }

        public static void SeedInspectionPlanSubs(ApplicationDbContext context)
        {
            context.InspectionPlanSubs.AddRange(new List<InspectionPlanSubs>
            {
                new() { Id = 1, InspPlanId = 1, PlanTypeId = 1, PlanState = 1, UploadedDateTime = DateTimeOffset.Now, Enabled = true },
                new() { Id = 2, InspPlanId = 1, PlanTypeId = 2, PlanState = 1, UploadedDateTime = DateTimeOffset.Now, Enabled = true },
                new() { Id = 3, InspPlanId = 2, PlanTypeId = 1, PlanState = 1, UploadedDateTime = DateTimeOffset.Now, Enabled = true }
            });
            context.SaveChanges();
        }

        public static void SeedInspectionPlanDatas(ApplicationDbContext context)
        {
            context.InspectionPlanDatas.AddRange(new List<InspectionPlanData>
            {
                new() { Id = 1, InspPlanSubId = 1, CharacteristicId = 1, LSL = 10, USL = 20, LCL = 12, UCL = 18, EnabledSPCChart = true, DataEntry = true, CpkMax = 1.33, CpkMin = 1.00, EnabledCpkControl = true, Notes = "5", PercentControlLimit = 95, Enabled = true },
                new() { Id = 2, InspPlanSubId = 1, CharacteristicId = 2, LSL = 5, USL = 15, LCL = 7, UCL = 13, EnabledSPCChart = false, DataEntry = true, CpkMax = 1.50, CpkMin = 1.20, EnabledCpkControl = false, Notes = "3", PercentControlLimit = 90, Enabled = true },
                new() { Id = 3, InspPlanSubId = 2, CharacteristicId = 3, LSL = 100,USL = 200, LCL = 120, UCL = 180, EnabledSPCChart = true, DataEntry = false, CpkMax = 1.25, CpkMin = 1.10, EnabledCpkControl = true, Notes = "4", PercentControlLimit = 92, Enabled = true }
            });
            context.SaveChanges();
        }

        public static void SeedProducts(ApplicationDbContext context)
        {
            context.Products.AddRange(new List<Products>
            {
                new () { Id = 1, Name = "Product 01", AreaId = 1, InspPlanId = 1, MoldQuantity = 2, CavityQuantity = 4, ModelInternal = "Model Test 01", Description = "Description 01", CustomerName = "Customer 01", Notes = "Notes 01", Enabled = true },
                new () { Id = 2, Name = "Product 02", AreaId = 1, InspPlanId = 1, MoldQuantity = 3, CavityQuantity = 5, ModelInternal = "Model Test 02", Description = "Description 02", CustomerName = "Customer 02", Notes = "Notes 02", Enabled = true },
                new () { Id = 3, Name = "Product 03", AreaId = 2, InspPlanId = 3, MoldQuantity = 4, CavityQuantity = 6, ModelInternal = "Model Test 03", Description = "Description 03", CustomerName = "Customer 03", Notes = "Notes 03", Enabled = true },
                new () { Id = 4, Name = "Product 04", AreaId = 2, InspPlanId = 3, MoldQuantity = 4, CavityQuantity = 6, ModelInternal = "Model Test 04", Description = "Description 04", CustomerName = "Customer 04", Notes = "Notes 04", Enabled = true },
            });
            context.SaveChanges();
        }

        public static void SeedJobDecisions(ApplicationDbContext context)
        {
            context.JobDecisions.AddRange(new List<JobDecisions>
            {
                new() { Id = 1, Decision = "Not yet decision", ColorCode = "16777215" },
                new() { Id = 2, Decision = "Pass", ColorCode = "33280" },
                new() { Id = 3, Decision = "Sorting", ColorCode = "16776960" },
                new() { Id = 4, Decision = "Rework", ColorCode = "16776960" },
                new() { Id = 5, Decision = "AOD", ColorCode = "16776960" },
                new() { Id = 6, Decision = "Reject", ColorCode = "16776960" },
            });
            context.SaveChanges();
        }

        public static void SeedJobs(ApplicationDbContext context) { 
            context.JobDatas.AddRange(new List<Jobs> {
                new() { 
                    Id = 1, 
                    JobCode = "Job Code 1", 
                    POCode = "PO Code 1", 
                    SOCode = "SO Code 1", 
                    AreaId = 1, 
                    ProductId = 1, 
                    JobDecisionId = 1, 
                    PlannedQuantity = 1000, 
                    OutputQuantity = 1000, 
                    UploadedDateTime = DateTimeOffset.Now, 
                    Enabled = true,
                    UserId = 1 
                },
                new() {
                    Id = 2,
                    JobCode = "Job Code 2",
                    POCode = "PO Code 2",
                    SOCode = "SO Code 2",
                    AreaId = 1,
                    ProductId = 2,
                    JobDecisionId = 1,
                    PlannedQuantity = 1000,
                    OutputQuantity = 1000,
                    UploadedDateTime = DateTimeOffset.Now,
                    Enabled = true,
                    UserId = 1
                },
                new() {
                    Id = 3,
                    JobCode = "Job Code 3",
                    POCode = "PO Code 3",
                    SOCode = "SO Code 3",
                    AreaId = 2,
                    ProductId = 3,
                    JobDecisionId = 1,
                    PlannedQuantity = 1000,
                    OutputQuantity = 1000,
                    UploadedDateTime = DateTimeOffset.Now,
                    Enabled = true,
                    UserId = 1
                },
                new() {
                    Id = 4,
                    JobCode = "Job Code 4",
                    POCode = "PO Code 4",
                    SOCode = "SO Code 4",
                    AreaId = 1,
                    ProductId = 1,
                    JobDecisionId = 1,
                    PlannedQuantity = 1000,
                    OutputQuantity = 1000,
                    UploadedDateTime = DateTimeOffset.Now,
                    Enabled = true,
                    UserId = 1
                },
            });
            context.SaveChanges();
        }

        public static void SeedShifts(ApplicationDbContext context)
        {
            context.Shifts.AddRange(new List<Shifts>{
                new() { Id = 1, Name = "Shift 1", StartTime = new TimeSpan(6,0,0), EndTime = new TimeSpan(18,0,0) },
                new() { Id = 2, Name = "Shift 2", StartTime = new TimeSpan(18,0,0), EndTime = new TimeSpan(6,0,0) }
            });

            context.SaveChanges();
        }

        public static void SeedProductionPlans(ApplicationDbContext context) {
            context.ProductionPlans.AddRange(new List<ProductionPlans>
            {
                new(){ 
                    Id = 1, 
                    JobId = 1, 
                    LineId = 1, 
                    PlannedQuantity = 1000, 
                    ProductionDate = DateTime.Now.AddDays(-9), 
                    LotInform = "Lot Inform 1",
                    MaterialInform = "Material Inform 1",
                    Notes = "Note 1",
                    StartTime = DateTime.Now.AddDays(-10),
                    EndTime = DateTime.Now.AddDays(-8),
                    UserId = 1,
                    Enabled = true,
                    CNCLatheMachine = null,
                },
                new(){
                    Id = 2,
                    JobId = 1,
                    LineId = 1,
                    PlannedQuantity = 2000,
                    ProductionDate = DateTime.Now.AddDays(-1),
                    LotInform = "Lot Inform 2",
                    MaterialInform = "Material Inform 2",
                    Notes = "Note 2",
                    StartTime = DateTime.Now.AddDays(-2),
                    EndTime = null,
                    UserId = 1,
                    Enabled = true,
                    CNCLatheMachine = null,
                },
                new(){
                    Id = 3,
                    JobId = 2,
                    LineId = 2,
                    PlannedQuantity = 3000,
                    ProductionDate = DateTime.Now.AddDays(-1),
                    LotInform = "Lot Inform 3",
                    MaterialInform = "Material Inform 3",
                    Notes = "Note 3",
                    StartTime = DateTime.Now.AddDays(-2),
                    EndTime = null,
                    UserId = 1,
                    Enabled = true,
                    CNCLatheMachine = null,
                },
                new(){
                    Id = 4,
                    JobId = 3,
                    LineId = 3,
                    PlannedQuantity = 3000,
                    ProductionDate = DateTime.Now.AddDays(-1),
                    LotInform = "Lot Inform 4",
                    MaterialInform = "Material Inform 4",
                    Notes = "Note 4",
                    StartTime = DateTime.Now.AddDays(-2),
                    EndTime = null,
                    UserId = 1,
                    Enabled = true,
                    CNCLatheMachine = null,
                },
            });

            context.SaveChanges();
        }

        public static void SeedTvDisplay(ApplicationDbContext context)
        {
            context.TVDisplays.AddRange(new List<TVDisplays> {
                new()
                {
                    Id = 1,
                    Name = "Tapes",

                },
                new()
                {
                    Id = 2,
                    Name = "Layout",

                }
            });

            context.SaveChanges();
        }

        public static void SeedMeasData(ApplicationDbContext context)
        {
            context.MeasDatas.AddRange(new List<Samples>
            {
                new() {
                    Id = 1,
                    ProductionId = 1,
                    JobId = 1,
                    CharacteristicId = 1,
                    LineId = 1,
                    CharacteristicValue = "15",
                    UploadedDateTime = DateTime.Now.AddDays(-9).AddMinutes(1),
                    MeasuredDateTime = DateTime.Now.AddDays(-9).AddMinutes(1),
                    MoldId = 1,
                    CavityId = 1,
                    DataCollection = 1,
                    EmailSent = 0,
                    Status = 0,
                    OutputQuantity = 1,
                    PlanTypeId = 1,
                    SampleIndex = 5,
                    SampleQuantity = 5,
                    UserId = 1,
                    Notes = "Note 1"
                },
                new() {
                    Id = 2,
                    ProductionId = 1,
                    JobId = 1,
                    CharacteristicId = 2,
                    LineId = 1,
                    CharacteristicValue = "10",
                    UploadedDateTime = DateTime.Now.AddDays(-9).AddMinutes(2),
                    MeasuredDateTime = DateTime.Now.AddDays(-9).AddMinutes(2),
                    MoldId = 1,
                    CavityId = 1,
                    DataCollection = 1,
                    EmailSent = 0,
                    Status = 0,
                    OutputQuantity = 1,
                    PlanTypeId = 1,
                    SampleIndex = 5,
                    SampleQuantity = 5,
                    UserId = 1,
                    Notes = "Note 1"
                },
                new() {
                    Id = 3,
                    ProductionId = 1,
                    JobId = 1,
                    CharacteristicId = 1,
                    LineId = 1,
                    CharacteristicValue = "14",
                    UploadedDateTime = DateTime.Now.AddDays(-9).AddMinutes(3),
                    MeasuredDateTime = DateTime.Now.AddDays(-9).AddMinutes(3),
                    MoldId = 1,
                    CavityId = 1,
                    DataCollection = 1,
                    EmailSent = 0,
                    Status = 0,
                    OutputQuantity = 1,
                    PlanTypeId = 1,
                    SampleIndex = 5,
                    SampleQuantity = 5,
                    UserId = 1,
                    Notes = "Note 1"
                },
                new() {
                    Id = 4,
                    ProductionId = 1,
                    JobId = 1,
                    CharacteristicId = 2,
                    LineId = 1,
                    CharacteristicValue = "12",
                    UploadedDateTime = DateTime.Now.AddDays(-9).AddMinutes(4),
                    MeasuredDateTime = DateTime.Now.AddDays(-9).AddMinutes(4),
                    MoldId = 1,
                    CavityId = 1,
                    DataCollection = 1,
                    EmailSent = 0,
                    Status = 0,
                    OutputQuantity = 1,
                    PlanTypeId = 1,
                    SampleIndex = 5,
                    SampleQuantity = 5,
                    UserId = 1,
                    Notes = "Note 1"
                },
                new() {
                    Id = 5,
                    ProductionId = 1,
                    JobId = 1,
                    CharacteristicId = 1,
                    LineId = 1,
                    CharacteristicValue = "16",
                    UploadedDateTime = DateTime.Now.AddDays(-9).AddMinutes(5),
                    MeasuredDateTime = DateTime.Now.AddDays(-9).AddMinutes(5),
                    MoldId = 1,
                    CavityId = 1,
                    DataCollection = 1,
                    EmailSent = 0,
                    Status = 0,
                    OutputQuantity = 1,
                    PlanTypeId = 1,
                    SampleIndex = 5,
                    SampleQuantity = 5,
                    UserId = 1,
                    Notes = "Note 1"
                },
                new() {
                    Id = 6,
                    ProductionId = 1,
                    JobId = 1,
                    CharacteristicId = 2,
                    LineId = 1,
                    CharacteristicValue = "11",
                    UploadedDateTime = DateTime.Now.AddDays(-9).AddMinutes(6),
                    MeasuredDateTime = DateTime.Now.AddDays(-9).AddMinutes(6),
                    MoldId = 1,
                    CavityId = 1,
                    DataCollection = 1,
                    EmailSent = 0,
                    Status = 0,
                    OutputQuantity = 1,
                    PlanTypeId = 1,
                    SampleIndex = 5,
                    SampleQuantity = 5,
                    UserId = 1,
                    Notes = "Note 1"
                },
                new() {
                    Id = 7,
                    ProductionId = 1,
                    JobId = 1,
                    CharacteristicId = 1,
                    LineId = 1,
                    CharacteristicValue = "13",
                    UploadedDateTime = DateTime.Now.AddDays(-9).AddMinutes(7),
                    MeasuredDateTime = DateTime.Now.AddDays(-9).AddMinutes(7),
                    MoldId = 1,
                    CavityId = 1,
                    DataCollection = 1,
                    EmailSent = 0,
                    Status = 0,
                    OutputQuantity = 1,
                    PlanTypeId = 1,
                    SampleIndex = 5,
                    SampleQuantity = 5,
                    UserId = 1,
                    Notes = "Note 1"
                },
                new() {
                    Id = 8,
                    ProductionId = 1,
                    JobId = 1,
                    CharacteristicId = 2,
                    LineId = 1,
                    CharacteristicValue = "6",
                    UploadedDateTime = DateTime.Now.AddDays(-9).AddMinutes(8),
                    MeasuredDateTime = DateTime.Now.AddDays(-9).AddMinutes(8),
                    MoldId = 1,
                    CavityId = 1,
                    DataCollection = 1,
                    EmailSent = 0,
                    Status = 0,
                    OutputQuantity = 1,
                    PlanTypeId = 1,
                    SampleIndex = 5,
                    SampleQuantity = 5,
                    UserId = 1,
                    Notes = "Note 1"
                },
                new() {
                    Id = 9,
                    ProductionId = 1,
                    JobId = 1,
                    CharacteristicId = 1,
                    LineId = 1,
                    CharacteristicValue = "18",
                    UploadedDateTime = DateTime.Now.AddDays(-9).AddMinutes(9),
                    MeasuredDateTime = DateTime.Now.AddDays(-9).AddMinutes(9),
                    MoldId = 1,
                    CavityId = 1,
                    DataCollection = 1,
                    EmailSent = 0,
                    Status = 0,
                    OutputQuantity = 1,
                    PlanTypeId = 1,
                    SampleIndex = 5,
                    SampleQuantity = 5,
                    UserId = 1,
                    Notes = "Note 1"
                },
                new() {
                    Id = 10,
                    ProductionId = 1,
                    JobId = 1,
                    CharacteristicId = 2,
                    LineId = 1,
                    CharacteristicValue = "14",
                    UploadedDateTime = DateTime.Now.AddDays(-9).AddMinutes(10),
                    MeasuredDateTime = DateTime.Now.AddDays(-9).AddMinutes(10),
                    MoldId = 1,
                    CavityId = 1,
                    DataCollection = 1,
                    EmailSent = 0,
                    Status = 0,
                    OutputQuantity = 1,
                    PlanTypeId = 1,
                    SampleIndex = 5,
                    SampleQuantity = 5,
                    UserId = 1,
                    Notes = "Note 1"
                },
            });
        }
    }
}
