using System.Security.Claims;
using FreelanceFlow.API.Data;
using FreelanceFlow.API.DTOs.Client;
using FreelanceFlow.API.Exceptions;
using FreelanceFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FreelanceFlow.API.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public ClientService(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        private string GetUserId() =>
            _httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedException("User not authenticated.");

        public async Task<List<ClientResponseDto>> GetAllAsync()
        {
            var userId = GetUserId();

            return await _context.Clients
                .Where(c => c.UserId == userId)
                .Select(c => new ClientResponseDto
                {
                    Id = c.Id,
                    CompanyName = c.CompanyName,
                    ContactName = c.ContactName,
                    Email = c.Email,
                    Phone = c.Phone,
                    ProjectCount = c.Projects.Count
                })
                .ToListAsync();
        }

        public async Task<ClientResponseDto> GetByIdAsync(int id)
        {
            var userId = GetUserId();

            return await _context.Clients
                .Where(c => c.Id == id && c.UserId == userId)
                .Select(c => new ClientResponseDto
                {
                    Id = c.Id,
                    CompanyName = c.CompanyName,
                    ContactName = c.ContactName,
                    Email = c.Email,
                    Phone = c.Phone,
                    ProjectCount = c.Projects.Count
                })
                .FirstOrDefaultAsync()
                ?? throw new NotFoundException("Client not found.");
        }

        public async Task<ClientResponseDto> CreateAsync(CreateClientDto dto)
        {
            var userId = GetUserId();

            var client = new Client
            {
                CompanyName = dto.CompanyName,
                ContactName = dto.ContactName,
                Email = dto.Email,
                Phone = dto.Phone,
                UserId = userId
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return new ClientResponseDto
            {
                Id = client.Id,
                CompanyName = client.CompanyName,
                ContactName = client.ContactName,
                Email = client.Email,
                Phone = client.Phone,
                ProjectCount = 0
            };
        }

        public async Task<ClientResponseDto> UpdateAsync(int id, UpdateClientDto dto)
        {
            var userId = GetUserId();

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId)
                ?? throw new NotFoundException("Client not found.");

            client.CompanyName = dto.CompanyName;
            client.ContactName = dto.ContactName;
            client.Email = dto.Email;
            client.Phone = dto.Phone;

            await _context.SaveChangesAsync();

            return new ClientResponseDto
            {
                Id = client.Id,
                CompanyName = client.CompanyName,
                ContactName = client.ContactName,
                Email = client.Email,
                Phone = client.Phone,
                ProjectCount = await _context.Projects.CountAsync(p => p.ClientId == client.Id)
            };
        }

        public async Task DeleteAsync(int id)
        {
            var userId = GetUserId();

            var client = await _context.Clients
                .Include(c => c.Projects)
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId)
                ?? throw new NotFoundException("Client not found.");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }
}