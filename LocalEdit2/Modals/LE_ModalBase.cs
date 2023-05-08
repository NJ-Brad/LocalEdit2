using Blazorise;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public class LE_ModalBase : ComponentBase
    {
        protected Modal? modalRef;
        private bool cancelClose;
        protected bool modalVisible;
        //private bool cancelled = false;
        //private bool validationRequired = false;

        public ModalResult Result { get; protected set; }


        [Parameter] public EventCallback Closed { get; set; }

        public Task ShowModal()
        {
            modalVisible = true;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        protected Task OnModalClosing(ModalClosingEventArgs e)
        {
            // just set Cancel to prevent modal from closing

            if ((e.CloseReason == CloseReason.EscapeClosing)
                || (Result == ModalResult.Unknown))
            {
                Result = ModalResult.Cancel;

                CloseModal();
            }

            //            if (cancelClose || e.CloseReason != CloseReason.UserClosing)
            if (cancelClose)
            {
                e.Cancel = true;
            }

            // reset - This covers the case where a user clicks Save (and gets an error), then clicks escape
            //validationRequired = false;
            cancelClose = false;

            return Task.CompletedTask;
        }

        protected Task CloseModal()
        {
            // possibly add a check for changed and prompt to lose changes

            cancelClose = false;
            //cancelled = true;
            Result = ModalResult.Cancel;

            if(modalRef != null)
                modalRef.Hide();

            return Closed.InvokeAsync();
        }

        protected async Task OnModalOpened()
        {
            // reset, for the next attempt to close
            cancelClose = false;
            //cancelled = false;
            await ResetValidation();
            await Opened();
        }

        public virtual async Task Opened()
        {
            await InvokeAsync(() => StateHasChanged());
        }

        public Validations? validations { get; set; }
        public virtual async Task<bool> Validate()
        {
            bool rtnVal = false;
            if (validations != null)
            {
                if (await validations.ValidateAll())
                {
                    rtnVal = true;
                }
            }
            else
                rtnVal = true;

            return rtnVal;
        }

        public virtual async Task ResetValidation()
        {
            if (validations != null)
                await validations.ClearAll();
        }


        protected async Task TryCloseModal()
        {
            // add a check for validity
            //validationRequired = true;

            Result = ModalResult.OK;
            cancelClose = true;

            //            if (await propEditor.IsValid())
            if (await Validate())
            {
                cancelClose = false;
            }

            if(modalRef != null)
                await modalRef.Hide();

            await Closed.InvokeAsync();
        }
    }
}
