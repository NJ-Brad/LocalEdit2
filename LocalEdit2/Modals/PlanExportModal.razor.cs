using Blazorise;
using LocalEdit.PlanTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class PlanExportModal : LE_ModalBase
    {

        // for potential "autoclose"
        // await Task.Delay(...)

        // MarkupString

        public string DiagramType { get; set; } = "gantt";
        public string OutputFormat { get; set; } = "html";

        protected Task ButtonClicked(ModalResult result)
        {
            Result = result;

            if (modalRef != null)
                modalRef.Hide();

            return Closed.InvokeAsync();
        }

        //private bool ButtonVisible(int buttonNumber)
        //{
        //    return ((buttonNumber > 0) && (buttonNumber <= Buttons.Count));
        //}

        //private bool ButtonPrimary(int buttonNumber)
        //{
        //    return (buttonNumber == Buttons.Count);
        //}

}
}
