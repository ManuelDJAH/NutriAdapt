using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriAdapt.Models;

namespace NutriAdapt.Areas.Nutriologo.Controllers;

[Area("Nutriologo")]
[Authorize(Roles = RolesUsuario.Nutriologo)]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
