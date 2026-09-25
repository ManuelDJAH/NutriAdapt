using Microsoft.AspNetCore.Mvc;
using NutriAdapt.Models;

namespace NutriAdapt.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var modelo = new MensajePrueba
        {
            Contenido = "Hola desde el Controller",
            FechaGenerado = DateTime.Now
        };

        return View(modelo);
    }
}
