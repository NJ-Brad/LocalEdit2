using Blazorise;
using LocalEdit.FlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class FlowRelationshipModal : LE_ModalBase
    {
        [Parameter]
        public FlowRelationship Item { get; set; } = new();

        [Parameter]
        public List<FlowItem> Items { get; set; } = new();

    }
}
