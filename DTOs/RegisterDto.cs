using System.ComponentModel.DataAnnotations;

namespace ProyectoClaseQ2.DTOs;

public class RegisterDto
{
    // lo que el fronted manda cuando alguien se quiere registrar
    [Required]
    public string FullName { get; set; } = string.Empty;
    [Required]
    [EmailAdress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [StringLength(100,MinimumLength=8)]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Address { get; set; } = string.Empty;
}