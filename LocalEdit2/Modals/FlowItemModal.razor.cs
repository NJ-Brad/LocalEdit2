using Blazorise;
using LocalEdit2.FlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Modals
{
    public partial class FlowItemModal : LE_ModalBase
    {
        [Parameter]
        public FlowItem Item { get; set; } = new();

    }
}
