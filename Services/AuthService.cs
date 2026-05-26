using System.Security.Cryptography;
using System.Text;
using Google.Cloud.Firestore;
using proyectofinalQ2.DTOs;
using proyectofinalQ2.Models;

namespace proyectofinalQ2.Services;

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

