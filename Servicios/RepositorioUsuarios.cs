using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NutriAdapt.Models;

namespace NutriAdapt.Servicios;

public interface IRepositorioUsuarios
{
    Task<int> Crear(Usuario usuario);
    Task<Usuario?> BuscarPorCorreo(string correo);
    Task<Usuario?> BuscarPorId(int usuarioId);
    Task Actualizar(Usuario usuario);
    Task Borrar(int usuarioId);
    Task<bool> Existe(string correo);
}

public class RepositorioUsuarios : IRepositorioUsuarios
{
    private readonly string connectionString;

    public RepositorioUsuarios(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("NutriAdaptConnection")!;
    }

    public async Task<int> Crear(Usuario usuario)
    {
        using var connection = new SqlConnection(connectionString);
        return await connection.QuerySingleAsync<int>("Usuarios_Crear",
            new { usuario.NombreCompleto, usuario.Correo, usuario.Contraseña, usuario.RolId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<Usuario?> BuscarPorCorreo(string correo)
    {
        using var connection = new SqlConnection(connectionString);
        return await connection.QuerySingleOrDefaultAsync<Usuario>("Usuarios_ObtenerPorCorreo",
            new { Correo = correo }, commandType: CommandType.StoredProcedure);
    }

    public async Task<Usuario?> BuscarPorId(int usuarioId)
    {
        using var connection = new SqlConnection(connectionString);
        return await connection.QuerySingleOrDefaultAsync<Usuario>("Usuarios_ObtenerPorId",
            new { UsuarioId = usuarioId }, commandType: CommandType.StoredProcedure);
    }

    public async Task Actualizar(Usuario usuario)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync("Usuarios_Actualizar",
            new { usuario.UsuarioId, usuario.NombreCompleto, usuario.Correo, usuario.Contraseña },
            commandType: CommandType.StoredProcedure);
    }

    public async Task Borrar(int usuarioId)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync("Usuarios_Borrar",
            new { UsuarioId = usuarioId }, commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> Existe(string correo)
    {
        using var connection = new SqlConnection(connectionString);
        var existe = await connection.QueryFirstOrDefaultAsync<int>(
            @"SELECT 1 FROM Usuarios WHERE Correo = @Correo;",
            new { correo });
        return existe == 1;
    }
}
