using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace LocalEdit2.C4Types
{
    public class C4Workspace
    {
        //public List<C4Item> Model { get; set; } = new List<C4Item>();
        //public ObservableCollection<C4Item> Model { get; set; } = new ObservableCollection<C4Item>();

        // Option 3:
        //      Don't include FlatModel in the JSON file
        //      Update FlatModel when this value is set
        //      Return updated Model from FlatModel in the getter
        //      Updates should be through FlatModel
        //      This allows existing data retention to work without modification
        //      This allows existing publishing to work without modification
        //      UI WILL need to be changed
        ObservableCollection<C4Item> model = new ObservableCollection<C4Item>();
        public ObservableCollection<C4Item> Model
        {
            get { return model; }
            set { 
                model = value;
                FlatModel.ConvertFromTree(model);
            }
        }

        public List<C4Relationship> Relationships { get; set; } = new List<C4Relationship>();


        // Option 1:
        //      Don't include this in the JSON file
        //      Capture changes here, and propagate them to the Model
        //      Parent enumerates children
        // Option 2:
        //      Only use this in the JSON file
        //      Tree version (Model) gets derived as required
        //      Child points to parent
        //      Collection will need a method to clean up any sort issues
        [JsonIgnore]
        public C4FlatItemCollection FlatModel { get; set; } = new C4FlatItemCollection();
    }
}
