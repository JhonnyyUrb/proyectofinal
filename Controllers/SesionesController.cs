using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyectofinalQ2.DTOs;
using proyectofinalQ2.Services;

namespace proyectofinalQ2.Controllers;

[Authorize]
[ApiController]
[Route("api/Sesiones")] 
public class SesionesController : ControllerBase
{
    private readonly SesionMediacionService _sesionService;

    public SesionesController(SesionMediacionService sesionService)
    {
        _sesionService = sesionService;
    }

    [HttpPost("agendar")] 
    public async Task<IActionResult> Agendar([FromBody] CrearSesionDto dto)
    {
        try
        {
            var mediadorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(mediadorId))
                return Unauthorized(new { message = "Mediador no identificado." });

            var resultado = await _sesionService.AgendarSesion(dto, mediadorId);
            return Ok(resultado);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("acuerdo")]
    public async Task<IActionResult> RegistrarAcuerdo([FromBody] CrearAcuerdoDto dto)
    {
        try
        {
            var resultado = await _sesionService.RegistrarAcuerdoFinal(dto);
            return Ok(resultado);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

   

    [HttpPut("acuerdo/{acuerdoId}/confirmar-reportante")]
    public async Task<IActionResult> ConfirmarReportante(string acuerdoId)
    {
        try
        {
            var esFormalizado = await _sesionService.ConfirmarAcuerdoPorReportante(acuerdoId);
            return Ok(new { message = "Acuerdo confirmado por el reportante.", formalizado = esFormalizado });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("acuerdo/{acuerdoId}/confirmar-denunciado")]
    public async Task<IActionResult> ConfirmarDenunciado(string acuerdoId)
    {
        try
        {
            var esFormalizado = await _sesionService.ConfirmarAcuerdoPorDenunciado(acuerdoId);
            return Ok(new { message = "Acuerdo confirmado por el denunciado.", formalizado = esFormalizado });
        }
        catch (Exception e) // 3. Corregido el ';' que rompía la compilación aquí
        {
            return BadRequest(new { message = e.Message });
        }
    }
}
