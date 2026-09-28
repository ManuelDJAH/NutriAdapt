using Microsoft.AspNetCore.Mvc;

namespace NutriAdapt.Areas.Nutriologo.Controllers;

[Area("Nutriologo")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
