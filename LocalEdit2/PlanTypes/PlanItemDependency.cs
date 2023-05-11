namespace LocalEdit2.PlanTypes
{
    public class PlanItemDependency
    {
        // WorkItem | StartDate
        public string DependencyType { get; set; } = "";
        public string ID { get; set; } = "";
        public string StartDate { get; set; } = "";

        public override string ToString()
        {
            return (DependencyType == "OTHER") ? ID : StartDate;
        }
    }
}
