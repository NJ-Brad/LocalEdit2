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

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddBlazorDownloadFile();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddHttpClient<IAddressDataService, AddressDataService>(x => x.BaseAddress = new Uri("https://api.myip.com"));
builder.Services.AddHttpClient<ILaunchItemDataService, LaunchItemDataService>(x => x.BaseAddress = new Uri("https://fdo.rocketlaunch.live"));
builder.Services.AddHttpClient<IDocumentDataService, DocumentDataService>(x => x.BaseAddress = new Uri("https://localeditfunctions.azurewebsites.net/"));
//builder.Services.AddHttpClient<IDocumentDataService, DocumentDataService>(x => x.BaseAddress = new Uri("http://localhost:7049/"));
builder.Services.AddHttpClient<IDocumentIndexDataService, DocumentIndexDataService>(x => x.BaseAddress = new Uri("https://localeditfunctions.azurewebsites.net/"));

//builder.Services.AddMermaidJS(options =>
//{
//    options.MaxTextSize = 100000;
//    options.SecurityLevel = MermaidSecurityLevels.Loose;
//    //options.SecurityLevel = MermaidSecurityLevels.AntiScript;
//});

//builder.Services.AddBlazorPanzoomServices();

await builder.Build().RunAsync();
