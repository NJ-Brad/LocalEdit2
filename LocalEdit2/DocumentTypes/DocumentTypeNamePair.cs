namespace LocalEdit2.DocumentTypes
{
    public class DocumentTypeNamePair
    {
        public DocumentTypeNamePair(DocumentTypeEnum value, string description)
        {
            Value = value;
            Description = description;
        }

        public DocumentTypeEnum Value { get; set; }
        public string Description { get; set; }
    }
}
