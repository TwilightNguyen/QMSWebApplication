using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using QMSWebApplication.BackendServer.Controllers;
using QMSWebApplication.BackendServer.Data;
using QMSWebApplication.BackendServer.Data.Entities;
using QMSWebApplication.ViewModels;
using Microsoft.Extensions.Options;
using QMSWebApplication.ViewModels.System.Job;

namespace QMSWebApplication.BackendServer.UnitTest.Controllers
{
    public class JobsControllerTest : IAsyncLifetime
    {
        public required ApplicationDbContext _context;

        public async Task DisposeAsync()
        {
            try { await _context.Database.EnsureDeletedAsync();  } catch { }
            await _context.DisposeAsync();
        }

        public async Task InitializeAsync()
        {
            _context = InMemoryDbContext.GetApplicationDbContext();

            InMemoryDbContext.SeedUsers(_context);
            InMemoryDbContext.SeedRoles(_context);
            InMemoryDbContext.SeedProductionAreas(_context);
            InMemoryDbContext.SeedProcesses(_context);
            InMemoryDbContext.SeedProcessLines(_context);
            InMemoryDbContext.SeedCharacteristics(_context);
            InMemoryDbContext.SeedEventLogs(_context);
            InMemoryDbContext.SeedInspPlanTypes(_context);
            InMemoryDbContext.SeedInspectionPlans(_context);
            InMemoryDbContext.SeedInspectionPlanSubs(_context);
            InMemoryDbContext.SeedInspectionPlanDatas(_context);
            InMemoryDbContext.SeedProducts(_context);
            InMemoryDbContext.SeedJobDecisions(_context);
            InMemoryDbContext.SeedJobs(_context);

            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task ShouldCreateInstance_NotNull_Success()
        {
            // Act
            var controller = new JobsController(_context);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task GetAll_ValidData_Success()
        {
            var controller = new JobsController(_context);

            // Act
            var result = await controller.GetAll();

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var jobs = okResult?.Value as IEnumerable<JobVm>;
            Assert.NotNull(jobs);
            Assert.Equal(4, jobs.Count());
            Assert.Contains(jobs!, a => a.JobCode == "Job Code 1");
            Assert.Contains(jobs!, a => a.JobCode == "Job Code 2");
            Assert.Contains(jobs!, a => a.JobCode == "Job Code 3");
            Assert.Contains(jobs!, a => a.JobCode == "Job Code 4");
        }

        [Fact]
        public async Task GetById_HasData_Success()
        {
            var controller = new JobsController (_context);

            // Act
            var result = await controller.GetById(1);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var job = okResult?.Value as JobVm;
            Assert.NotNull(job);
            Assert.Equal(1, job.Id);
        }

        [Fact]
        public async Task GetById_NotFound_Failure()
        {
            var controller = new JobsController(_context);

            // Act
            var result = await controller.GetById(999);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult?.StatusCode);
            Assert.Equal("Job not found.", notFoundResult?.Value);
        }

        [Fact] 
        public async Task GetPaging_NoFilter_Sucess()
        {
            var controller = new JobsController(_context);
            // Act
            var result = await controller.GetPaging(null, 1, 2);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var pagedResult = okResult?.Value as Pagination<JobVm>;
            Assert.NotNull(pagedResult);
            Assert.Equal(4, pagedResult?.TotalRecords);
            Assert.Equal(2, pagedResult?.Items.Count);
        }

        [Fact]
        public async Task GetPaging_WithFilter_Success()
        {
            var controller = new JobsController(_context);

            // Act 
            var result = await controller.GetPaging("1", 1, 2);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var pagedResult = okResult?.Value as Pagination<JobVm>;

            Assert.NotNull(pagedResult);
            Assert.Equal(1, pagedResult?.TotalRecords);
            Assert.Equal(1, pagedResult?.Items.Count);
            Assert.Contains(pagedResult?.Items!, j => j.JobCode == "Job Code 1");
        }

        [Fact]
        public async Task CreateJob_ValidData_Success()
        {
            var controller = new JobsController(_context);

            var request = new JobCreateRequest
            {
                AreaId = 1,
                ProductId = 1,
                JobCode = "Job Code 1 New",
                POCode = "PO Code 1 New",
                SOCode = "SO Code 1 New",
                PlannedQuantity = 1000,
                OutputQuanlity = 1000,
            };

            // Act
            var result = await controller.CreateJob(request);

            // Assert
            Assert.NotNull(result);
            var createResult = result as CreatedAtActionResult;
            Assert.NotNull(createResult);
            var jobVm = createResult?.Value as JobVm;
            Assert.NotNull(jobVm);
            Assert.Equal("Job Code 1 New", jobVm?.JobCode);
            Assert.Equal("PO Code 1 New", jobVm?.POCode);
            Assert.Equal("SO Code 1 New", jobVm?.SOCode);
            Assert.Equal(1000, jobVm?.PlannedQuanlity);
            Assert.Equal(1000, jobVm?.OutputQuanlity);
            Assert.Equal(1, jobVm?.AreaId);
            Assert.Equal(1, jobVm?.ProductId);
        }

        [Fact]
        public async Task CreateJob_JobCodeNotBeEmpty_Failre()
        {
            var controller = new JobsController(_context);
            
            var request = new JobCreateRequest
            {
                AreaId = 1,
                ProductId = 1,
                JobCode = string.Empty,
                POCode = "PO Code 2 New",
                SOCode = "SO Code 2 New",
                PlannedQuantity = 1000,
                OutputQuanlity = 1000,
            };
            // Act
            var result = await controller.CreateJob(request);

            // Assert
            Assert.NotNull(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.Equal(400, badRequest?.StatusCode);
            Assert.Equal("Job Code cannot be empty.", badRequest?.Value);
        }

        [Fact]
        public async Task CreateJob_InvalidProductionArea_Failre()
        {
            var controller = new JobsController(_context);

            var request = new JobCreateRequest
            {
                AreaId = 999,
                ProductId = 1,
                JobCode = "Job Code 3 New",
                POCode = "PO Code 3 New",
                SOCode = "SO Code 3 New",
                PlannedQuantity = 1000,
                OutputQuanlity = 1000,
            };

            // Act
            var result = await controller.CreateJob(request);

            // Assert
            Assert.NotNull(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.Equal(400, badRequest?.StatusCode);
            Assert.Equal("Invalid Production Area.", badRequest?.Value);
        }

        [Fact]
        public async Task CreateJob_InvalidProduct_Failre()
        {
            var controller = new JobsController(_context);

            var request = new JobCreateRequest
            {
                AreaId = 1,
                ProductId = 999,
                JobCode = "Job Code 4 New",
                POCode = "PO Code 4 New",
                SOCode = "SO Code 4 New",
                PlannedQuantity = 1000,
                OutputQuanlity = 1000,
            };

            // Act
            var result = await controller.CreateJob(request);

            // Assert
            Assert.NotNull(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.Equal(400, badRequest?.StatusCode);
            Assert.Equal("Invalid Product.", badRequest?.Value);
        }

        [Fact]
        public async Task CreateJob_DuplicateJobCode_Failre()
        {
            var controller = new JobsController(_context);

            var request = new JobCreateRequest
            {
                AreaId = 1,
                ProductId = 1,
                JobCode = "Job Code 1",
                POCode = "PO Code 5 New",
                SOCode = "SO Code 5 New",
                PlannedQuantity = 1000,
                OutputQuanlity = 1000,
            };

            // Act
            var result = await controller.CreateJob(request);

            // Assert
            Assert.NotNull(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.NotNull(badRequest);
            Assert.Equal(400, badRequest?.StatusCode);
            Assert.Equal("Job with the same Job Code already exists in this Production Area.", badRequest?.Value);
        }

        [Fact]
        public async Task UpdateJob_ValidData_Success()
        {
            var controller = new JobsController(_context);

            var request = new JobVm
            {
                Id = 1,
                AreaId = 1,
                ProductId = 1,
                JobCode = "Job Code 1 Update",
                POCode = "PO Code 1 Update",
                SOCode = "SO Code 1 Update",
                PlannedQuanlity = 2000,
                OutputQuanlity = 3000,
                JobDecisionId = 2,
            };

            // Act
            var result = await controller.UpdateJob(1, request);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var job = okResult?.Value as JobVm;
            Assert.NotNull(job);
            Assert.Equal(1, job.AreaId);
            Assert.Equal(1, job.ProductId);
            Assert.Equal("Job Code 1 Update", job.JobCode);
            Assert.Equal("PO Code 1 Update", job.POCode);
            Assert.Equal("SO Code 1 Update", job.SOCode);
            Assert.Equal(2000, job.PlannedQuanlity);
            Assert.Equal(3000, job.OutputQuanlity);
        }

        [Fact]
        public async Task UpdateJob_NotFound_Failure()
        {
            var controller = new JobsController(_context);

            var request = new JobVm
            {
                Id = 999,
                AreaId = 1,
                ProductId = 1,
                JobCode = "Job Code 2 Update",
                POCode = "PO Code 2 Update",
                SOCode = "SO Code 2 Update",
                PlannedQuanlity = 2000,
                OutputQuanlity = 3000,
                JobDecisionId = 2,
            };

            // Act
            var result = await controller.UpdateJob(999, request);

            // Assert
            Assert.NotNull(result);
            var notfoundRequest = result as NotFoundObjectResult;
            Assert.Equal(404, notfoundRequest?.StatusCode);
            Assert.Equal("Job not found.", notfoundRequest?.Value);
        }

        [Fact]
        public async Task UpdateJob_InvalidId_Failure()
        {
            var controller = new JobsController(_context);

            var request = new JobVm
            {
                Id = 2,
                AreaId = 1,
                ProductId = 1,
                JobCode = "Job Code 2 Update",
                POCode = "PO Code 2 Update",
                SOCode = "SO Code 2 Update",
                PlannedQuanlity = 2000,
                OutputQuanlity = 3000,
                JobDecisionId = 2,
            };

            // Act
            var result = await controller.UpdateJob(999, request);

            // Assert
            Assert.NotNull(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.Equal(400, badRequest?.StatusCode);
            Assert.Equal("Invalid Job Id.", badRequest?.Value);
        }

        [Fact]
        public async Task UpdateJob_DuplicateJobCode_Failure()
        {
            var controller = new JobsController(_context);

            var request = new JobVm
            {
                Id = 3,
                AreaId = 2,
                ProductId = 3,
                JobCode = "Job Code 3",
                POCode = "PO Code 2 Update",
                SOCode = "SO Code 2 Update",
                PlannedQuanlity = 2000,
                OutputQuanlity = 3000,
                JobDecisionId = 2,
            };

            // Act
            var result = await controller.UpdateJob(3, request);

            // Assert
            Assert.NotNull(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.Equal(400, badRequest?.StatusCode);
            Assert.Equal("Job with the same Job Code already exists in this Production Area.", badRequest?.Value);
        }

        [Fact]
        public async Task DeleteJob_ValidId_Success()
        {
            var controller = new JobsController (_context);

            // Act 
            var result = await controller.DeleteJob(4);
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            var job = okResult?.Value as JobVm;
            Assert.NotNull(job);
            Assert.Equal(4, job.Id);
        }

        [Fact]
        public async Task DeleteJob_NonExistentId_Success() { 
            var controller = new JobsController (_context);

            // Act
            var result = await controller.DeleteJob(999);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.Equal(404, notFoundResult?.StatusCode);
            Assert.Equal("Job not found.", notFoundResult?.Value);
        }
    }
}
