using Blazorise;
using LocalEdit2.SequenceTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Modals
{
    public partial class SequenceRelationshipModal : LE_ModalBase
    {
        [Parameter]
        public SequenceRelationship Item { get; set; } = new();

        [Parameter]
        public List<SequenceItem> Items { get; set; } = new();

    }
}
