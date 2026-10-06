using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NutriAdapt.Models;

namespace NutriAdapt.Servicios;

// Agrega a la cookie el rol (para [Authorize(Roles = ...)]) y el nombre para mostrar.
public class UsuarioClaimsPrincipalFactory : UserClaimsPrincipalFactory<Usuario>
{
    public const string ClaimNombreCompleto = "NombreCompleto";

    public UsuarioClaimsPrincipalFactory(UserManager<Usuario> userManager, IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Usuario user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim(ClaimTypes.Role, RolesUsuario.Nombre(user.RolId)));
        identity.AddClaim(new Claim(ClaimNombreCompleto, user.NombreCompleto));
        return identity;
    }
}
