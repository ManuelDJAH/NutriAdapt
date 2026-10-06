namespace NutriAdapt.Models;

// Valores de Usuarios.RolId (CK_Usuarios_RolId: 1 = Nutriologo, 2 = Paciente)
public static class RolesUsuario
{
    public const byte NutriologoId = 1;
    public const byte PacienteId = 2;

    public const string Nutriologo = "Nutriologo";
    public const string Paciente = "Paciente";

    public static string Nombre(byte rolId) => rolId == NutriologoId ? Nutriologo : Paciente;
}
