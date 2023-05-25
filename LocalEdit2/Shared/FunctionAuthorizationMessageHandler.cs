using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Shared
{
    public class FunctionAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public FunctionAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
                authorizedUrls: new[] { "http://localhost:7049" }
//                scopes: new[] { "access_as_user" }
                );
        }
    }
}
