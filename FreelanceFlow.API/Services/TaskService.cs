using System.Security.Claims;
using FreelanceFlow.API.Data;
using FreelanceFlow.API.DTOs.Common;
using FreelanceFlow.API.DTOs.Task;
using FreelanceFlow.API.Exceptions;
using FreelanceFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelanceFlow.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public TaskService(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        private string GetUserId() =>
            _httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedException("User not authenticated.");

        public async Task<PagedResult<TaskResponseDto>> GetAllAsync(TaskQueryDto query)
        {
            var userId = GetUserId();

            var queryable = _context.Tasks
                .Where(t => t.Project!.Client!.UserId == userId);

            if (query.Status.HasValue)
                queryable = queryable.Where(t => t.Status == query.Status.Value);

            if (query.Priority.HasValue)
                queryable = queryable.Where(t => t.Priority == query.Priority.Value);

            if (query.ProjectId.HasValue)
                queryable = queryable.Where(t => t.ProjectId == query.ProjectId.Value);

            var totalCount = await queryable.CountAsync();

            var items = await queryable
                .OrderByDescending(t => t.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    EstimatedHours = t.EstimatedHours,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project!.Name
                })
                .ToListAsync();

            return new PagedResult<TaskResponseDto>
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<TaskResponseDto> GetByIdAsync(int id)
        {
            var userId = GetUserId();

            return await _context.Tasks
                .Where(t => t.Id == id && t.Project!.Client!.UserId == userId)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Priority = t.Priority,
                    Status = t.Status,
                    EstimatedHours = t.EstimatedHours,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project!.Name
                })
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException("Task not found.");
        }

        public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
        {
            var userId = GetUserId();

            var project = await _context.Projects
                .Include(p => p.Client)
                .FirstOrDefaultAsync(p => p.Id == dto.ProjectId && p.Client!.UserId == userId)
                ?? throw new NotFoundException("Project not found.");

            var task = new ProjectTask
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                EstimatedHours = dto.EstimatedHours,
                ProjectId = dto.ProjectId,
                Status = Models.TaskStatus.ToDo
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                EstimatedHours = task.EstimatedHours,
                ProjectId = task.ProjectId,
                ProjectName = project.Name
            };
        }

        public async Task<TaskResponseDto> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var userId = GetUserId();

            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id && t.Project!.Client!.UserId == userId)
                ?? throw new NotFoundException("Task not found.");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Priority = dto.Priority;
            task.Status = dto.Status;
            task.EstimatedHours = dto.EstimatedHours;

            await _context.SaveChangesAsync();

            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Priority = task.Priority,
                Status = task.Status,
                EstimatedHours = task.EstimatedHours,
                ProjectId = task.ProjectId,
                ProjectName = task.Project!.Name
            };
        }

        public async Task DeleteAsync(int id)
        {
            var userId = GetUserId();

            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.Project!.Client!.UserId == userId)
                ?? throw new NotFoundException("Task not found.");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}