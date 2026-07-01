using FreelanceFlow.API.DTOs.Common;
using FreelanceFlow.API.DTOs.Task;

namespace FreelanceFlow.API.Services
{
    public interface ITaskService
    {
        Task<PagedResult<TaskResponseDto>> GetAllAsync(TaskQueryDto query);
        Task<TaskResponseDto> GetByIdAsync(int id);
        Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
        Task<TaskResponseDto> UpdateAsync(int id, UpdateTaskDto dto);
        Task DeleteAsync(int id);
    }
}