using System.ComponentModel.DataAnnotations;

namespace ProyectoClaseQ2.DTOs;

public class LoginDto
{
    // lo que el fronted manda cuando alguien quiere entrar
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    
    
}