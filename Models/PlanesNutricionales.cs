using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class PlanesNutricionales
{
    public int PlanId { get; set; }

    public int PacienteId { get; set; }

    public int NutriologoId { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public decimal KcalObjetivo { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Nutriologos Nutriologo { get; set; } = null!;

    public virtual Pacientes Paciente { get; set; } = null!;

    public virtual ICollection<PlanPorciones> PlanPorciones { get; set; } = new List<PlanPorciones>();

    public virtual ICollection<RecetasAdaptadas> RecetasAdaptadas { get; set; } = new List<RecetasAdaptadas>();
}
