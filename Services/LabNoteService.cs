﻿using System;
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

    public LabNoteService(FirebaseService firebaseService, IConfiguration configuration)
    {
        _firebaseService = firebaseService;
        _configuration = configuration;
    }
    
    public async Task<LabNote> CreateNoteAsync(CreateLabNoteDto dto, string userId)
    {
        var validCategories = new List<string> { "Quimica", "Biologia", "Fisica", "Otro" };
        if (!validCategories.Contains(dto.Category))
        {
            throw new ArgumentException("Categoría no permitida. Use: Quimica, Biologia, Fisica u Otro.");
        }
        
        var note = new LabNote
        {
           Id = Guid.NewGuid().ToString(),
            Title = dto.Title,
            Observation = dto.Observation,
            Category = dto.Category,
            Priority = dto.Priority,
            IsPublic = dto.IsPublic,
            Tags = dto.Tags,
            CreatedAt = DateTime.UtcNow, 
            UserId = userId 
        };

        var collection = _firebaseService.GetCollection("labnotes");
        
        await collection.Document(note.Id).SetAsync(new Dictionary<string, object>
        {
            { "Id", note.Id },
            { "Title", note.Title },
            { "Observation", note.Observation },
            { "Category", note.Category },
            { "Priority", note.Priority },
            { "IsPublic", note.IsPublic },
            { "Tags", note.Tags },
            { "CreatedAt", Timestamp.FromDateTime(note.CreatedAt) },
            { "UserId", note.UserId }
        });
        return note;
    }
    
    public async Task<List<LabNote>> GetNotesByUserIdAsync(string userId)
    {
        var collection = _firebaseService.GetCollection("labnotes");
        var querySnapshot = await collection.WhereEqualTo("UserId", userId).GetSnapshotAsync();
        var notesList = new List<LabNote>();

        foreach (var doc in querySnapshot.Documents)
        {
            if (!doc.Exists) continue;
            var data = doc.ToDictionary();
            
            var note = new LabNote
            {
                Id = data.ContainsKey("Id") ? data["Id"].ToString()! : doc.Id,
                Title = data.ContainsKey("Title") ? data["Title"].ToString()! : string.Empty,
                Observation = data.ContainsKey("Observation") ? data["Observation"].ToString()! : string.Empty,
                Category = data.ContainsKey("Category") ? data["Category"].ToString()! : string.Empty,
                Priority = data.ContainsKey("Priority") ? Convert.ToInt32(data["Priority"]) : 1,
                IsPublic = data.ContainsKey("IsPublic") && Convert.ToBoolean(data["IsPublic"]),
                Tags = data.ContainsKey("Tags") ? data["Tags"].ToString()! : string.Empty,
                UserId = data.ContainsKey("UserId") ? data["UserId"].ToString()! : string.Empty,
                CreatedAt = data.ContainsKey("CreatedAt") && data["CreatedAt"] is Timestamp ts ? ts.ToDateTime() : DateTime.UtcNow
            };
            notesList.Add(note);
        }
        return notesList;
    }
    
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