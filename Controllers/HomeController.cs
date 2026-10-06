using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NutriAdapt.Models;

namespace NutriAdapt.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        // Pagina inicial publica; con sesion iniciada se va directo al panel de su rol
        if (User.Identity?.IsAuthenticated == true)
        {
            var area = User.IsInRole(RolesUsuario.Nutriologo) ? RolesUsuario.Nutriologo : RolesUsuario.Paciente;
            return RedirectToAction("Index", "Home", new { area });
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
