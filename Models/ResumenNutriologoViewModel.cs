namespace NutriAdapt.Models;

// Datos del dashboard del nutriologo (Nutriologos_ObtenerResumenPorUsuario)
public class ResumenNutriologoViewModel
{
    public int NutriologoId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string CedulaProfesional { get; set; } = string.Empty;
    public string? Especialidad { get; set; }

    public int TotalPacientes { get; set; }
    public int PlanesActivos { get; set; }
    public int RecetasAdaptadas { get; set; }
    public int TotalAlimentos { get; set; }
    public int TotalRecetas { get; set; }
}
