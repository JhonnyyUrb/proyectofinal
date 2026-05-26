using Microsoft.AspNetCore.Mvc;
using ProyectoClaseQ2.DTOs;
using ProyectoClaseQ2.Services; //importar el namespace de la interfaz

namespace ProyectoClaseQ2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService; // Cambiamos a la interfaz

        
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var user = await _authService.Register(dto);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // ... método Login similar
    }
}