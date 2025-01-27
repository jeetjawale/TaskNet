// TasksController.cs
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Backend.Exceptions;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirst("userId")?.Value ?? throw new UnauthorizedException("Invalid token"));

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedTasksResponseDto))]
        public async Task<ActionResult<PaginatedTasksResponseDto>> GetTasks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            return await _taskService.GetUserTasks(GetUserId(), page, pageSize);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TaskResponseDto))]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(TaskCreateDto dto)
        {
            var task = await _taskService.CreateTask(dto, GetUserId());
            return CreatedAtAction(nameof(CreateTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskResponseDto))]
        public async Task<ActionResult<TaskResponseDto>> UpdateTask(int id, TaskCreateDto dto)
        {
            return await _taskService.UpdateTask(id, dto, GetUserId());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTask(id, GetUserId());
            return NoContent();
        }
    }
}