using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyectofinalQ2.DTOs;
using proyectofinalQ2.Services;

namespace proyectofinalQ2.Controllers;

[ApiController]
[Route("api/casos")]
public class CasosController : ControllerBase
{
    private readonly CasoConflictoService _casoService;

    public CasosController(CasoConflictoService casoService)
    {
        _casoService = casoService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] CrearCasoDto dto)
    {
        try
        {
            var ciudadanoId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(ciudadanoId))
                return Unauthorized(new { message = "Usuario no identificado" });

            var resultado = await _casoService.CrearCaso(dto, ciudadanoId);
            return Ok(resultado);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Listar()
    {
        try
        {
            string role = "Ciudadano";
            string? zoneId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Ciudadano";
                zoneId = User.FindFirst("ZoneId")?.Value;
            }

            var casos = await _casoService.ListarCasos(role, zoneId);
            return Ok(casos);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet("mis-casos")]
    [Authorize]
    public async Task<IActionResult> GetMisCasos()
    {
        var userid=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userid))
        {
            return Unauthorized("No se pudo identificar el usario desde el token");
        }
        var casos=await _casoService.ObtenerMisCasos(userid);
        return Ok(casos);
    }

    [HttpPut("{casosId}/asignar-mediador")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AsignarMediador(string casoId, [FromBody] string mediadorId)
    {
        if (string.IsNullOrEmpty(mediadorId))
        {
            return BadRequest("El id de mediador es requerido");
        }
        await _casoService.AsignarMediador(casoId, mediadorId);
        return Ok((new {mensaje="Mediador asignado exitosamente y Estado cambiado a Asignado"}));
    }

    [HttpPut("{casoId}/estado")]
    [Authorize(Roles = "Admin,Mediador")]
    public async Task<IActionResult> ActualizarEstado(string casoId, [FromBody] string nuevoEstado)
    {
        if (string.IsNullOrEmpty(nuevoEstado))
        {
            return BadRequest("El nuevo estado es requerido");
        }
        await _casoService.ActualizarEstadoCaso(casoId, nuevoEstado);
        return Ok((new{mensaje=$"El estado del caso fue actualizado a '{nuevoEstado}' con exito"}));
    }
    
}
