using Blazorise;
using LocalEdit.SequenceTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class SequenceItemModal : LE_ModalBase
    {
        [Parameter]
        public SequenceItem Item { get; set; } = new();

    }
}
