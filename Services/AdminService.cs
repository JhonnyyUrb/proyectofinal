using Google.Cloud.Firestore;
using proyectofinalQ2.DTOs;

namespace proyectofinalQ2.Services;

public class AdminService
{
    private readonly FirebaseService _firebaseService;

    public AdminService(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    public async Task AsignarZonaMediador(AsignarZona dto)
    {
        var collection = _firebaseService.GetCollection("Users");
        var userRef = collection.Document(dto.UserId);

        var snapshot = await userRef.GetSnapshotAsync();

        if (!snapshot.Exists)
        {
            throw new Exception("Usuario no encontrado");
        }

        await userRef.UpdateAsync(new Dictionary<string, object>
        {
            { "ZoneId", dto.ZoneId }
        });
    }

    public async Task AlternarEstadoUsuario(string userId)
    {
        var collection = _firebaseService.GetCollection("Users");
        var userRef = collection.Document(userId);

        var snapshot = await userRef.GetSnapshotAsync();

        if (!snapshot.Exists)
        {
            throw new Exception("Usuario no encontrado");
        }

        var data = snapshot.ToDictionary();

        bool isActive = data.ContainsKey("IsActive") 
            ? Convert.ToBoolean(data["IsActive"]) 
            : true;

        await userRef.UpdateAsync(new Dictionary<string, object>
        {
            { "IsActive", !isActive }
        });
    }
}