using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class Alimentos
{
    public int AlimentoId { get; set; }

    public int GrupoId { get; set; }

    public string NombreAlimento { get; set; } = null!;

    public string MedidaCasera { get; set; } = null!;

    public decimal GramosPorPorcion { get; set; }

    public virtual GruposAlimenticios Grupo { get; set; } = null!;

    public virtual ICollection<RecetaAdaptadaDetalle> RecetaAdaptadaDetalle { get; set; } = new List<RecetaAdaptadaDetalle>();

    public virtual ICollection<RecetaIngredientes> RecetaIngredientes { get; set; } = new List<RecetaIngredientes>();
}
