using FreelanceFlow.API.DTOs.Client;

namespace FreelanceFlow.API.Services
{
    public interface IClientService
    {
        Task<List<ClientResponseDto>> GetAllAsync();
        Task<ClientResponseDto> GetByIdAsync(int id);
        Task<ClientResponseDto> CreateAsync(CreateClientDto dto);
        Task<ClientResponseDto> UpdateAsync(int id, UpdateClientDto dto);
        Task DeleteAsync(int id);
    }
}