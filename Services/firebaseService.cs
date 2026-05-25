using Google.Cloud.Firestore;

namespace ProyectoClaseQ2.Services;



public class firebaseService
{
    
    
    // este servicio es el puente entre nuestra app y firebase
    // tod0 lo que vamos a hablar con firestore pasa por aqui
    
    private readonly FirestoreDb _firestoreDb;


    public firebaseService()
    {
        // decirle a fb donde esta en archivo con las credenciales
        // usar l ruta relativa
        
        var credentialPath = Path.Combine(AppContext.BaseDirectory, "Config", "firebase-credentialss.json");
        
        // esta es una variable de entorno que usa el sdk de GOOGLE, para autenticarse
        
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialPath);
        
        // ahora aqui colocamos el project id
        _firestoreDb = FirestoreDb.Create("proyectofinal-14607");
        
    }
    // devuelve una referencia de una coleccion
    public CollectionReference GetCollection(string collectionName)
    {
        return _firestoreDb.Collection(collectionName);
    }
}