using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Quizes.Web;
using Quizes.Web.Apis.Interfaces;
using Quizes.Web.Auth;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder
    .Services
    .AddCascadingAuthenticationState();
builder
    .Services
    .AddDevExpressBlazor();
builder
    .Services
    .AddSingleton<QuizAuthStateProvider>();
builder
    .Services
    .AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<QuizAuthStateProvider>());
builder
    .Services
    .AddAuthorizationCore();

ConfigureRefit(builder.Services);

await builder.Build().RunAsync();

static void ConfigureRefit(IServiceCollection services)
{
    //const string ApiBaseUrl = "https://tkz3266f-7183.euw.devtunnels.ms/";
    const string ApiBaseUrl = "https://localhost:7183";

    services
        .AddRefitClient<IAuthApi>()
        .ConfigureHttpClient(SetHttpClient);
    services
        .AddRefitClient<ICategoryApi>(GetRefitSettings)
        .ConfigureHttpClient(SetHttpClient);

    static void SetHttpClient(HttpClient httpClient)
        => httpClient.BaseAddress = new Uri(ApiBaseUrl);

    static RefitSettings GetRefitSettings(IServiceProvider sp)
    {
        var authStateProvider = sp.GetRequiredService<QuizAuthStateProvider>();
        var a = Task.FromResult(authStateProvider.User?.Token ?? "");

        return new RefitSettings
        {
            AuthorizationHeaderValueGetter = (_, __) => Task.FromResult(authStateProvider.User?.Token ?? "")
        };
    }
}