using WasmAuthSample.PlatformAbstractions;

namespace WasmAuthSample.Browser;

public class BrowserAuthenticationService : IAuthenticationService
{
    public void Login()
    {
        string currentLocation = JavaScriptInterop.GetWindowLocation();
        JavaScriptInterop.SetWindowLocation($"https://localhost:7050/api/account/login?returnUrl={currentLocation}&selectAccount=true");
    }
}