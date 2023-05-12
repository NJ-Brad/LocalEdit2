namespace LocalEdit2.FlowTypes
{
    public class FlowDocument
    {
        public FlowDocument()
        {
            headerConfig = new();
        }

        public bool hasFooter { get; set; }
        public bool hasHeader { get; set; }
        public FlowHeaderConfig headerConfig { get; set; } = new();
        public List<FlowItem> items { get; set; }= new List<FlowItem>();
    }
}
