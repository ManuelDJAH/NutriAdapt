using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NutriAdapt.Models;
using NutriAdapt.Servicios;

namespace NutriAdapt.Areas.Nutriologo.Controllers;

[Area("Nutriologo")]
[Authorize(Roles = RolesUsuario.Nutriologo)]
public class HomeController : Controller
{
    private readonly IRepositorioNutriologos repositorioNutriologos;
    private readonly IServicioUsuarios servicioUsuarios;

    public HomeController(IRepositorioNutriologos repositorioNutriologos,
        IServicioUsuarios servicioUsuarios)
    {
        this.repositorioNutriologos = repositorioNutriologos;
        this.servicioUsuarios = servicioUsuarios;
    }

    public async Task<IActionResult> Index()
    {
        var usuarioId = servicioUsuarios.ObtenerUsuarioId();
        var resumen = await repositorioNutriologos.ObtenerResumen(usuarioId);

        // Usuario con RolId = 1 pero sin fila en Nutriologos (por ejemplo, insertado a mano)
        if (resumen is null)
        {
            return View("SinPerfil");
        }

        return View(resumen);
    }
}
