using System.ComponentModel.DataAnnotations;

namespace proyectofinalQ2.DTOs;

public class RegisterDto
{
    // lo que el fronted manda cuando alguien se quiere registrar
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    public string FullName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato de correo es invalido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La contraseña es obligatorio")]
    [StringLength(100,MinimumLength=8,ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
    public string Password { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La direccion es obligatorio")]
    public string Address { get; set; } = string.Empty;
    
    public string Role { get; set; } = "ciudadano";
}

