using Blazorise;
using System.Collections.ObjectModel;

namespace LocalEdit2.C4Types
{
    public class C4FlatItemCollection : ObservableCollection<C4FlatItem>
    {
        public void ConvertFromTree(ObservableCollection<C4Item> tree)
        {
            // Not sure which is better.  Look into this later
            base.Clear();
            //base.ClearItems();

            ProcessItemsFromTree(tree, "", 0);

            System.Console.WriteLine(Count);
        }

        private void ProcessItemsFromTree(ObservableCollection<C4Item> tree, string parentAlias, int level)
        {
            foreach (var item in tree)
            {
                Add(new C4FlatItem(item, parentAlias, level));
                ProcessItemsFromTree(item.Children, item.Alias, level + 1);
            }
        }

        public ObservableCollection<C4Item> ConvertToTree()
        {
            ObservableCollection<C4Item> rtnVal = new ObservableCollection<C4Item>();

            // option 1:
            // One run through the list.
            // The parent has been created (higher in the list) before the child is processed (beneath it in the list)
            // Cannot handle out of order children

            // option 2:
            // this is recursive.
            // Add all of the items without a parent.
            // For each one, add their children.
            // (Multiple runs through the list)
            // (List is small - shouldn't be too big a performance hit
            // Handles out of order items
            ProcessItemsToTree(rtnVal, "");

            return rtnVal;
        }

        private void ProcessItemsToTree(ObservableCollection<C4Item> tree, string parentAlias)
        {
            foreach (var item in base.Items)
            {
                if (item.ParentAlias.Equals(parentAlias, StringComparison.OrdinalIgnoreCase))
                {
                    C4Item newItem = new C4Item 
                    {
                        ItemType = item.ItemType,
                        Text = item.Text,
                        IsDatabase = item.IsDatabase,
                        IsExternal = item.IsExternal,
                        Alias = item.Alias,
                        Description = item.Description,
                        Technology = item.Technology
                    };
                    tree.Add(newItem);
                    ProcessItemsToTree(newItem.Children, newItem.Alias);
                }
            }
        }

        public C4FlatItem FindByAlias(string alias)
        {
            C4FlatItem rtnVal = null;

            foreach (var item in base.Items)
            {
                if(item.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase))
                {
                    rtnVal = item;
                    break;
                }
            }

            return rtnVal;
        }

        // Add a new item, at this level, after this item and before the next item at this level
        public int GetAddIndex(C4FlatItem? itemBefore)
        {
            int rtnVal = -1;

            if (itemBefore != null)
            {
                int itemIndex = IndexOf(itemBefore);

                int level = itemBefore.Level;

                for(int here = itemIndex + 1; here< Count; here++)
                {
                    C4FlatItem item = base.Items[here];
                    if(item.Level <= level)
                    {
                        rtnVal = here;
                        break;
                    }
                }
            }

            return rtnVal;
        }

        // Add a new item, at this level, after this item and before the next item at this level
        public int GetAddChildIndex(C4FlatItem? itemBefore)
        {
            int rtnVal = -1;

            if (itemBefore != null)
            {
                int itemIndex = IndexOf(itemBefore);

                int level = itemBefore.Level + 1;

                for (int here = itemIndex + 1; here < Count; here++)
                {
                    C4FlatItem item = base.Items[here];
                    if (item.Level <= level)
                    {
                        rtnVal = here;
                        break;
                    }
                }
            }

            return rtnVal;
        }

    }
}
