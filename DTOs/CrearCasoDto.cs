using System.ComponentModel.DataAnnotations;

namespace proyectofinalQ2.DTOs;

public class CrearCasoDto
{
    [Required(ErrorMessage = "El titulo es requerido")]
    public string Titulo { get; set; } = string.Empty;
    [Required(ErrorMessage = "La descripcion es requerida")]
    public string Descripcion { get; set; } = string.Empty;
    [Required(ErrorMessage = "La direccion es requerida")]
    public string Direccion { get; set; } = string.Empty;
    [Required(ErrorMessage = "La categoria es requerida")]
    public string Categoria { get; set; } = string.Empty;
    [Required(ErrorMessage = "La contraparte es requerida")]
    public string ContraparteId { get; set; } = string.Empty;
    [Required(ErrorMessage = "La zona es requerida")]
    public string ZoneId { get; set; } = string.Empty;
}