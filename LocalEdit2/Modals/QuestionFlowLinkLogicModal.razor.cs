using Blazorise;
using LocalEdit.QuestionFlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class QuestionFlowLinkLogicModal : LE_ModalBase
    {
        [Parameter]
        public LinkLogic Item { get; set; } = new();

        [Parameter]
        public List<QuestionFlowItem> Items { get; set; } = new();

    }
}
