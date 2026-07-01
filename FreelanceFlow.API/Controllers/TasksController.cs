using FreelanceFlow.API.DTOs.Task;
using FreelanceFlow.API.Models;
using FreelanceFlow.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceFlow.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] Models.TaskStatus? status,
            [FromQuery] TaskPriority? priority,
            [FromQuery] int? projectId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var query = new TaskQueryDto
            {
                Status = status,
                Priority = priority,
                ProjectId = projectId,
                Page = page,
                PageSize = pageSize
            };

            var result = await _taskService.GetAllAsync(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskDto dto)
        {
            var task = await _taskService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TaskResponseDto>> Update(int id, UpdateTaskDto dto)
        {
            var task = await _taskService.UpdateAsync(id, dto);
            return Ok(task);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.DeleteAsync(id);
            return NoContent();
        }
    }
}