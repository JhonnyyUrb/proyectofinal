using Google.Apis.Util;

namespace proyectofinalQ2.Services;

public class SesionMediacionService
{
private readonly TuDbContext _context;

public SesionMediacionService (TuDbContext context) => _context = context;

public async Task<bool> RegistrarAcuerdo(string idsesion, string textoAcuerdo)
{
    var sesion = await _context.SesionesMediacion.FindAsync(idsesion);
    if (sesion == null) return false;

    sesion.AcuerdoFinal = textoAcuerdo;
    sesion.Estado = "COMPLETADA";

    await _context.SaveChangesAsync();
    return true;
    
}
}