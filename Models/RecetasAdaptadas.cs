using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class RecetasAdaptadas
{
    public int RecetaAdaptadaId { get; set; }

    public int RecetaId { get; set; }

    public int PacienteId { get; set; }

    public int? PlanId { get; set; }

    public string TiempoComida { get; set; } = null!;

    public string GeneradaPor { get; set; } = null!;

    public DateTime FechaGeneracion { get; set; }

    public virtual Pacientes Paciente { get; set; } = null!;

    public virtual PlanesNutricionales? Plan { get; set; }

    public virtual Recetas Receta { get; set; } = null!;

    public virtual ICollection<RecetaAdaptadaDetalle> RecetaAdaptadaDetalle { get; set; } = new List<RecetaAdaptadaDetalle>();
}
