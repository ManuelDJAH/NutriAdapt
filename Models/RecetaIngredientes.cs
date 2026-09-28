using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class RecetaIngredientes
{
    public int RecetaIngredienteId { get; set; }

    public int RecetaId { get; set; }

    public int AlimentoId { get; set; }

    public decimal CantidadOriginal { get; set; }

    public string UnidadMedida { get; set; } = null!;

    public decimal PorcionesGrupoOriginal { get; set; }

    public virtual Alimentos Alimento { get; set; } = null!;

    public virtual Recetas Receta { get; set; } = null!;
}
