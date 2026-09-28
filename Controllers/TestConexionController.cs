using Microsoft.AspNetCore.Mvc;
using NutriAdapt.Data;

namespace NutriAdapt.Controllers;

public class TestConexionController : Controller
{
    private readonly NutriAdaptContext _context;

    public TestConexionController(NutriAdaptContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        if (_context.Database.CanConnect())
        {
            return Content("Conexión exitosa a la base de datos de NutriAdapt.");
        }

        return Content("No se pudo conectar a la base de datos de NutriAdapt.");
    }
}
