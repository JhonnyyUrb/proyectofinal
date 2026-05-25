using System.Security.Cryptography;
using System.Text;
using Google.Cloud.Firestore;
using ProyectoClaseQ2.DTOs;
using ProyectoClaseQ2.Models;

namespace ProyectoClaseQ2.Services;

public class AuthService
{
   private readonly firebaseService _firebaseService;
   private readonly IConfiguration _configuration;
   
   public AuthService(firebaseService firebaseService, IConfiguration configuration)
   {
       _firebaseService = firebaseService;
       _configuration = configuration;
   }
   
   
      //.......     
    }

