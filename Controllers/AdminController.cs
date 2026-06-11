using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyectofinalQ2.DTOs;
using proyectofinalQ2.Services;

namespace proyectofinalQ2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;

    public AdminController(AdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPut("asignar-zona")]
    public async Task<IActionResult> AsignarZona([FromBody] AsignarZona dto)
    {
        try
        {
            await _adminService.AsignarZonaMediador(dto);
            return Ok("Zona asignada correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("usuarios/{userId}/alternar-estado")]
    public async Task<IActionResult> AlternarEstadoUsuario(string userId)
    {
        try
        {
            await _adminService.AlternarEstadoUsuario(userId);
            return Ok("Estado del usuario actualizado correctamente");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}