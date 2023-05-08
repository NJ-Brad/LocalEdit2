using Blazorise;
using LocalEdit.SequenceTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class SequenceRelationshipModal : LE_ModalBase
    {
        [Parameter]
        public SequenceRelationship Item { get; set; } = new();

        [Parameter]
        public List<SequenceItem> Items { get; set; } = new();

    }
}
