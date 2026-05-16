namespace ProyectoClaseQ2.Models;

public class Acuerdo
{
    
    public string Id { get; set; }

    public string IdCaso { get; set; }

    public string TextAcuerdo { get; set; }

    public bool ConfirmadoPorReportante { get; set; }

    public bool ConfirmadoPorDenunciado { get; set; }

    
    //Como poner una fecha limite...
    //public DateTime? FechaFormalizacion { get; set; }
    
    
}