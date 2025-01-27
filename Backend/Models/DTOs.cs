namespace Backend.Models
{
    public record RegisterUserDto(string Email, string Password);
    public record LoginDto(string Email, string Password);
    public record AuthResponse(string Token, string Email);
    public record ForgotPasswordDto(string Email);
    public record ResetPasswordDto(string Token, string NewPassword);

    public record TaskCreateDto(
        string Title,
        string Description,
        DateTime DueDate,
        PriorityLevel Priority,
        List<string> Tags);

    public record TaskResponseDto(
        int Id,
        string Title,
        string Description,
        bool IsCompleted,
        DateTime CreatedAt,
        DateTime DueDate,
        string Priority,
        List<string> Tags);

    public record PaginatedTasksResponseDto(
        List<TaskResponseDto> Tasks,
        int TotalItems,
        int PageNumber,
        int PageSize,
        int TotalPages);
}