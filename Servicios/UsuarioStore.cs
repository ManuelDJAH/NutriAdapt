using Microsoft.AspNetCore.Identity;
using NutriAdapt.Models;

namespace NutriAdapt.Servicios;

// Puente entre ASP.NET Core Identity y la tabla Usuarios (via IRepositorioUsuarios).
// El correo funciona como nombre de usuario.
public class UsuarioStore : IUserStore<Usuario>, IUserEmailStore<Usuario>, IUserPasswordStore<Usuario>
{
    private readonly IRepositorioUsuarios repositorioUsuarios;

    public UsuarioStore(IRepositorioUsuarios repositorioUsuarios)
    {
        this.repositorioUsuarios = repositorioUsuarios;
    }

    public async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken)
    {
        user.UsuarioId = await repositorioUsuarios.Crear(user);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken)
    {
        await repositorioUsuarios.Actualizar(user);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken)
    {
        await repositorioUsuarios.Borrar(user.UsuarioId);
        return IdentityResult.Success;
    }

    public async Task<Usuario?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        return int.TryParse(userId, out var id) ? await repositorioUsuarios.BuscarPorId(id) : null;
    }

    public async Task<Usuario?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await repositorioUsuarios.BuscarPorCorreo(normalizedUserName);
    }

    public async Task<Usuario?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return await repositorioUsuarios.BuscarPorCorreo(normalizedEmail);
    }

    public Task<string> GetUserIdAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UsuarioId.ToString());
    }

    public Task<string?> GetUserNameAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(user.Correo);
    }

    public Task SetUserNameAsync(Usuario user, string? userName, CancellationToken cancellationToken)
    {
        user.Correo = userName ?? string.Empty;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.CorreoNormalizado);
    }

    public Task SetNormalizedUserNameAsync(Usuario user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.CorreoNormalizado = normalizedName;
        return Task.CompletedTask;
    }

    public Task<string?> GetEmailAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(user.Correo);
    }

    public Task SetEmailAsync(Usuario user, string? email, CancellationToken cancellationToken)
    {
        user.Correo = email ?? string.Empty;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedEmailAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.CorreoNormalizado);
    }

    public Task SetNormalizedEmailAsync(Usuario user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        user.CorreoNormalizado = normalizedEmail;
        return Task.CompletedTask;
    }

    // Sin confirmacion de correo por ahora
    public Task<bool> GetEmailConfirmedAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public Task SetEmailConfirmedAsync(Usuario user, bool confirmed, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    // Los nombres "PasswordHash" vienen de la interfaz de Identity;
    // con PasswordHasherTextoPlano lo que llega aqui es la contraseña tal cual
    public Task<string?> GetPasswordHashAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Contraseña);
    }

    public Task SetPasswordHashAsync(Usuario user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.Contraseña = passwordHash;
        return Task.CompletedTask;
    }

    public Task<bool> HasPasswordAsync(Usuario user, CancellationToken cancellationToken)
    {
        return Task.FromResult(!string.IsNullOrEmpty(user.Contraseña));
    }

    public void Dispose()
    {
    }
}
