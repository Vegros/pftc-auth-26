using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using pftc_auth.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Set credentials env var early (ok)
Environment.SetEnvironmentVariable(
    "GOOGLE_APPLICATION_CREDENTIALS",
    builder.Configuration["Authentication:Google:Credentials"]
);

builder.Services.AddSingleton<FirestoreRepository>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

        options.Scope.Add("profile");
        options.Scope.Add("email"); // add this, since you read email

        options.Events.OnCreatingTicket = context =>
        {
            var email = context.User.GetProperty("email").GetString();
            var picture = context.User.GetProperty("picture").GetString();

            if (!string.IsNullOrEmpty(email))
                context.Identity?.AddClaim(new Claim(ClaimTypes.Email, email));

            if (!string.IsNullOrEmpty(picture))
                context.Identity?.AddClaim(new Claim("picture", picture));

            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();