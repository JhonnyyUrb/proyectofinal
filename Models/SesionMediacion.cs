using System;

namespace proyectofinalQ2.Models;

public class SesionMediacion
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string IdCaso { get; set; } = string.Empty;
    public string MediadorId { get; set; } = string.Empty;
    public DateTime FechaSesion { get; set; }
    public string Modalidad { get; set; } = string.Empty;
    public string EnlaceReunion { get; set; } = string.Empty;
    public string Estado { get; set; } = "Programada";
}