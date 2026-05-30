using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Microsoft.IdentityModel.Tokens;
using proyectofinalQ2.DTOs;
using proyectofinalQ2.Models;

namespace proyectofinalQ2.Services;

public class LabNoteService
{
    
    private readonly FirebaseService _firebaseService;
    private readonly IConfiguration _configuration;
    
    public async Task<bool> DeleteNoteAsync(string id, string currentUserId)
    {
        var collection =_firebaseService.GetCollection("labnotes");
        var docRef = collection.Document(id);
        var snapshot = await docRef.GetSnapshotAsync();

        if (!snapshot.Exists)
        {
            return false;
        }

        var data = snapshot.ToDictionary();
        string ownerId=data.ContainsKey("UserId")?data["UserId"].ToString():string.Empty;

        if (ownerId != currentUserId)
        {
            throw new UnauthorizedAccessException("No tiene permiso para eliminar esta labnote");
        }

        await docRef.DeleteAsync();
        return true;
        
    }
}