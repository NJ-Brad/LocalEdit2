using Blazorise;
using LocalEdit2.QuestionFlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Modals
{
    public partial class QuestionFlowRelationshipModal : LE_ModalBase
    {
        [Parameter]
        public QuestionFlowRelationship Item { get; set; } = new();

        [Parameter]
        public List<QuestionFlowItem> Items { get; set; } = new();

    }
}
