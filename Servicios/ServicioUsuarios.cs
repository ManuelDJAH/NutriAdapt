using System.Security.Claims;

namespace NutriAdapt.Servicios;

public interface IServicioUsuarios
{
    int ObtenerUsuarioId();
}

// Obtiene el UsuarioId de quien inicio sesion (en lugar de un UsuarioId fijo)
public class ServicioUsuarios : IServicioUsuarios
{
    private readonly HttpContext httpContext;

    public ServicioUsuarios(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor.HttpContext!;
    }

    public int ObtenerUsuarioId()
    {
        if (httpContext.User.Identity?.IsAuthenticated != true)
        {
            throw new ApplicationException("El usuario no ha iniciado sesión");
        }

        var idClaim = httpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
        return int.Parse(idClaim.Value);
    }
}
