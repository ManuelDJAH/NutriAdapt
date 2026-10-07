using Microsoft.AspNetCore.Identity;
using NutriAdapt.Models;

namespace NutriAdapt.Servicios;

// Proyecto escolar: la contraseña se guarda tal cual en Usuarios.Contraseña
// y el login compara texto contra texto.
// Para volver al hash de Identity, quitar el registro de esta clase en Program.cs.
public class PasswordHasherTextoPlano : IPasswordHasher<Usuario>
{
    public string HashPassword(Usuario user, string password)
    {
        return password;
    }

    public PasswordVerificationResult VerifyHashedPassword(Usuario user, string hashedPassword, string providedPassword)
    {
        return hashedPassword == providedPassword
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
    }
}
