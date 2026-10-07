namespace NutriAdapt.Models;

// Usuario que maneja ASP.NET Core Identity (UserManager / SignInManager).
// Se lee y escribe en la tabla Usuarios con Dapper a traves de UsuarioStore.
public class Usuario
{
    public int UsuarioId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Contraseña { get; set; }
    public byte RolId { get; set; }
    public DateTime FechaCreacion { get; set; }

    // Solo en memoria: la tabla no tiene columna normalizada (la intercalacion es CI)
    public string? CorreoNormalizado { get; set; }
}
