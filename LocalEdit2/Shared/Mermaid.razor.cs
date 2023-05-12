using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Text;

namespace LocalEdit2.Shared
{
    public partial class Mermaid : ComponentBase
    {
        [Inject]
        protected IJSRuntime? JsRuntime { get; set; } = null;

        // https://stackoverflow.com/questions/58346600/why-do-blazor-components-and-elements-not-have-id-attributes
        [Parameter]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [Parameter]
        public string GraphText { get; set; } = "graph LR \n";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (this.JsRuntime is not null)
            {
                if (firstRender)
                {
                    // This is to get the Id value set.  If this is removed, or too short, the graph will not display
                    // AND if you don't await it, things are dicey.  DAMHIKT
                    await Task.Delay(100);
                    await this.JsRuntime.InvokeVoidAsync("JsFunctions.MermaidInitialize");
                    await OnParametersSetAsync();
                }

                //                await this.JsRuntime.InvokeVoidAsync("JsFunctions.MermaidRender");
            }
        }

        protected override async Task<Task> OnParametersSetAsync()
        {
            if (this.JsRuntime is not null)
            {
                await this.JsRuntime.InvokeVoidAsync("JsFunctions.MermaidRender2Async", this.Id, GraphText);
            }
            return base.OnParametersSetAsync();
        }
    }
}
