namespace ProyectoClaseQ2.Models;

public class CasoConflicto
{
   
        public string Id { get; set; }
        public string reportId { get; set; }
        public string NomDenunciado { get; set; }

        public string TipoConflicto { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }

        public string Estado { get; set; }

        public string IdMediador { get; set; }

        public string Evidencia { get; set; }

        public DateTime Created { get; set; }
    
}