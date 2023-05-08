using Blazorise;
using LocalEdit.QuestionFlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit.Modals
{
    public partial class QuestionFlowItemModal : LE_ModalBase
    {
        [Parameter]
        public QuestionFlowItem Item { get; set; } = new();

        [Parameter]
        public List<QuestionFlowItem> Items { get; set; } = new();

        public QuestionFlowRelationship? SelectedRelationshipRow { get; set; } = new();
        private QuestionFlowRelationshipModal? QuestionFlowRelationshipModalRef;

        // setting to null allows the toolbar buttons to enable/disable properly
        public LinkLogic? SelectedLinkLogicRow { get; set; } = null;
        private QuestionFlowLinkLogicModal? QuestionFlowLinkLogicModalRef;

        bool adding = false;

        private Task ShowRelationshipModal()
        {
            if (SelectedRelationshipRow == null)
            {
                return Task.CompletedTask;
            }
            if (QuestionFlowRelationshipModalRef != null)
            { 
                //QuestionFlowRelationshipModalRef.Item = SelectedRelationshipRow;

                QuestionFlowRelationshipModalRef?.ShowModal();
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
            if (QuestionFlowLinkLogicModalRef != null)
            {
                QuestionFlowLinkLogicModalRef?.ShowModal();
            }

            return Task.CompletedTask;
        }

        private Task AddNewRelationship()
        {
            QuestionFlowRelationship newRelationship = new()
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

            foreach (QuestionFlowItem fi in Items)
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

        private Task OnQuestionFlowRelationshipModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (QuestionFlowRelationshipModalRef?.Result == ModalResult.Cancel)
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

        private Task OnQuestionFlowLinkLogicModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (QuestionFlowLinkLogicModalRef?.Result == ModalResult.Cancel)
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
