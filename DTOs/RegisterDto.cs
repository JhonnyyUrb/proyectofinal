namespace ProyectoClaseQ2.DTOs;

public class RegisterDto
{
    // lo que el fronted manda cuando alguien se quiere registrar
    
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    public string Direccion { get; set; } = string.Empty;
}