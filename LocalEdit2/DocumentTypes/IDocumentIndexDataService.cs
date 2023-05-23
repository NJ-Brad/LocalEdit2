namespace LocalEdit2.DocumentTypes
{
    public interface IDocumentIndexDataService
    {
        Task<IEnumerable<Document>> GetAllDocuments(string? DocumentType);
    }
}
