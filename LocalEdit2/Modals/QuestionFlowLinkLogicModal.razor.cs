using Blazorise;
using LocalEdit2.QuestionFlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Modals
{
    public partial class QuestionFlowLinkLogicModal : LE_ModalBase
    {
        [Parameter]
        public LinkLogic Item { get; set; } = new();

        [Parameter]
        public List<QuestionFlowItem> Items { get; set; } = new();

    }
}
