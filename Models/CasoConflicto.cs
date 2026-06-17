using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

namespace proyectofinalQ2.Models;

public class CasoConflicto
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string CiudadanoId { get; set; } = string.Empty;

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La Descripcion es obligatoria.")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "Ejemplo: Ruido o Basura")]
    public string Categoria { get; set; } = string.Empty;

public string Direccion { get; set; } = string.Empty;
public string Estado { get; set; } = "Pendiente";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Este campo no debe ser enviado por el usuario en el request.
    // Se recomienda excluirlo de los DTOs de entrada (Input Models).
    public string UserId { get; set; }

    public string ContraparteId { get; set; } = string.Empty;
    public string ZoneId { get; set; } = string.Empty;
    
    [FirestoreProperty]
    public string MediadorId { get; set; } = string.Empty;
    
}