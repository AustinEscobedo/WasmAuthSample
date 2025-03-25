using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
       {
           // Configure the default scheme to be cookie-based
           // All other schemes will be challenge-based and will issue cookies on successful authentication
           options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
       })
       .AddCookie(options =>
       {
           options.Cookie.SameSite = SameSiteMode.Lax;
           options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
           options.Cookie.HttpOnly = true;

           options.Events = new CookieAuthenticationEvents
           {
               OnRedirectToLogin = context =>
               {
                   // Return 401 instead of redirecting to a login page
                   // Client-side code can redirect to the user as required
                   context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                   return Task.CompletedTask;
               },
               OnRedirectToAccessDenied = context =>
               {
                   // Return 403 instead of redirecting to an access denied page
                   // Client-side code can redirect to the user as required
                   context.Response.StatusCode = StatusCodes.Status403Forbidden;
                   return Task.CompletedTask;
               }
           };
       })
       .AddOpenIdConnect(options =>
       {
           options.Authority = builder.Configuration["OpenIdConnect:Authority"];
           options.ClientId = builder.Configuration["OpenIdConnect:ClientId"];
           options.ClientSecret = builder.Configuration["OpenIdConnect:ClientSecret"];
           options.CallbackPath = $"/api/account/login-callback";

           options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           options.ResponseType = OpenIdConnectResponseType.Code;

           options.Scope.Clear();
           options.Scope.Add("openid");
           options.Scope.Add("profile");
           options.Scope.Add("email");
           options.Scope.Add("offline_access");

           options.GetClaimsFromUserInfoEndpoint = true;
           options.SaveTokens = true;

           options.CorrelationCookie.SameSite = SameSiteMode.None;
           options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
           options.NonceCookie.SameSite = SameSiteMode.None;
           options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;

           options.Events = new OpenIdConnectEvents
           {
               OnRedirectToIdentityProvider = context =>
               {
                   if (context.HttpContext.Request.Path == $"/api/account/logout")
                   {
                       context.Response.Clear();
                       context.Response.StatusCode = 200;
                       context.HandleResponse();
                   }
                   else if (context.HttpContext.Request.Path != $"/api/account/login")
                   {
                       context.Response.Clear();
                       context.Response.StatusCode = 401;
                       context.HandleResponse();
                   }

                   return Task.CompletedTask;
               }
           };
       });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .RequireAuthorization();

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}