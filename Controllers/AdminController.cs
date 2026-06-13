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
    
    [HttpGet("usuarios")]
    public async Task<IActionResult> ListarUsuarios()
    {
        try
        {
            var usuarios = await _adminService.ListarUsuarios();
            return Ok(usuarios);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet("mediadores")]
    public async Task<IActionResult> ListarMediadores()
    {
        try
        {
            var mediadores = await _adminService.ListarMediadores();
            return Ok(mediadores);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("desactivar-mediador/{userId}")]
    public async Task<IActionResult> DesactivarMediador(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "El ID del mediador es requerido." });
            }

            await _adminService.DesactivarMediador(userId);
            return Ok(new { message = "Mediador desactivado correctamente." });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> ObtenerDashboard()
    {
        try
        {
            var dashboard = await _adminService.ObtenerDashboard();
            return Ok(dashboard);
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
