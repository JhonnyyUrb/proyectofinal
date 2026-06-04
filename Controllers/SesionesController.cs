namespace proyectofinalQ2.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SesionesController
{
    private readonly SesionMediacionService _service;

    public SesionesController(SesionMediacionService service) => _service = service;

    [HttpPost("agendar")]
    public async Task<IActionResult> Agendar([FromBody] CrearSesionDto dto) // Usa tu DTO
    {
        // Llamar al servicio para guardar
        var nuevaSesion = await _service.CrearSesion(dto);
        return Ok(nuevaSesion);
    }

    [HttpPut("{id}/finalizar")]
    public async Task<IActionResult> Finalizar(string id, [FromBody] string acuerdo)
    {
        var resultado = await _service.RegistrarAcuerdo(id, acuerdo);
        return resultado ? NoContent() : NotFound();
    } 
}
