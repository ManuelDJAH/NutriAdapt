using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class GruposAlimenticios
{
    public int GrupoId { get; set; }

    public string NombreGrupo { get; set; } = null!;

    public decimal KcalPorPorcion { get; set; }

    public decimal ProteinaPorPorcion { get; set; }

    public decimal HidratosPorPorcion { get; set; }

    public decimal LipidosPorPorcion { get; set; }

    public virtual ICollection<Alimentos> Alimentos { get; set; } = new List<Alimentos>();

    public virtual ICollection<PlanPorciones> PlanPorciones { get; set; } = new List<PlanPorciones>();
}
