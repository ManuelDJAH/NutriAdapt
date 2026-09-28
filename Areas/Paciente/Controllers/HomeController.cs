using Microsoft.AspNetCore.Mvc;

namespace NutriAdapt.Areas.Paciente.Controllers;

[Area("Paciente")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
