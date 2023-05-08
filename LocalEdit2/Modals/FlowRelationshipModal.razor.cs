using Blazorise;
using LocalEdit2.FlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Modals
{
    public partial class FlowRelationshipModal : LE_ModalBase
    {
        [Parameter]
        public FlowRelationship Item { get; set; } = new();

        [Parameter]
        public List<FlowItem> Items { get; set; } = new();

    }
}
