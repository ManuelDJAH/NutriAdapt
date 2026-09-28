using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class Recetas
{
    public int RecetaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? TipoPlatillo { get; set; }

    public decimal PorcionesRendimientoOriginal { get; set; }

    public string? Instrucciones { get; set; }

    public virtual ICollection<RecetaIngredientes> RecetaIngredientes { get; set; } = new List<RecetaIngredientes>();

    public virtual ICollection<RecetasAdaptadas> RecetasAdaptadas { get; set; } = new List<RecetasAdaptadas>();
}
