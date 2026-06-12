using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyectofinalQ2.Services;

namespace proyectofinalQ2.Controllers;

[Authorize(Roles = "admin,Admin")]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;

    public AdminController(AdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("asignar-zona")]
    public async Task<IActionResult> AsignarZona([FromBody] AsignarZonaRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.ZoneId))
            {
                return BadRequest(new { message = "UserId y ZoneId son obligatorios." });
            }

            var resultado = await _adminService.AsignarZonaMediador(request.UserId, request.ZoneId);
            return Ok(new { message = "Zona asignada correctamente al mediador." });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("alternar-estado/{userId}")]
    public async Task<IActionResult> AlternarEstado(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "El ID de usuario es requerido." });
            }

            var nuevoEstado = await _adminService.AlternarEstadoUsuario(userId);
            return Ok(new { message = $"El estado del usuario se actualizó a: {(nuevoEstado ? "Activo" : "Inactivo")}", isActive = nuevoEstado });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}

public class AsignarZonaRequest
{
    public string UserId { get; set; } = string.Empty;
    public string ZoneId { get; set; } = string.Empty;
}
