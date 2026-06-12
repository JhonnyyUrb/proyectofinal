using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using proyectofinalQ2.DTOs;
using proyectofinalQ2.Models;

namespace proyectofinalQ2.Services;

public class SesionMediacionService
{
    private readonly FirebaseService _firebaseService;

    public SesionMediacionService(FirebaseService firebaseService)
    {
        _firebaseService = firebaseService;
    }

    public async Task<SesionMediacion> AgendarSesion(CrearSesionDto dto, string mediadorId)
    {
        var sesionesCollection = _firebaseService.GetCollection("SesionesMediacion");
        var casosCollection = _firebaseService.GetCollection("CasosConflicto");

        // Validar que el caso existe
        var casoDoc = await casosCollection.Document(dto.IdCaso).GetSnapshotAsync();
        if (!casoDoc.Exists)
        {
            throw new Exception("El caso especificado no existe.");
        }

        var sesion = new SesionMediacion
        {
            Id = Guid.NewGuid().ToString(),
            IdCaso = dto.IdCaso,
            MediadorId = mediadorId,
            FechaSesion = dto.FechaSesion,
            Modalidad = dto.Modalidad,
            EnlaceReunion = dto.EnlaceReunion ?? string.Empty,
            Estado = "Programada"
        };

        // Guardar sesión en Firestore
        await sesionesCollection.Document(sesion.Id).SetAsync(new Dictionary<string, object>
        {
            { "Id", sesion.Id },
            { "IdCaso", sesion.IdCaso },
            { "MediadorId", sesion.MediadorId },
            { "FechaSesion", Timestamp.FromDateTime(sesion.FechaSesion.ToUniversalTime()) },
            { "Modalidad", sesion.Modalidad },
            { "EnlaceReunion", sesion.EnlaceReunion },
            { "Estado", sesion.Estado }
        });

        // Actualizar el estado del caso a "En Mediacion"
        await casosCollection.Document(dto.IdCaso).UpdateAsync("Estado", "En Mediación");

        return sesion;
    }

    public async Task<Acuerdo> RegistrarAcuerdoFinal(CrearAcuerdoDto dto)
    {
        var acuerdosCollection = _firebaseService.GetCollection("Acuerdos");
        var casosCollection = _firebaseService.GetCollection("CasosConflicto");

        // Validar que el caso existe
        var casoDoc = await casosCollection.Document(dto.IdCaso).GetSnapshotAsync();
        if (!casoDoc.Exists)
        {
            throw new Exception("El caso especificado no existe.");
        }

        var acuerdo = new Acuerdo
        {
            Id = Guid.NewGuid().ToString(),
            IdCaso = dto.IdCaso,
            TextAcuerdo = dto.TextoAcuerdo,
            ConfirmadoPorReportante = false,
            ConfirmadoPorDenunciado = false
        };

        // Guardar acuerdo en Firestore
        await acuerdosCollection.Document(acuerdo.Id).SetAsync(new Dictionary<string, object>
        {
            { "Id", acuerdo.Id },
            { "IdCaso", acuerdo.IdCaso },
            { "TextAcuerdo", acuerdo.TextAcuerdo },
            { "ConfirmadoPorReportante", acuerdo.ConfirmadoPorReportante },
            { "ConfirmadoPorDenunciado", acuerdo.ConfirmadoPorDenunciado }
        });

        // Actualizar el estado del caso a "Resuelto" (o se puede dejar en "En Mediacion" hasta confirmación,
        // pero para cumplir con el flujo estándar actualizamos su estado).
        await casosCollection.Document(dto.IdCaso).UpdateAsync("Estado", "Resuelto");

        return acuerdo;
    }
}
