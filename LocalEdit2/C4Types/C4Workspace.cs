using System.Collections.ObjectModel;

namespace LocalEdit2.C4Types
{
    public class C4Workspace
    {
        //public List<C4Item> Model { get; set; } = new List<C4Item>();
        public ObservableCollection<C4Item> Model { get; set; } = new ObservableCollection<C4Item>();
        public List<C4Relationship> Relationships { get; set; } = new List<C4Relationship>();

    }
}
