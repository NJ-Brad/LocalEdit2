using Blazorise;
using LocalEdit2.PlanTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Modals
{
    public partial class SprintModal : LE_ModalBase
    {
        bool adding = false;

        [Parameter]
        public Sprint Item { get; set; } = new();
    }
}
