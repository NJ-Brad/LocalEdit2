using System.Collections.ObjectModel;

namespace LocalEdit2.C4Types
{
    public class C4FlatItem : C4Item
    {
        public C4FlatItem()
        {

        }
        public C4FlatItem(C4Item baseItem, string parentAlias, int level)
        {
            ItemType = baseItem.ItemType;
            Text = baseItem.Text;
            IsDatabase = baseItem.IsDatabase;
            IsExternal = baseItem.IsExternal;
            Alias = baseItem.Alias;
            Description = baseItem.Description;
            Technology = baseItem.Technology;

            Level = level;
            ParentAlias = parentAlias;
        }

        public int Level { get; set; } = 0;
        public string ParentAlias { get; set; } = "";
    }
}
