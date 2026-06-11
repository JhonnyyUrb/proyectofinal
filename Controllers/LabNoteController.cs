using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyectofinalQ2.DTOs;
using proyectofinalQ2.Services;

namespace proyectofinalQ2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LabNoteController : ControllerBase
{
    private readonly LabNoteService _labNoteService;

    public LabNoteController(LabNoteService labNoteService)
    {
        _labNoteService = labNoteService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLabNoteDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("No se encontró el usuario en el token");
        }

        var note = await _labNoteService.CreateNoteAsync(dto, userId);

        return Ok(note);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotes()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("No se encontró el usuario en el token");
        }

        var notes = await _labNoteService.GetNotesByUserIdAsync(userId);

        return Ok(notes);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("No se encontró el usuario en el token");
        }

        try
        {
            var result = await _labNoteService.DeleteNoteAsync(id, userId);

            if (!result)
            {
                return NotFound("Nota no encontrada");
            }

            return Ok("Nota eliminada correctamente");
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}