using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YudaSPCWebApplication.BackendServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "KnowledgeBaseSepence");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    intRoleID = table.Column<int>(type: "int", nullable: false),
                    strRoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    strDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    intLevel = table.Column<int>(type: "int", nullable: false),
                    intRoleUser = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    intUserID = table.Column<int>(type: "int", nullable: false),
                    strRoleID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    strPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    strEmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strFullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strDepartment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strStaffID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    dtLastActivityTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    intEnable = table.Column<int>(type: "int", nullable: true),
                    strSelectedAreaID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbCharacteristic",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strCharacteristicName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    intMeaTypeID = table.Column<int>(type: "int", nullable: true),
                    intProcessID = table.Column<int>(type: "int", nullable: true),
                    intCharacteristicType = table.Column<int>(type: "int", nullable: true),
                    strCharacteristicUnit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    boolDeleted = table.Column<bool>(type: "bit", nullable: true),
                    intDefectRateLimit = table.Column<int>(type: "int", nullable: true),
                    intEventEnable = table.Column<int>(type: "int", nullable: true),
                    intEmailEventModel = table.Column<int>(type: "int", nullable: true),
                    intDecimals = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbCharacteristic", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbEmailServer",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strEmailAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strDisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strSMTPHost = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    intSMTPPort = table.Column<int>(type: "int", nullable: true),
                    boolEnabledSSL = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbEmailServer", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbEventLog",
                columns: table => new
                {
                    intEventID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dtEventTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    strEventCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    strEvent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    strStation = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbEventLog", x => x.intEventID);
                });

            migrationBuilder.CreateTable(
                name: "tbEventRoles",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intCharacteristicID = table.Column<int>(type: "int", nullable: true),
                    strRoleID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbEventRoles", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbInspectionPlan",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strInspPlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    intAreaID = table.Column<int>(type: "int", nullable: true),
                    dtCreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    dtUpdateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    boolDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbInspectionPlan", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbInspectionPlanData",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intInspPlanSubID = table.Column<int>(type: "int", nullable: true),
                    intCharacteristicID = table.Column<int>(type: "int", nullable: true),
                    ftLSL = table.Column<double>(type: "float", nullable: true),
                    ftUSL = table.Column<double>(type: "float", nullable: true),
                    ftLCL = table.Column<double>(type: "float", nullable: true),
                    ftUCL = table.Column<double>(type: "float", nullable: true),
                    boolSPCChart = table.Column<bool>(type: "bit", nullable: true),
                    boolDataEntry = table.Column<bool>(type: "bit", nullable: true),
                    intPlanState = table.Column<int>(type: "int", nullable: true),
                    ftCpkMax = table.Column<double>(type: "float", nullable: true),
                    ftCpkMin = table.Column<double>(type: "float", nullable: true),
                    boolSpkControl = table.Column<bool>(type: "bit", nullable: true),
                    strSampleSize = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ftPercentControlLimit = table.Column<double>(type: "float", nullable: true),
                    boolDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbInspectionPlanData", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbInspectionPlanSub",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intInspPlanID = table.Column<int>(type: "int", nullable: false),
                    intPlanTypeID = table.Column<int>(type: "int", nullable: false),
                    boolDeleted = table.Column<bool>(type: "bit", nullable: false),
                    dtCreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbInspectionPlanSub", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbInspectionPlanTracking",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intInspPlanID = table.Column<int>(type: "int", nullable: true),
                    intCharacteristicID = table.Column<int>(type: "int", nullable: true),
                    intPlanState = table.Column<int>(type: "int", nullable: true),
                    dtCreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    intUserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbInspectionPlanTracking", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbInspectionPlanType",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strPlanTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbInspectionPlanType", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbJobData",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intAreaID = table.Column<int>(type: "int", nullable: true),
                    intProductID = table.Column<int>(type: "int", nullable: true),
                    strJobCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strPOCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strSOCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    intJobQty = table.Column<int>(type: "int", nullable: true),
                    intOutputQty = table.Column<int>(type: "int", nullable: true),
                    dtCreateTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    intJobDecisionID = table.Column<int>(type: "int", nullable: true),
                    intUserID = table.Column<int>(type: "int", nullable: true),
                    boolDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbJobData", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbJobDecision",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strDecision = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    intColorCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbJobDecision", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbMeasData3_01",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intCharacteristicID = table.Column<int>(type: "int", nullable: true),
                    varCharacteristicValue = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    varCharacteristicRange = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    dtTimeStamp = table.Column<DateTime>(type: "datetime", nullable: true),
                    intDataCollection = table.Column<int>(type: "int", nullable: true),
                    dtTimeMeasure = table.Column<DateTime>(type: "datetime", nullable: true),
                    intJobID = table.Column<int>(type: "int", nullable: true),
                    intProductionID = table.Column<int>(type: "int", nullable: true),
                    intLineID = table.Column<int>(type: "int", nullable: true),
                    intUserID = table.Column<int>(type: "int", nullable: true),
                    intOutputQty = table.Column<int>(type: "int", nullable: true),
                    intSampleIndex = table.Column<int>(type: "int", nullable: true),
                    intOKNG = table.Column<int>(type: "int", nullable: true),
                    intEmailSent = table.Column<int>(type: "int", nullable: true),
                    intPlanTypeID = table.Column<int>(type: "int", nullable: true),
                    strOutputNotes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    intSampleQty = table.Column<int>(type: "int", nullable: true),
                    intMoldID = table.Column<int>(type: "int", nullable: true),
                    intCavityID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbMeasData3_01", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbMeasureType",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strMeaType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbMeasureType", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbProcess",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strProcessName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    intAreaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbProcess", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbProcessLine",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strProcessLineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strProcessLineCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    intAreaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbProcessLine", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbProductionArea",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strNameArea = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbProductionArea", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbProductionData",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intLineID = table.Column<int>(type: "int", nullable: true),
                    dtStartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    dtEndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    intJobID = table.Column<int>(type: "int", nullable: true),
                    boolDeleted = table.Column<bool>(type: "bit", nullable: true),
                    intUserID = table.Column<int>(type: "int", nullable: true),
                    strNotes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    dtProductionDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    intProductionQty = table.Column<int>(type: "int", nullable: true),
                    strLotInform = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strMaterialInform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    intCNCLatheMachine = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbProductionData", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbProductName",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strNameProduct = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    intAreaID = table.Column<int>(type: "int", nullable: true),
                    intInspPlanID = table.Column<int>(type: "int", nullable: true),
                    boolDeleted = table.Column<bool>(type: "bit", nullable: false),
                    strModelInternal = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strCustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strNotes = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    intVialFixture = table.Column<int>(type: "int", nullable: true),
                    strDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    intMoldQty = table.Column<int>(type: "int", nullable: true),
                    intCavityQty = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbProductName", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbShift",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StrNameShift = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    dtStartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    dtEndTime = table.Column<TimeSpan>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbShift", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbTVDisplay",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strTVName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    strProductionID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    strCharacteristicID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    strPlanTypeID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    strMoldID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    strCavityID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbTVDisplay", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "tbWebSession",
                columns: table => new
                {
                    intID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    strSessionID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    strIpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dtStartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    dtEndTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbWebSession", x => x.intID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "tbCharacteristic");

            migrationBuilder.DropTable(
                name: "tbEmailServer");

            migrationBuilder.DropTable(
                name: "tbEventLog");

            migrationBuilder.DropTable(
                name: "tbEventRoles");

            migrationBuilder.DropTable(
                name: "tbInspectionPlan");

            migrationBuilder.DropTable(
                name: "tbInspectionPlanData");

            migrationBuilder.DropTable(
                name: "tbInspectionPlanSub");

            migrationBuilder.DropTable(
                name: "tbInspectionPlanTracking");

            migrationBuilder.DropTable(
                name: "tbInspectionPlanType");

            migrationBuilder.DropTable(
                name: "tbJobData");

            migrationBuilder.DropTable(
                name: "tbJobDecision");

            migrationBuilder.DropTable(
                name: "tbMeasData3_01");

            migrationBuilder.DropTable(
                name: "tbMeasureType");

            migrationBuilder.DropTable(
                name: "tbProcess");

            migrationBuilder.DropTable(
                name: "tbProcessLine");

            migrationBuilder.DropTable(
                name: "tbProductionArea");

            migrationBuilder.DropTable(
                name: "tbProductionData");

            migrationBuilder.DropTable(
                name: "tbProductName");

            migrationBuilder.DropTable(
                name: "tbShift");

            migrationBuilder.DropTable(
                name: "tbTVDisplay");

            migrationBuilder.DropTable(
                name: "tbWebSession");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropSequence(
                name: "KnowledgeBaseSepence");
        }
    }
}
