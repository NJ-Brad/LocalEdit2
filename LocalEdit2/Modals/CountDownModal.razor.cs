using Blazorise;
using LocalEdit.CountDownTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class CountDownModal : LE_ModalBase
    {
        //private Modal? modalRef;
        public CountDownConfiguration Configuration { get; set; } = new();

        //Alert myAlert;

        //public override Task Opened()
        //{
        //    if (ParentType == C4TypeEnum.Unknown)
        //    {
        //        myAlert.Show();
        //    }

        //    return Task.CompletedTask;
        //}
    }
}
