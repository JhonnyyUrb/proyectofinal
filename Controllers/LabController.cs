using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Security.Claims;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyectofinalQ2.Models;
using proyectofinalQ2.Services;

namespace proyectofinalQ2.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LabController: ControllerBase
{
    private readonly LabNoteService _labNoteService;

    public LabController(LabNoteService labNoteService)
    {
        _labNoteService = labNoteService;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "Token Invalido" });
            }
            
            var result = await _labNoteService.DeleteNoteAsync(id, currentUserId);

            if (!result)
            {
                return NotFound(new { message = "Nota eliminada" });
            }

            return Ok(new { message = "Nota eliminada exitosamente." });
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "Error del servidor" });
        }
    }
    [HttpPost("Register")]
    [AllowAnonymous] 
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var _firebaseApiKey = "AIzaSyC9QtAADlI2CSo3N3wIT1a2kyRhsiuNSDE"; 
        var client = new HttpClient(); 
        
        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={_firebaseApiKey}";

        var payload = new { 
            email = request.Email, 
            password = request.Password, 
            returnSecureToken = true 
        };

        var response = await client.PostAsJsonAsync(url, payload);

        if (!response.IsSuccessStatusCode)
        {
         //   return BadRequest(new { message = "Error al crear el usuario" });
         var errorContent = await response.Content.ReadAsStringAsync();
         return BadRequest(new { message = "Error al crear el usuario", detalle = errorContent });
        }

        var content = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();

        return Ok(new
        {
            idToken = content.GetProperty("idToken").GetString(),
            localId = content.GetProperty("localId").GetString(),
            email = content.GetProperty("email").GetString()
        });
    }

    [HttpPost("Notes")]
    public async Task<IActionResult> CreateNote([FromBody] proyectofinalQ2.DTOs.CreateLabNoteDto dto)
    {
        try
        {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized(new { message = "Token invalido" });
        }
        
        var newNote=await _labNoteService.CreateNoteAsync(dto, currentUserId);
        
        return CreatedAtAction(nameof(GetMyNotes),new { id = newNote.Id},newNote);
        
        }
        catch (ArgumentException e)
        {
return BadRequest(new { message = e.Message });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "Error del servidor al crear la nota" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotes()
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized(new { message = "Token Inválido o ausente" });
            }
            
            var notes = await _labNoteService.GetNotesByUserIdAsync(currentUserId);
        
            return Ok(notes);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "Error del servidor al obtener notas" });
        }
    }
    

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
}