using Blazor.DownloadFileFast.Interfaces;
using Blazorise;
using LocalEdit2.C4Types;
using LocalEdit2.DocumentTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
//using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

namespace LocalEdit2.Modals
{
    public partial class DocumentManagementModal : LE_ModalBase
    {

        [Inject]
        public IBlazorDownloadFileService? BlazorDownloadFileService { get; set; }
        [Inject]
        public IDocumentIndexDataService? DocumentIndexDataService { get; set; }
        [Inject]
        public IDocumentDataService? DocumentDataService { get; set; }

        [Parameter]
        public Document Document { get; set; } = new();

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }


        private string originalFileName = string.Empty;

        public string? FileText { get 
            { 
                return Document.Content; 
            }
            set 
            {
                if (Document == null)
                    Document = new();
                Document.Content = value;
            } 
        }

        bool loadFileMode = false;

        public Task SaveFile(string fileText)
        {
            loadFileMode = false;
            FileText = fileText;

            modalVisible = true;

            InvokeAsync(() => StateHasChanged());

            //Result = ModalResult.Saved;
            //modalRef?.Hide();

            //Closed.InvokeAsync();
            return Task.CompletedTask;
        }

        public async Task LoadFile()
        {
            loadFileMode = true;
            modalVisible = true;

            //InvokeAsync(() => StateHasChanged());
            //StateHasChanged();

            //return Task.CompletedTask;
        }

        async Task OnFileUpload(FileUploadEventArgs e)
        {
            try
            {
                using MemoryStream ms = new();
                await e.File.OpenReadStream(long.MaxValue).CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);

                if (Document == null)
                    Document = new();

                Document.Content = await new StreamReader(ms).ReadToEndAsync();
                Document.DocumentTitle = e.File.Name;
                Document.Id = string.Empty;

                Result = ModalResult.OK;
                if (modalRef != null)
                    await modalRef.Hide();

                await Closed.InvokeAsync();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                //this.StateHasChanged();
            }
        }

        //private async void LoadFiles(InputFileChangeEventArgs e)
        //{
        //    // https://learn.microsoft.com/en-us/aspnet/core/blazor/file-uploads?view=aspnetcore-6.0&pivots=webassembly

        //    var reader = await new StreamReader(e.File.OpenReadStream()).ReadToEndAsync();
        //    //await localStorage.SetItemAsync("uploaded", reader);
        //    fileText = reader;

        //    // Note that the following line is necessary because otherwise
        //    // Blazor would not recognize the state change and refresh the UI
        //    //InvokeAsync(() =>
        //    //{
        //    StateHasChanged();
        //    //});
        //}

        private async void DownloadFile()
        {
            // https://stackoverflow.com/questions/16072709/converting-string-to-byte-array-in-c-sharp
            if (FileText != null)
            {
                byte[] bytes = Encoding.ASCII.GetBytes(FileText);

                Result = ModalResult.Saved;

                if (Document == null)
                    Document = new();

                if (string.IsNullOrEmpty(Document.DocumentTitle))
                {
                    Document.DocumentTitle = "example.txt";
                }

                if (BlazorDownloadFileService != null)
                    await BlazorDownloadFileService.DownloadFileAsync(Document.DocumentTitle, bytes);
            }

            if(modalRef != null)
                await modalRef.Hide();

            await Closed.InvokeAsync();
        }

        string selectedTab = "cloud";

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            return Task.CompletedTask;
        }

        private List<DocumentTypeNamePair> DocumentTypes { get; set; } = new();
        private List<Document>? Documents { get; set; } = null;
        private Document? SelectedDocument { get; set; } = null;

        [Parameter]
        public string DocumentType { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            //DocumentTypes.Clear();

            //DocumentTypes.Add(new DocumentTypeNamePair(DocumentTypeEnum.C4, "C4 Document"));
            //DocumentTypes.Add(new DocumentTypeNamePair(DocumentTypeEnum.Flow, "Flow Document"));
            //DocumentTypes.Add(new DocumentTypeNamePair(DocumentTypeEnum.Plan, "Plan Document"));

            if (DocumentIndexDataService != null)
            {
                var v = await DocumentIndexDataService.GetAllDocuments(DocumentType);
                Documents = v.ToList();
            }
            else
            {
                Documents = null;
            }

            originalFileName = Document.DocumentTitle;

            await base.OnParametersSetAsync();
        }

        private async Task LoadCloudDocument()
        {

            //// https://stackoverflow.com/questions/60264657/get-current-user-in-a-blazor-component
            //var authState = await authenticationStateTask;
            //var user = authState.User;

            //if (user.Identity.IsAuthenticated)
            //{
            //    Console.WriteLine($"{user.Identity.Name} is authenticated.");
            //}

            if (DocumentDataService != null)
            {
                Document doc = await DocumentDataService.GetDocument(SelectedDocument.Id);

                Document.Content = doc.Content;
                Document.DocumentTitle = doc.DocumentTitle;
                Document.Id = doc.Id;

                Result = ModalResult.OK;
            }

            if (modalRef != null)
            {
                await modalRef.Hide();
            }
            await Closed.InvokeAsync();
        }

        private async Task SaveCloudDocument()
        {
            // some day, I may check for duplicate file names.  For now YOLO

            // a name change creates a new document ID
            if(originalFileName != Document.DocumentTitle)
            {
                Document.Id = string.Empty;
            }

            //if(string.IsNullOrEmpty(Document.Id))
            //    Document.Id = Guid.NewGuid().ToString().Replace("-", "");

            Document.DocumentType = DocumentType;

            if (DocumentDataService != null)
            {
                if (string.IsNullOrEmpty(Document.Id))
                    await DocumentDataService.CreateDocument(Document);
                else
                    await DocumentDataService.UpdateDocument(Document);
                }

                if (modalRef != null)
            {
                await modalRef.Hide();
            }
            await Closed.InvokeAsync();

            Result = ModalResult.Unknown;
        }

    }
}
