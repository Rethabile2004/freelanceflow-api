using System.Security.Claims;
using FreelanceFlow.API.Data;
using FreelanceFlow.API.DTOs.Common;
using FreelanceFlow.API.DTOs.Project;
using FreelanceFlow.API.Exceptions;
using FreelanceFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelanceFlow.API.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ProjectService(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        private string GetUserId() =>
            _httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedException("User not authenticated.");

        public async Task<PagedResult<ProjectResponseDto>> GetAllAsync(ProjectQueryDto query)
        {
            var userId = GetUserId();

            var queryable = _context.Projects
                .Where(p => p.Client!.UserId == userId);

            if (query.Status.HasValue)
                queryable = queryable.Where(p => p.Status == query.Status.Value);

            if (query.ClientId.HasValue)
                queryable = queryable.Where(p => p.ClientId == query.ClientId.Value);

            if (!string.IsNullOrWhiteSpace(query.Search))
                queryable = queryable.Where(p => p.Name.Contains(query.Search));

            var totalCount = await queryable.CountAsync();

            var items = await queryable
                .OrderByDescending(p => p.StartDate)
                .ThenByDescending(p => p.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    DueDate = p.DueDate,
                    Budget = p.Budget,
                    Status = p.Status,
                    ClientId = p.ClientId,
                    ClientCompanyName = p.Client!.CompanyName,
                    TaskCount = p.Tasks.Count,
                    InvoiceCount = p.Invoices.Count
                })
                .ToListAsync();

            return new PagedResult<ProjectResponseDto>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ProjectResponseDto> GetByIdAsync(int id)
        {
            var userId = GetUserId();

            return await _context.Projects
                .Where(p => p.Id == id && p.Client!.UserId == userId)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    DueDate = p.DueDate,
                    Budget = p.Budget,
                    Status = p.Status,
                    ClientId = p.ClientId,
                    ClientCompanyName = p.Client!.CompanyName,
                    TaskCount = p.Tasks.Count,
                    InvoiceCount = p.Invoices.Count
                })
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException("Project not found.");
        }

        public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto)
        {
            var userId = GetUserId();

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == dto.ClientId && c.UserId == userId)
                ?? throw new NotFoundException("Client not found.");

            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                DueDate = dto.DueDate,
                Budget = dto.Budget,
                ClientId = dto.ClientId,
                Status = ProjectStatus.Active
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                DueDate = project.DueDate,
                Budget = project.Budget,
                Status = project.Status,
                ClientId = project.ClientId,
                ClientCompanyName = client.CompanyName,
                TaskCount = 0,
                InvoiceCount = 0
            };
        }

        public async Task<ProjectResponseDto> UpdateAsync(int id, UpdateProjectDto dto)
        {
            var userId = GetUserId();

            var project = await _context.Projects
                .Include(p => p.Client)
                .FirstOrDefaultAsync(p => p.Id == id && p.Client!.UserId == userId)
                ?? throw new NotFoundException("Project not found.");

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.StartDate = dto.StartDate;
            project.DueDate = dto.DueDate;
            project.Budget = dto.Budget;
            project.Status = dto.Status;

            await _context.SaveChangesAsync();

            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                DueDate = project.DueDate,
                Budget = project.Budget,
                Status = project.Status,
                ClientId = project.ClientId,
                ClientCompanyName = project.Client!.CompanyName,
                TaskCount = await _context.Tasks.CountAsync(t => t.ProjectId == project.Id),
                InvoiceCount = await _context.Invoices.CountAsync(i => i.ProjectId == project.Id)
            };
        }

        public async Task DeleteAsync(int id)
        {
            var userId = GetUserId();

            var project = await _context.Projects
                .Include(p => p.Tasks)
                .Include(p => p.Invoices)
                .FirstOrDefaultAsync(p => p.Id == id && p.Client!.UserId == userId)
                ?? throw new NotFoundException("Project not found.");

            if (project.Tasks.Count > 0 || project.Invoices.Count > 0)
                throw new BadRequestException(
                    "Cannot delete a project that still has tasks or invoices. Remove them first.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}