using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WasmAuthSample.Api;

[ApiController]
[Route("/api/[controller]")]
public class AccountController : ControllerBase
{
    [HttpGet("Login")]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = "/", bool selectAccount = false)
    {
        // returnUrl should be validated to prevent redirect attacks
        AuthenticationProperties authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(LoginCallback), new {returnUrl})
        };

        if (selectAccount)
        {
            authenticationProperties.SetParameter("prompt", "select_account");
        }

        return Challenge(authenticationProperties, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [HttpGet("Login-Callback")]
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult LoginCallback(string returnUrl = "/")
    {
        if(User.Identity?.IsAuthenticated != true)
            return Unauthorized();
        
        return Redirect(returnUrl);
    }
    
    [HttpPost("Logout")]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}