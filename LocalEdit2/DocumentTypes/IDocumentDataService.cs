using Microsoft.AspNetCore.Components.Authorization;

namespace LocalEdit2.DocumentTypes
{
    public interface IDocumentDataService
    {
        Task<IEnumerable<Document>> GetAllDocuments();
        Task<Document> GetDocument(string id);
        Task<Document> DeleteDocument(string id);
        Task<Document> UpdateDocument(Document document);
        Task<Document> CreateDocument(Document document);
    }
}

