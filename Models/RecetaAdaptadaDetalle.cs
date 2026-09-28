using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class RecetaAdaptadaDetalle
{
    public int DetalleId { get; set; }

    public int RecetaAdaptadaId { get; set; }

    public int AlimentoId { get; set; }

    public decimal CantidadAjustada { get; set; }

    public string UnidadMedida { get; set; } = null!;

    public decimal FactorEscala { get; set; }

    public virtual Alimentos Alimento { get; set; } = null!;

    public virtual RecetasAdaptadas RecetaAdaptada { get; set; } = null!;
}
