using Blazorise;
using LocalEdit2.SequenceTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Modals
{
    public partial class SequenceItemModal : LE_ModalBase
    {
        [Parameter]
        public SequenceItem Item { get; set; } = new();

    }
}
