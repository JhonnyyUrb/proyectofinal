using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace proyectofinalQ2.Services
{
    // Esta clase le dice a Scalar que nuestra API usa Bearer tokens
    // Es necesaria porque en .NET 10 Scalar no lo detecta automáticamente
    internal sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
    {
        private readonly IAuthenticationSchemeProvider _authenticationSchemeProvider;

        // Recibimos el proveedor de esquemas de autenticación por inyección de dependencias
        public BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider)
        {
            _authenticationSchemeProvider = authenticationSchemeProvider;
        }

        public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var authSchemes = await _authenticationSchemeProvider.GetAllSchemesAsync();

            // Solo aplicamos si la app tiene Bearer configurado
            if (authSchemes.Any(s => s.Name == "Bearer"))
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                // Declaramos el esquema Bearer en el documento OpenAPI
                document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT"
                };

                // Aplicamos el esquema a todos los endpoints del documento
                foreach (var operation in document.Paths.Values.SelectMany(p => p.Operations))
                {
                    operation.Value.Security ??= new List<OpenApiSecurityRequirement>();
                    operation.Value.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    });
                }
            }
        }
    }
}