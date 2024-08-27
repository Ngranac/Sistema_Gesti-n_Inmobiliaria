using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Index"; // Ruta de inicio de sesi�n
        options.LogoutPath = "/Logout"; // Ruta de cierre de sesi�n
        options.AccessDeniedPath = "/AccessDenied"; // Ruta cuando el acceso est� denegado
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapGet("/test-connection", async (HttpContext context) =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    try
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            await context.Response.WriteAsync("Conexi�n exitosa a la base de datos.");
        }
    }
    catch (Exception ex)
    {
        await context.Response.WriteAsync($"Error al conectar a la base de datos: {ex.Message}");
    }
});

app.Run();


