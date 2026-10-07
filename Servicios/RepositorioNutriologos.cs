using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using NutriAdapt.Models;

namespace NutriAdapt.Servicios;

public interface IRepositorioNutriologos
{
    Task<int> Crear(int usuarioId, string cedulaProfesional, string? especialidad);
    Task<ResumenNutriologoViewModel?> ObtenerResumen(int usuarioId);
}

public class RepositorioNutriologos : IRepositorioNutriologos
{
    private readonly string connectionString;

    public RepositorioNutriologos(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("NutriAdaptConnection")!;
    }

    public async Task<int> Crear(int usuarioId, string cedulaProfesional, string? especialidad)
    {
        using var connection = new SqlConnection(connectionString);
        return await connection.QuerySingleAsync<int>("Nutriologos_Crear",
            new { UsuarioId = usuarioId, CedulaProfesional = cedulaProfesional, Especialidad = especialidad },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ResumenNutriologoViewModel?> ObtenerResumen(int usuarioId)
    {
        using var connection = new SqlConnection(connectionString);
        return await connection.QuerySingleOrDefaultAsync<ResumenNutriologoViewModel>(
            "Nutriologos_ObtenerResumenPorUsuario",
            new { UsuarioId = usuarioId }, commandType: CommandType.StoredProcedure);
    }
}
