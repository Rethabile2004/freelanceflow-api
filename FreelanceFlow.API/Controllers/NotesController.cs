using FreelanceFlow.API.DTOs.Note;
using FreelanceFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceFlow.API.Controllers
{
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("api/clients/{clientId}/notes")]
        public async Task<ActionResult<List<NoteResponseDto>>> GetByClient(int clientId)
        {
            var notes = await _noteService.GetByClientIdAsync(clientId);
            return Ok(notes);
        }

        [HttpPost("api/notes")]
        public async Task<ActionResult<NoteResponseDto>> Create(CreateNoteDto dto)
        {
            var note = await _noteService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByClient), new { clientId = note.ClientId }, note);
        }

        [HttpPut("api/notes/{id}")]
        public async Task<ActionResult<NoteResponseDto>> Update(int id, UpdateNoteDto dto)
        {
            var note = await _noteService.UpdateAsync(id, dto);
            return Ok(note);
        }

        [HttpDelete("api/notes/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _noteService.DeleteAsync(id);
            return NoContent();
        }
    }
}