using Blazorise;
using LocalEdit.FlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class FlowItemModal : LE_ModalBase
    {
        [Parameter]
        public FlowItem Item { get; set; } = new();

    }
}
