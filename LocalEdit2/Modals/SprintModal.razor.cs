using Blazorise;
using LocalEdit.PlanTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class SprintModal : LE_ModalBase
    {
        bool adding = false;

        [Parameter]
        public Sprint Item { get; set; } = new();
    }
}
