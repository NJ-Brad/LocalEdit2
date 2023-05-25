using LocalEdit2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Blazored.LocalStorage;
using LocalEdit2.IPAddressTypes;
using LocalEdit2.LaunchTypes;
using LocalEdit2.PlanTypes;
using LocalEdit2.DocumentTypes;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using LocalEdit2.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//.AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, GraphUserAccountFactory>();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddScoped<FunctionAuthorizationMessageHandler>();

builder.Services.AddBlazorDownloadFile();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddHttpClient<IAddressDataService, AddressDataService>(x => x.BaseAddress = new Uri("https://api.myip.com"));
builder.Services.AddHttpClient<ILaunchItemDataService, LaunchItemDataService>(x => x.BaseAddress = new Uri("https://fdo.rocketlaunch.live"));

builder.Services.AddHttpClient<IDocumentDataService, DocumentDataService>(x => x.BaseAddress = new Uri("https://localeditfunctions.azurewebsites.net/"));
//builder.Services.AddHttpClient<IDocumentDataService, DocumentDataService>(x => x.BaseAddress = new Uri("http://localhost:7049"));

//builder.Services.AddHttpClient<IDocumentDataService, DocumentDataService>("Functions", x => x.BaseAddress = new Uri("http://localhost:7049/"))
//    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

//builder.Services.AddHttpClient<IDocumentDataService, DocumentDataService>("Functions", x => x.BaseAddress = new Uri("http://localhost:7049"))
//    .AddHttpMessageHandler<FunctionAuthorizationMessageHandler>();


builder.Services.AddHttpClient<IDocumentIndexDataService, DocumentIndexDataService>(x => x.BaseAddress = new Uri("https://localeditfunctions.azurewebsites.net/"));

// https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-7.0#attach-tokens-to-outgoing-requests
//builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
//    .CreateClient("Functions"));

// https://stackoverflow.com/questions/68553637/blazor-standalone-wasm-unable-to-get-access-token-with-msal

// https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/additional-scenarios?view=aspnetcore-7.0#attach-tokens-to-outgoing-requests
//string baseUrlForAuth = builder.HostEnvironment.BaseAddress;


builder.Services.AddMsalAuthentication(options =>
{
    // inspired by : https://github.com/dotnet/aspnetcore/issues/39104
    options.ProviderOptions.LoginMode = "redirect";
    // this one wasn't required
    options.ProviderOptions.Cache.CacheLocation = "localStorage";

    //options.ProviderOptions.DefaultAccessTokenScopes.Add("http://localhost:7049"); // API

    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
});


//builder.Services.AddMermaidJS(options =>
//{
//    options.MaxTextSize = 100000;
//    options.SecurityLevel = MermaidSecurityLevels.Loose;
//    //options.SecurityLevel = MermaidSecurityLevels.AntiScript;
//});

//builder.Services.AddBlazorPanzoomServices();

await builder.Build().RunAsync();
