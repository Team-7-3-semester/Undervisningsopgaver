namespace Undervisningsopgaver.DTOs;

public record RegisterDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateTime? BirthDate,
    string? Department);

public record LoginDto(string Email, string Password);
