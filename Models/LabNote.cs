using System.ComponentModel.DataAnnotations;

namespace proyectofinalQ2.Models;

public class LabNote
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres.")]
    public string Title { get; set; }

    [Required(ErrorMessage = "La observación es obligatoria.")]
    public string Observation { get; set; }

    [Required]
    [RegularExpression("^(Quimica|Biologia|Fisica|Otro)$", 
        ErrorMessage = "La categoría debe ser: Quimica, Biologia, Fisica u Otro.")]
    public string Category { get; set; }

    [Range(1, 3, ErrorMessage = "La prioridad debe ser 1 (baja), 2 (media) o 3 (alta).")]
    public int Priority { get; set; }

    public bool IsPublic { get; set; }

    public string Tags { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Este campo no debe ser enviado por el usuario en el request.
    // Se recomienda excluirlo de los DTOs de entrada (Input Models).
    public string UserId { get; set; }

}