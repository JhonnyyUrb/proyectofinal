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
}
