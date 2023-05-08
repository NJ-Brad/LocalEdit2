using System.Collections.ObjectModel;

namespace LocalEdit2.C4Types
{
    public class C4Item
    {
        private string alias = "";

        public C4TypeEnum ItemType { get; set; } = C4TypeEnum.Unknown;
        //public int Level { get; set; }
        public string Text { get; set; } = "";
        //public string Command { get; set; } = "";
        public bool IsDatabase { get; set; } = false;
        public bool IsExternal { get; set; } = false;
        public string Alias { get 
            {
                if (string.IsNullOrEmpty(alias))
                {
                    FixAlias();
                }
                return alias;
            }
            set => alias = value; }
        public string Description { get; set; } = "";
        public string Technology { get; set; } = string.Empty;
        public ObservableCollection<C4Item> Children { get; set; } = new ObservableCollection<C4Item>();

        private void FixAlias()
        {
            string workString = "";
            // Text is second choice
            if (string.IsNullOrEmpty(Text))
            {
                workString = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
            }
            else
            {
                workString = Text.Replace(' ', '_').ToUpper();
            }

            alias = workString;
        }

    }
}
