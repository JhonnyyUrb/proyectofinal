using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using proyectofinalQ2.DTOs;
using proyectofinalQ2.Models;

namespace proyectofinalQ2.Services;

public class CasoConflictoService
{
    private readonly FirebaseService _firebaseService;

    public CasoConflictoService(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    public async Task<CasoConflicto> CrearCaso(CrearCasoDto dto, string ciudadanoId)
    {
        var collection = _firebaseService.GetCollection("CasosConflicto");

        // 1. Validar duplicados activos entre las mismas partes
        var query1 = await collection
            .WhereEqualTo("CiudadanoId", ciudadanoId)
            .WhereEqualTo("ContraparteId", dto.ContraparteId)
            .GetSnapshotAsync();

        var query2 = await collection
            .WhereEqualTo("CiudadanoId", dto.ContraparteId)
            .WhereEqualTo("ContraparteId", ciudadanoId)
            .GetSnapshotAsync();

        var todosLosCasos = query1.Documents.Concat(query2.Documents);
        foreach (var doc in todosLosCasos)
        {
            if (doc.Exists)
            {
                var data = doc.ToDictionary();
                var estado = data.ContainsKey("Estado") ? data["Estado"].ToString() : "";
                if (estado != "Resuelto" && estado != "Cerrado sin acuerdo" && estado != "CerradoSinAcuerdo")
                {
                    throw new Exception("Ya existe un caso activo entre las mismas dos partes.");
                }
            }
        }

        // 2. Crear el nuevo caso
        var nuevoCaso = new CasoConflicto
        {
            Id = Guid.NewGuid().ToString(),
            CiudadanoId = ciudadanoId,
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            Direccion = dto.Direccion,
            Categoria = dto.Categoria,
            Estado = "Pendiente",
            ContraparteId = dto.ContraparteId,
            ZoneId = dto.ZoneId,
            UserId = ciudadanoId,
            CreatedAt = DateTime.UtcNow,
        };

        await collection.Document(nuevoCaso.Id).SetAsync(new Dictionary<string, object>
        {
            { "Id", nuevoCaso.Id },
            { "CiudadanoId", nuevoCaso.CiudadanoId },
            { "Titulo", nuevoCaso.Titulo },
            { "Descripcion", nuevoCaso.Descripcion },
            { "Direccion", nuevoCaso.Direccion },
            { "Categoria", nuevoCaso.Categoria },
            { "Estado", nuevoCaso.Estado },
            { "ContraparteId", nuevoCaso.ContraparteId },
            { "ZoneId", nuevoCaso.ZoneId },
            { "UserId", nuevoCaso.UserId },
            { "CreatedAt", Timestamp.FromDateTime(nuevoCaso.CreatedAt) }
        });

        return nuevoCaso;
    }

    public async Task<List<CasoConflicto>> ListarCasos(string role, string? zoneId)
    {
        var collection = _firebaseService.GetCollection("CasosConflicto");
        Query query = collection;

        // Si es mediador y tiene una zona asignada, filtrar por esa zona
        if (role == "Mediador" && !string.IsNullOrEmpty(zoneId))
        {
            query = collection.WhereEqualTo("ZoneId", zoneId);
        }

        var snapshot = await query.GetSnapshotAsync();
        var lista = new List<CasoConflicto>();

        foreach (var doc in snapshot.Documents)
        {
            if (!doc.Exists) continue;
            var data = doc.ToDictionary();

            var caso = new CasoConflicto
            {
                Id = data.ContainsKey("Id") ? data["Id"].ToString()! : doc.Id,
                CiudadanoId = data.ContainsKey("CiudadanoId") ? data["CiudadanoId"].ToString()! : "",
                Titulo = data.ContainsKey("Titulo") ? data["Titulo"].ToString()! : "",
                Descripcion = data.ContainsKey("Descripcion") ? data["Descripcion"].ToString()! : "",
                Direccion = data.ContainsKey("Direccion") ? data["Direccion"].ToString()! : "",
                Categoria = data.ContainsKey("Categoria") ? data["Categoria"].ToString()! : "",
                Estado = data.ContainsKey("Estado") ? data["Estado"].ToString()! : "Pendiente",
                ContraparteId = data.ContainsKey("ContraparteId") ? data["ContraparteId"].ToString()! : "",
                ZoneId = data.ContainsKey("ZoneId") ? data["ZoneId"].ToString()! : "",
                UserId = data.ContainsKey("UserId") ? data["UserId"].ToString()! : "",
                CreatedAt = data.ContainsKey("CreatedAt") && data["CreatedAt"] is Timestamp ts ? ts.ToDateTime() : DateTime.UtcNow
            };
            lista.Add(caso);
        }

        return lista;
    }
}