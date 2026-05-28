namespace proyectofinalQ2.Models;

public class User
{
    // representar un user en el sistema
    //nesta clase es lo que vamos a guadar en firestore y leer
    
    
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        
        public string Role { get; set; } // Admin, mediador o ciudadano
        public string? ZoneId { get; set; }
        // para saber cuando se cre el registro
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
     
    // la contraseña  siempre ira hasheada, nunca en texto plano
    public string PasswordHash { get; set; } = string.Empty;
     
    
}