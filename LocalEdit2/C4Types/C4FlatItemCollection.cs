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

            ProcessItems(tree, "", 0);

            System.Console.WriteLine(Count);
        }

        private void ProcessItems(ObservableCollection<C4Item> tree, string parentAlias, int level)
        {
            foreach (var item in tree)
            {
                Add(new C4FlatItem(item, parentAlias, level));
                ProcessItems(item.Children, item.Alias, level + 1);
            }
        }

        public ObservableCollection<C4Item> ConvertToTree()
        {
            ObservableCollection<C4Item> rtnVal = new ObservableCollection<C4Item>();

            return rtnVal;
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
