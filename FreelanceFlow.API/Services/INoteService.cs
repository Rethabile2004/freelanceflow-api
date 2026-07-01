using FreelanceFlow.API.DTOs.Note;

namespace FreelanceFlow.API.Services
{
    public interface INoteService
    {
        Task<List<NoteResponseDto>> GetByClientIdAsync(int clientId);
        Task<NoteResponseDto> CreateAsync(CreateNoteDto dto);
        Task<NoteResponseDto> UpdateAsync(int id, UpdateNoteDto dto);
        Task DeleteAsync(int id);
    }
}