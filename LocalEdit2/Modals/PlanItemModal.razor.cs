using Blazorise;
using LocalEdit.PlanTypes;
using Microsoft.AspNetCore.Components;
using Octokit;

namespace LocalEdit.Modals
{
    public partial class PlanItemModal : LE_ModalBase
    {
        private string DecodePlanItemId(string id)
        {
            string rtnVal = id;

            foreach (PlanItem fi in Items)
            {
                if (fi.ID == id)
                {
                    rtnVal = fi.Label == null ? "" : fi.Label;
                    break;
                }
            }

            return rtnVal;
        }

//        DateTime? earliestDate;

//        void OnDateChanged(DateTime? date)
//        {
////            earliestDate = date;

//            int numItems = Item.Dependencies.Count;

//            for (int itemNum = numItems - 1; itemNum > -1; itemNum--)
//            {
//                if (Item.Dependencies[itemNum].DependencyType == "DATE")
//                {
//                    Item.Dependencies.RemoveAt(itemNum);
//                }
//            }

//            if (date.HasValue)
//            {
//                PlanItemDependency newItem = new PlanItemDependency();
//                newItem.DependencyType = "DATE";
//                newItem.StartDate = date.Value.ToShortDateString();
//                Item.Dependencies.Add(newItem);
//            }
//        }

        void OnRememberMeChanged2(string value)
        {
            if (IsChecked(value))
            {
                RemoveItemDependency(value);
            }
            else
            {
                AddItemDependency(value);
            }
        }

        private void AddItemDependency(string value)
        {
            PlanItemDependency newItem = new PlanItemDependency();
            newItem.DependencyType = "OTHER";
            newItem.ID = value;
            //    < SelectItem Value =@("DATE") > Date </ SelectItem >
            //< SelectItem Value =@("OTHER") > Other Item </ SelectItem >
            Item.Dependencies.Add(newItem);
        }

        private void RemoveItemDependency(string value)
        {
            int idx = GetDependencyIndex(value);

            if (idx != -1)
            {
                Item.Dependencies.RemoveAt(idx);
            }
        }

        private int GetDependencyIndex(string value)
        {
            int rtnVal = -1;
            try
            {
                int numItems = (Item != null) ? Item.Dependencies.Count : 0;

                for (int itemNum = 0; itemNum < numItems; itemNum++)
                {
                    if (Item.Dependencies[itemNum].ID == value)
                    {
                        rtnVal = itemNum;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return rtnVal;
        }


        bool IsChecked(string value)
        {
            return (GetDependencyIndex(value) != -1);
        }

        PlanDependencyModal? planDependencyModalRef = null;
        bool adding = false;

        //        PlanItemDependency? SelectedDependencyRow { get; set; } = null;
        PlanItemDependency? SelectedDependencyRow { get; set; } = new();

        private Task EditDependency()
        {
            if (SelectedDependencyRow == null)
            {
                return Task.CompletedTask;
            }

            if (planDependencyModalRef != null)
            {
                //planDependencyModalRef.Item = selectedDependencyRow;

                planDependencyModalRef?.ShowModal();
            }
            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewDependency()
        {
            PlanItemDependency newItem = new PlanItemDependency();

            SelectedDependencyRow = newItem;
            Item.Dependencies.Add(newItem);
            adding = true;

            return EditDependency();
        }

        private Task OnDependencyModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (planDependencyModalRef?.Result == ModalResult.Cancel)
                {
                    if (SelectedDependencyRow != null)
                    {
                        Item.Dependencies.Remove(SelectedDependencyRow);
                        SelectedDependencyRow = null;
                    }
                }
            }
            adding = false;

            return Task.CompletedTask;
        }

        private Task DeleteDependency()
        {
            if (SelectedDependencyRow != null)
            {
                Item.Dependencies.Remove(SelectedDependencyRow);
                SelectedDependencyRow = null;
            }

            return Task.CompletedTask;
        }



        [Parameter]
        public PlanItem Item { get; set; } = new();

        [Parameter]
        public List<PlanItem> Items { get; set; } = new();
    }
}
