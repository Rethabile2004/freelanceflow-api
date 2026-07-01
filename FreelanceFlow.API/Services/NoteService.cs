using System.Security.Claims;
using FreelanceFlow.API.Data;
using FreelanceFlow.API.DTOs.Note;
using FreelanceFlow.API.Exceptions;
using FreelanceFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelanceFlow.API.Services
{
    public class NoteService : INoteService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public NoteService(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        private string GetUserId() =>
            _httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedException("User not authenticated.");

        public async Task<List<NoteResponseDto>> GetByClientIdAsync(int clientId)
        {
            var userId = GetUserId();
            
            var clientExists = await _context.Clients
                .AnyAsync(c => c.Id == clientId && c.UserId == userId);

            if (!clientExists)
                throw new NotFoundException("Client not found.");

            return await _context.Notes
                .Where(n => n.ClientId == clientId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NoteResponseDto
                {
                    Id = n.Id,
                    Content = n.Content,
                    CreatedAt = n.CreatedAt,
                    ClientId = n.ClientId
                })
                .ToListAsync();
        }

        public async Task<NoteResponseDto> CreateAsync(CreateNoteDto dto)
        {
            var userId = GetUserId();

            var clientExists = await _context.Clients
                .AnyAsync(c => c.Id == dto.ClientId && c.UserId == userId);

            if (!clientExists)
                throw new NotFoundException("Client not found.");

            var note = new Note
            {
                Content = dto.Content,
                ClientId = dto.ClientId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return new NoteResponseDto
            {
                Id = note.Id,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                ClientId = note.ClientId
            };
        }

        public async Task<NoteResponseDto> UpdateAsync(int id, UpdateNoteDto dto)
        {
            var userId = GetUserId();

            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.Client!.UserId == userId)
                ?? throw new NotFoundException("Note not found.");

            note.Content = dto.Content;
            await _context.SaveChangesAsync();

            return new NoteResponseDto
            {
                Id = note.Id,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                ClientId = note.ClientId
            };
        }

        public async Task DeleteAsync(int id)
        {
            var userId = GetUserId();

            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.Client!.UserId == userId)
                ?? throw new NotFoundException("Note not found.");

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
        }
    }
}