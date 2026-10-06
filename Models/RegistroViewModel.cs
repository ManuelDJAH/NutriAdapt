using System.ComponentModel.DataAnnotations;

namespace NutriAdapt.Models;

// Registro publico: solo nutriologos. Los pacientes los da de alta su nutriologo.
public class RegistroViewModel
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    [Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico válido")]
    [StringLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    [Display(Name = "Correo")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmarPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(20, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    [Display(Name = "Cédula profesional")]
    public string CedulaProfesional { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    [Display(Name = "Especialidad (opcional)")]
    public string? Especialidad { get; set; }
}
