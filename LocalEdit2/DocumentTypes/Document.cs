namespace LocalEdit2.DocumentTypes
{
    public class Document
    {
        //public string? Id { get; set; } = Guid.NewGuid().ToString().Replace("-", "");
        public string? Id { get; set; }
        public string? DocumentType { get; set; } = "";
        public string? DocumentTitle { get; set; } = "";
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? Modified { get; set; } = DateTime.Now;
        public string? Content { get; set; } = "";
    }
}
