using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriAdapt.Models;

namespace NutriAdapt.Areas.Paciente.Controllers;

[Area("Paciente")]
[Authorize(Roles = RolesUsuario.Paciente)]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
