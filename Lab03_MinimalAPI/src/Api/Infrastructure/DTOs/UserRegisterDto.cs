using System.ComponentModel.DataAnnotations;

namespace Lab03_MinimalAPI.DTOs;

public record UserRegisterDto(
    [Required, StringLength(50)] string Username,
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password);
