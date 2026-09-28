using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class Nutriologos
{
    public int NutriologoId { get; set; }

    public int UsuarioId { get; set; }

    public string CedulaProfesional { get; set; } = null!;

    public string? Especialidad { get; set; }

    public virtual ICollection<Pacientes> Pacientes { get; set; } = new List<Pacientes>();

    public virtual ICollection<PlanesNutricionales> PlanesNutricionales { get; set; } = new List<PlanesNutricionales>();

    public virtual Usuarios Usuario { get; set; } = null!;
}
