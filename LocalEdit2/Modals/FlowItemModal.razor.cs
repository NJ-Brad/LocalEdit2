using Blazorise;
using LocalEdit2.FlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Modals
{
    public partial class FlowItemModal : LE_ModalBase
    {
        [Parameter]
        public FlowItem Item { get; set; } = new();

        [Parameter]
        public List<FlowItem> Items { get; set; } = new();

        public FlowRelationship? SelectedRelationshipRow { get; set; } = new();
        private FlowRelationshipModal? FlowRelationshipModalRef;

        // setting to null allows the toolbar buttons to enable/disable properly
        public LinkLogic? SelectedLinkLogicRow { get; set; } = null;
        private FlowLinkLogicModal? FlowLinkLogicModalRef;

        bool adding = false;

        private Task ShowRelationshipModal()
        {
            if (SelectedRelationshipRow == null)
            {
                return Task.CompletedTask;
            }
            if (FlowRelationshipModalRef != null)
            { 
                //FlowRelationshipModalRef.Item = SelectedRelationshipRow;

                FlowRelationshipModalRef?.ShowModal();
            }
            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ShowLinkLogicModal()
        {
            if (SelectedLinkLogicRow == null)
            {
                return Task.CompletedTask;
            }
            if (FlowLinkLogicModalRef != null)
            {
                FlowLinkLogicModalRef?.ShowModal();
            }

            return Task.CompletedTask;
        }

        private Task AddNewRelationship()
        {
            FlowRelationship newRelationship = new()
            {
                Label = "New Relationship"
            };

            SelectedRelationshipRow = newRelationship;
            Item?.NextQuestions?.Add(newRelationship);
            adding = true;

            return ShowRelationshipModal();
        }

        private Task AddNewLinkLogic()
        {
            LinkLogic newLinkLogic = new()
            {
                //Label = "New Relationship"
            };

            SelectedLinkLogicRow = newLinkLogic;
            Item?.linkLogic?.Add(newLinkLogic);
            adding = true;

            return ShowLinkLogicModal();
        }

        private string DecodeQuestionFlowId(string id)
        {
            string rtnVal = id;

            foreach (FlowItem fi in Items)
            {
                if (fi.id == id)
                {
                    rtnVal = fi.title;
                    break;
                }
            }

            return rtnVal;
        }

        private Task DeleteRelationship()
        {
            if (SelectedRelationshipRow != null)
            {
                Item?.NextQuestions?.Remove(SelectedRelationshipRow);
                SelectedRelationshipRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task DeleteLinkLogic()
        {
            if (SelectedLinkLogicRow != null)
            {
                Item?.linkLogic?.Remove(SelectedLinkLogicRow);
                SelectedLinkLogicRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnFlowRelationshipModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (FlowRelationshipModalRef?.Result == ModalResult.Cancel)
                {
                    if (SelectedRelationshipRow != null)
                    {
                        Item?.NextQuestions?.Remove(SelectedRelationshipRow);
                        SelectedRelationshipRow = null;
                    }
                }
            }
            adding = false;

//          if(SelectedRelationshipRow != null)
//            {
//                SelectedRelationshipRow.DecodedFlowId = DecodeQuestionFlowId(SelectedRelationshipRow.To);
//            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnFlowLinkLogicModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (FlowLinkLogicModalRef?.Result == ModalResult.Cancel)
                {
                    if (SelectedLinkLogicRow != null)
                    {
                        Item?.linkLogic?.Remove(SelectedLinkLogicRow);
                        SelectedLinkLogicRow = null;
                    }
                }
            }
            adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

    }
}
