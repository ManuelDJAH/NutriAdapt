using System.ComponentModel.DataAnnotations;

namespace NutriAdapt.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico válido")]
    [Display(Name = "Correo")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Recuérdame")]
    public bool Recuerdame { get; set; }
}
