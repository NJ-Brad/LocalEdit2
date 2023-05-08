namespace LocalEdit2.C4Types
{
    public class C4TypeNamePair
    {
        public C4TypeNamePair(C4TypeEnum value, string description)
        {
            Value = value;
            Description = description;
        }

        public C4TypeEnum Value { get; set; }
        public string Description { get; set; }
    }
}
