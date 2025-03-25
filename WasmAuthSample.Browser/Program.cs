using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Microsoft.Extensions.DependencyInjection;
using WasmAuthSample;
using WasmAuthSample.Browser;
using WasmAuthSample.PlatformAbstractions;

internal sealed partial class Program
{
    private static Task Main(string[] args) => BuildAvaloniaApp()
        .WithInterFont()
        .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
    {
        App.Services.AddSingleton<IAuthenticationService, BrowserAuthenticationService>();
        
        return AppBuilder.Configure<App>();
    }
}