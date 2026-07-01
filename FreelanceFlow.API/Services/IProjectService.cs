using FreelanceFlow.API.DTOs.Common;
using FreelanceFlow.API.DTOs.Project;

namespace FreelanceFlow.API.Services
{
    public interface IProjectService
    {
        Task<PagedResult<ProjectResponseDto>> GetAllAsync(ProjectQueryDto query);
        Task<ProjectResponseDto> GetByIdAsync(int id);
        Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto);
        Task<ProjectResponseDto> UpdateAsync(int id, UpdateProjectDto dto);
        Task DeleteAsync(int id);
    }
}