using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DSR_Practice_Debts.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

//string connection = builder.Configuration.GetConnectionString("DefaultConnection");

//≈—À» ¬ »Ã≈Õ» —≈–¬≈–¿ Õ≈“ "\\SQLEXPRESS", –¿— ŒÃ≈Õ“»–Œ¬¿“‹ —“–Œ ” 16 » «¿ ŒÃ≈Õ“»–Œ¬¿“‹ —“–Œ ” 18
//string connection = $"Data Source={Environment.MachineName};Initial Catalog=DSR_Practice_Debts;Integrated Security=True;TrustServerCertificate=true";

string connection = $"Data Source={Environment.MachineName}\\SQLEXPRESS;Initial Catalog=DSR_Practice_Debts;Integrated Security=True;TrustServerCertificate=true";
StreamWriter wrt = new StreamWriter("buf.txt");
wrt.Write(connection);
wrt.Close();

builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(connection));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => //CookieAuthenticationOptions
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                });
builder.Services.AddControllersWithViews();

// œÓ‰ÍÎ˛˜ÂÌËÂ ‡‚ÚÓËÁ‡ˆËË
builder.Services.AddAuthorization(opts => {

    opts.AddPolicy("OnlyForAdmin", policy => {
        policy.RequireClaim(ClaimTypes.Role, "Admin");
    });
    opts.AddPolicy("OnlyForUser", policy =>
    {
        policy.RequireClaim(ClaimTypes.Role, "User");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

