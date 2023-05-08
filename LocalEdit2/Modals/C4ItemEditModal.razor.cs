using Blazorise;
using LocalEdit.C4Types;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Xml.Linq;

namespace LocalEdit.Modals
{
    public partial class C4ItemEditModal : LE_ModalBase
    {
        //private Modal? modalRef;
        public C4TypeEnum ParentType { get; set; } = C4TypeEnum.Unknown;
        public C4Item SelectedNode { get; set; } = new ();

        private Select<C4TypeEnum>? itemTypeCombo { get; set; }

        private List<C4TypeNamePair> c4Types { get; set; } = new();


        protected override void OnParametersSet()
        {
            //if ((itemTypeCombo != null) && (itemTypeCombo.SelectedValue != null))
            //{
            //    C4TypeEnum firstItem = itemTypeCombo.SelectedValue;

            c4Types.Clear();

            if (ShouldShow(C4TypeEnum.Person))
                c4Types.Add(new C4TypeNamePair(C4TypeEnum.Person, "Person"));

            if (ShouldShow(C4TypeEnum.System))
                c4Types.Add(new C4TypeNamePair(C4TypeEnum.System, "System"));

            if (ShouldShow(C4TypeEnum.EnterpriseBoundary))
                c4Types.Add(new C4TypeNamePair(C4TypeEnum.EnterpriseBoundary, "Enterprise"));

            if(ShouldShow(C4TypeEnum.Container))
                c4Types.Add(new C4TypeNamePair(C4TypeEnum.Container, "Container"));

            if (ShouldShow(C4TypeEnum.Component))
                c4Types.Add(new C4TypeNamePair(C4TypeEnum.Component, "Component"));

            if (ShouldShow(C4TypeEnum.Database))
                c4Types.Add(new C4TypeNamePair(C4TypeEnum.Database, "Database"));

            if (ShouldShow(C4TypeEnum.Node))
                c4Types.Add(new C4TypeNamePair(C4TypeEnum.Node, "Node"));

            base.OnParametersSet();
        }

        //private bool cancelClose;
        //private bool modalVisible;
        //private bool cancelled = false;

        //private Task OnModalClosing(ModalClosingEventArgs e)
        //{
        //    // just set Cancel to prevent modal from closing

        //    if (e.CloseReason == CloseReason.EscapeClosing)
        //    {
        //        CloseModal();
        //    }

        //    if (cancelClose || e.CloseReason != CloseReason.UserClosing)
        //    {
        //        e.Cancel = true;
        //    }

        //    return Task.CompletedTask;
        //}

        //private Task CloseModal()
        //{
        //    // possibly add a chack for changed and prompt to lose changes

        //    cancelClose = false;
        //    //cancelled = true;

        //    return modalRef.Hide();
        //}

        Alert? myAlert { get; set; }

        private bool ShouldShow(C4TypeEnum itemType)
        {
            bool rtnVal = false;

            switch (ParentType)
            {
                case C4TypeEnum.Unknown:
                    switch (itemType)
                    {
                        case C4TypeEnum.Person:
                        case C4TypeEnum.System:
                        case C4TypeEnum.EnterpriseBoundary:
                            rtnVal = true;
                            break;
                    }
                    break;
                case C4TypeEnum.Person:
                    // nothing is allowed
                    break;
                case C4TypeEnum.System:
                    switch (itemType)
                    {
                        case C4TypeEnum.Container:
                        case C4TypeEnum.Database:
                            rtnVal = true;
                            break;
                    }
                    break;
                case C4TypeEnum.EnterpriseBoundary:
                    switch (itemType)
                    {
                        case C4TypeEnum.Person:
                        case C4TypeEnum.System:
                            rtnVal = true;
                            break;
                    }
                    break;
                case C4TypeEnum.Container:
                    switch (itemType)
                    {
                        case C4TypeEnum.Component:
                            rtnVal = true;
                            break;
                    }
                    break;
                }
            return rtnVal;
        }

        public override Task Opened()
        {
            if (ParentType == C4TypeEnum.Unknown)
            {
                myAlert?.Show();
            }

            return Task.CompletedTask;
        }


        //private Task OnModalOpened()
        //{
        //    // reset, for the next attempt to close
        //    cancelClose = false;
        //    //            propEditor.ResetValidation();
        //    cancelled = false;

        //    return Task.CompletedTask;
        //}
        //private async Task TryCloseModal()
        //{
        //    // add a check for validity

        //    cancelClose = true;

        //    if (await propEditor.IsValid())
        //    {
        //        cancelClose = false;
        //    }

        //    await modalRef.Hide();
        //}

        bool showA = true;
        //private bool isRegistrationSuccess = false;

    }
}
