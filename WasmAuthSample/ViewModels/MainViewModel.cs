using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WasmAuthSample.PlatformAbstractions;

namespace WasmAuthSample.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authenticationService;
    [ObservableProperty] private string _greeting = "Welcome to Avalonia!";

    public MainViewModel(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    [RelayCommand]
    private void Login()
    {
        _authenticationService.Login();
    }
}