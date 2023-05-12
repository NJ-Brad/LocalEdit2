using Blazorise;
using LocalEdit2.C4Types;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace LocalEdit2.Modals
{
    public partial class C4RelationshipModal : LE_ModalBase
    {
        public override async Task Opened()
        {
            await InvokeAsync(() => StateHasChanged());

            //            return Task.CompletedTask;
            return;
        }


        [Parameter]
        public C4Relationship Item { get; set; } = new();

        [Parameter]
        //        public List<C4Item> Items
#pragma warning disable BL0007 // Component parameters should be auto properties
        public ObservableCollection<C4Item>? Items
#pragma warning restore BL0007 // Component parameters should be auto properties
        { get => items;
            set { 
                items = value;
                if (items != null)
                {
                    GetAllItems(items);
                }
            } 
        }

        private ObservableCollection<C4Item>? items = new ();
        //private List<C4Item> items = new();

        public List<C4Item> AllItems { get; set; } = new();

        private void GetAllItems(ObservableCollection<C4Item> items)
        {
            AllItems.Clear();
            GetAllItems_(items);
        }

        private void GetAllItems_(ObservableCollection<C4Item> items)
        {
            foreach (C4Item item in items)
            {
                AllItems.Add(item);
                GetAllItems_(item.Children);
            }
        }

    }
}
