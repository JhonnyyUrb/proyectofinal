using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using proyectofinalQ2.Models;

namespace proyectofinalQ2.Services;

public class AdminService
{
    private readonly FirebaseService _firebaseService;

    public AdminService(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    public async Task<bool> AsignarZonaMediador(string userId, string zoneId)
    {
        var collection = _firebaseService.GetCollection("Users");
        var docRef = collection.Document(userId);
        var snapshot = await docRef.GetSnapshotAsync();

        if (!snapshot.Exists)
        {
            throw new Exception("Usuario no encontrado.");
        }

        var data = snapshot.ToDictionary();
        var role = data.ContainsKey("Role") ? data["Role"].ToString() : "";

        // Opcional: Validar que sea un mediador (o rol similar, case-insensitive)
        if (!string.Equals(role, "Mediador", StringComparison.OrdinalIgnoreCase))
        {
            throw new Exception("El usuario no tiene asignado el rol de Mediador.");
        }

        await docRef.UpdateAsync("ZoneId", zoneId);
        return true;
    }

    public async Task<bool> AlternarEstadoUsuario(string userId)
    {
        var collection = _firebaseService.GetCollection("Users");
        var docRef = collection.Document(userId);
        var snapshot = await docRef.GetSnapshotAsync();

        if (!snapshot.Exists)
        {
            throw new Exception("Usuario no encontrado.");
        }

        var data = snapshot.ToDictionary();
        bool isActive = true;

        if (data.ContainsKey("IsActive"))
        {
            isActive = Convert.ToBoolean(data["IsActive"]);
        }

        bool nuevoEstado = !isActive;
        await docRef.UpdateAsync("IsActive", nuevoEstado);

        return nuevoEstado;
    }
}
