using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriAdapt.Models;
using NutriAdapt.Servicios;

namespace NutriAdapt.Controllers;

public class UsuariosController : Controller
{
    private readonly UserManager<Usuario> userManager;
    private readonly SignInManager<Usuario> signInManager;
    private readonly IRepositorioNutriologos repositorioNutriologos;

    public UsuariosController(UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager,
        IRepositorioNutriologos repositorioNutriologos)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.repositorioNutriologos = repositorioNutriologos;
    }

    [AllowAnonymous]
    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(RegistroViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var usuario = new Usuario
        {
            NombreCompleto = modelo.NombreCompleto,
            Correo = modelo.Correo,
            RolId = RolesUsuario.NutriologoId
        };

        var resultado = await userManager.CreateAsync(usuario, modelo.Password);

        if (!resultado.Succeeded)
        {
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(modelo);
        }

        try
        {
            await repositorioNutriologos.Crear(usuario.UsuarioId, modelo.CedulaProfesional, modelo.Especialidad);
        }
        catch
        {
            // Sin perfil de nutriologo el usuario queda huerfano: se elimina
            await userManager.DeleteAsync(usuario);
            throw;
        }

        await signInManager.SignInAsync(usuario, isPersistent: true);
        return RedirectToAction("Index", "Home", new { area = RolesUsuario.Nutriologo });
    }

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel modelo, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        var resultado = await signInManager.PasswordSignInAsync(modelo.Correo, modelo.Password,
            modelo.Recuerdame, lockoutOnFailure: false);

        if (!resultado.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
            return View(modelo);
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        // Home/Index redirige al panel segun el rol
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
}
