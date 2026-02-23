using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies; 
using Microsoft.AspNetCore.Authentication.Google; 


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
}).AddCookie().AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.Scope.Add("profile");
    options.Events.OnCreatingTicket = (context) =>
    {
        var email = context.User.GetProperty("email").GetString();
        var picture = context.User.GetProperty("picture").GetString();
        context.Identity.AddClaim(new Claim("email", email));
        context.Identity.AddClaim(new Claim("picture", picture));
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization(); 

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();