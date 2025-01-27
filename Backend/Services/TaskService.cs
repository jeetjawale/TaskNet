using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Exceptions;

namespace Backend.Services
{
    public interface ITaskService
    {
        Task<PaginatedTasksResponseDto> GetUserTasks(int userId, int pageNumber, int pageSize);
        Task<TaskResponseDto> CreateTask(TaskCreateDto dto, int userId);
        Task<TaskResponseDto> UpdateTask(int taskId, TaskCreateDto dto, int userId);
        Task DeleteTask(int taskId, int userId);
    }

    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context) => _context = context;

        public async Task<PaginatedTasksResponseDto> GetUserTasks(int userId, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _context.TaskItems
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.CreatedAt);

            var totalItems = await query.CountAsync();
            var tasks = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => MapToDto(t))
                .ToListAsync();

            return new PaginatedTasksResponseDto(
                tasks,
                totalItems,
                pageNumber,
                pageSize,
                (int)Math.Ceiling(totalItems / (double)pageSize));
        }

        public async Task<TaskResponseDto> CreateTask(TaskCreateDto dto, int userId)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                Tags = string.Join(",", dto.Tags),
                UserId = userId
            };

            _context.TaskItems.Add(task);
            await _context.SaveChangesAsync();
            return MapToDto(task);
        }

        public async Task<TaskResponseDto> UpdateTask(int taskId, TaskCreateDto dto, int userId)
        {
            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId)
                ?? throw new NotFoundException("Task not found");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;
            task.Priority = dto.Priority;
            task.Tags = string.Join(",", dto.Tags);

            await _context.SaveChangesAsync();
            return MapToDto(task);
        }

        public async Task DeleteTask(int taskId, int userId)
        {
            var task = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId)
                ?? throw new NotFoundException("Task not found");

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
        }

        private static TaskResponseDto MapToDto(TaskItem task) => new(
            task.Id,
            task.Title,
            task.Description,
            task.IsCompleted,
            task.CreatedAt,
            task.DueDate,
            task.Priority.ToString(),
            task.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
        );
    }
}