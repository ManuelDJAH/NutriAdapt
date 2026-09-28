using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class PlanPorciones
{
    public int PlanPorcionId { get; set; }

    public int PlanId { get; set; }

    public int GrupoId { get; set; }

    public string TiempoComida { get; set; } = null!;

    public decimal CantidadPorciones { get; set; }

    public virtual GruposAlimenticios Grupo { get; set; } = null!;

    public virtual PlanesNutricionales Plan { get; set; } = null!;
}
