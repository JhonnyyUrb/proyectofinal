using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Google.Cloud.Firestore;
using Microsoft.IdentityModel.Tokens;
using proyectofinalQ2.DTOs;
using proyectofinalQ2.Models;

namespace proyectofinalQ2.Services;

public class AuthService
{
   private readonly FirebaseService _firebaseService;
   private readonly IConfiguration _configuration;
   
   public AuthService(FirebaseService firebaseService, IConfiguration configuration)
   {
       _firebaseService = firebaseService;
       _configuration = configuration;
   }
   
      public async Task<User> Register(RegisterDto dto)
      {
          var collection = _firebaseService.GetCollection("Users");
          var existing = await collection
              .WhereEqualTo("Email", dto.Email)
              .GetSnapshotAsync();
          
          if (existing.Count>0)
              throw new Exception("Ya existe un usuario con ese email");

          var user = new User
          {
              Id = Guid.NewGuid().ToString(),
              FullName = dto.FullName,
              Email = dto.Email,
              Address = dto.Address,
              PasswordHash = HashPassword(dto.Password),
              Role = "ciudadano",
              ZoneId = null,
              IsActive = true,
              CreatedAt = DateTime.UtcNow
          };
          
          await collection.Document(user.Id).SetAsync(new Dictionary<string, object>
          {
              {"Id",user.Id},
              {"FullName", user.FullName},
              {"Email", user.Email},
              {"Address", user.Address},
              {"ZoneId", user.ZoneId??""},
              {"Role", user.Role},
              {"CreatedAt",Timestamp.FromDateTime(user.CreatedAt)},
              {"PasswordHash", user.PasswordHash},
          });
          return user;

      }
      
      public async Task<string> Login(LoginDto dto)
      {
        var collectionn=_firebaseService.GetCollection("users");
        var snapshot = await collectionn
            .WhereEqualTo("Email", dto.Email)
            .GetSnapshotAsync();
        
        if(snapshot.Count==0) throw new Exception("No existe usuario con ese email");

        var doc = snapshot.Documents[0];
        var data = doc.ToDictionary();

        var user = new User()
        {
            Id = data.ContainsKey("Id")?data["Id"].ToString()!: doc.Id,
            
            FullName = data.ContainsKey("FullName")?data["FullName"].ToString()!:"",
            
            Email = data.ContainsKey("Email")?data["Email"].ToString()!:"",
            
            Address = data.ContainsKey("Address")?data["Address"].ToString()!:"",
            
           ZoneId = data.ContainsKey("ZoneId") && data["ZoneId"]!=null?data["ZoneId"].ToString():null,
           
            Role = data.ContainsKey("Role")? data["Role"].ToString()!:"Ciudadano",
            
            CreatedAt = ((Google.Cloud.Firestore.Timestamp)data["CreatedAt"]).ToDateTime(),
            
            PasswordHash = data.ContainsKey("PasswordHash")?data["PasswordHash"].ToString()!:"",
            
            IsActive =data.ContainsKey("IsActive") || Convert.ToBoolean(data["IsActive"])
        };

        if (!VerifyPassword(dto.Password, user.PasswordHash)) throw new Exception("El correo o contraseña es Incorrecta");

        if (!user.IsActive) throw new Exception("Su cuenta se encuentra inactiva.");
        
        return GenerateToken(user);

      }
      
      private string GenerateToken(User user)
      {
          // El token lleva cierta informacion, Id, Email y Role del usuario que hizo login
          // Para proteccion de los endpoints, se sabe quien los esta llamando
          var claims = new List<Claim>
          {
              new Claim(ClaimTypes.NameIdentifier, user.Id),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.Role, user.Role)
          };

          if (!string.IsNullOrEmpty(user.Role))
          {
              claims.Add(new Claim("ZoneId", user.ZoneId));
          }

          var key = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
          var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                
          var token = new JwtSecurityToken(
                        
              issuer: _configuration["Jwt:Issuer"], //Quien lo genera, nuestro token lo genera la app
              audience: _configuration["Jwt:Issuer"], // Para quien lo genera, clientes / front-end
              claims: claims, // Estos son los datos del usuario
              expires: DateTime.UtcNow.AddHours(8), //Tiempo de vida del token
              signingCredentials: creds // Firma de seguridad
          );
          return new JwtSecurityTokenHandler().WriteToken(token);
      }
      
      private bool VerifyPassword(string dtoPassword, string userPasswordHash)
      {
          return HashPassword(dtoPassword) == userPasswordHash;
      }

      // Para encriptar la contraseña
      private string HashPassword(string password)
      {
          // SHA256 - tipo de encriptacion
          var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
          return Convert.ToBase64String(bytes);
      }
      
}

