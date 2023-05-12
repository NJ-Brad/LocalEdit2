using Blazorise;
using LocalEdit2.FlowTypes;
using Microsoft.AspNetCore.Components;

namespace LocalEdit2.Modals
{
    public partial class FlowItemModal : LE_ModalBase
    {
        [Parameter]
        public FlowItem Item { get; set; } = null;

        [Parameter]
        public List<FlowItem> Items { get; set; } = new();

        public FlowRelationship? SelectedRelationshipRow { get; set; } = null;
        private FlowRelationshipModal? FlowRelationshipModalRef;

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

        private Task AddNewRelationship()
        {
            FlowRelationship newRelationship = new()
            {
                From = Item?.id,
                Label = "New Relationship"
            };

            SelectedRelationshipRow = newRelationship;
            Item?.NextItems?.Add(newRelationship);
            adding = true;

            return ShowRelationshipModal();
        }

        private string DecodeFlowId(string id)
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
                Item?.NextItems?.Remove(SelectedRelationshipRow);
                SelectedRelationshipRow = null;
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
                        Item?.NextItems?.Remove(SelectedRelationshipRow);
                        SelectedRelationshipRow = null;
                    }
                }
            }
            adding = false;

//          if(SelectedRelationshipRow != null)
//            {
//                SelectedRelationshipRow.DecodedFlowId = DecodeFlowId(SelectedRelationshipRow.To);
//            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }
    }
}
