using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace WasmAuthSample.Browser;

[SupportedOSPlatform("browser")]
internal static partial class JavaScriptInterop
{
    [JSImport("globalThis.GetWindowLocation")]
    internal static partial string GetWindowLocation();
    
    [JSImport("globalThis.SetWindowLocation")]
    internal static partial string SetWindowLocation(string location);
}