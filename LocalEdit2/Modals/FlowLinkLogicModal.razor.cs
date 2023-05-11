using Blazorise;
using LocalEdit2.FlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Modals
{
    public partial class FlowLinkLogicModal : LE_ModalBase
    {
        [Parameter]
        public LinkLogic Item { get; set; } = new();

        [Parameter]
        public List<FlowItem> Items { get; set; } = new();

    }
}
