using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;  
using Microsoft.AspNetCore.Authentication.Google;

namespace pftc_auth.Controllers;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        var redirectUrl = Url.Action("GoogleResponse", "Account");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(); 
        if(!result.Succeeded)
            return RedirectToAction("Login");
        return RedirectToAction("Index", "Home");
    }
    public IActionResult Logout()
    {
        return SignOut("Cookies");
    }
}