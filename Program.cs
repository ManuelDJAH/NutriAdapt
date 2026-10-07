using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NutriAdapt.Data;
using NutriAdapt.Models;
using NutriAdapt.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NutriAdaptContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NutriAdaptConnection")));

builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddTransient<IRepositorioNutriologos, RepositorioNutriologos>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();

// Identity sobre la tabla Usuarios (UsuarioStore + Dapper), sin tablas AspNet*
builder.Services.AddTransient<IUserStore<Usuario>, UsuarioStore>();
builder.Services.AddIdentityCore<Usuario>()
    .AddSignInManager()
    .AddClaimsPrincipalFactory<UsuarioClaimsPrincipalFactory>()
    .AddErrorDescriber<MensajesDeErrorIdentity>();
builder.Services.AddTransient<IPasswordHasher<Usuario>, PasswordHasherTextoPlano>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignOutScheme = IdentityConstants.ApplicationScheme;
}).AddCookie(IdentityConstants.ApplicationScheme, opciones =>
{
    opciones.LoginPath = "/Usuarios/Login";
    opciones.AccessDeniedPath = "/Usuarios/AccesoDenegado";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
