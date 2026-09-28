using System;
using System.Collections.Generic;

namespace NutriAdapt.Models;

public partial class Usuarios
{
    public int UsuarioId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte RolId { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Nutriologos? Nutriologos { get; set; }

    public virtual Pacientes? Pacientes { get; set; }
}
