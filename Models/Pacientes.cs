using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class Pacientes
{
    public int PacienteId { get; set; }

    public int UsuarioId { get; set; }

    public int NutriologoId { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public string Sexo { get; set; } = null!;

    public string? ObjetivoGeneral { get; set; }

    public virtual Nutriologos Nutriologo { get; set; } = null!;

    public virtual ICollection<PlanesNutricionales> PlanesNutricionales { get; set; } = new List<PlanesNutricionales>();

    public virtual ICollection<RecetasAdaptadas> RecetasAdaptadas { get; set; } = new List<RecetasAdaptadas>();

    public virtual Usuarios Usuario { get; set; } = null!;
}
