using Blazor.DownloadFileFast.Interfaces;
using Blazorise;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace LocalEdit.Modals
{
    public partial class FileManagementModal : LE_ModalBase
    {

        [Inject]
        public IBlazorDownloadFileService? BlazorDownloadFileService { get; set; }
        public string? FileText { get; set; } = "";
        public string? Name { get; set; } = "";

        bool loadFileMode = false;

        //protected override async Task OnInitializedAsync()
        //{
        //            await localStorage.SetItemAsync("name", "John Smith");
        //var name = await localStorage.GetItemAsync<string>("name");
        //var upl = await localStorage.GetItemAsync<string>("uploaded");

        //var exists = await localStorage.GetItemAsync<string>("i18nextLng");

        //var keys = await localStorage.KeysAsync();
        //}

        public Task SaveFile(string fileText)
        {
            loadFileMode = false;
            FileText = fileText;

            modalVisible = true;

            InvokeAsync(() => StateHasChanged());

            Result = ModalResult.Saved;
            modalRef?.Hide();

            Closed.InvokeAsync();
            return Task.CompletedTask;
        }

        public Task LoadFile()
        {
            loadFileMode = true;
            modalVisible = true;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        async Task OnFileUpload(FileUploadEventArgs e)
        {
            try
            {
                using MemoryStream ms = new();
                await e.File.OpenReadStream(long.MaxValue).CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                FileText = await new StreamReader(ms).ReadToEndAsync();
                //fileText = await new StreamReader(e.File.OpenReadStream()).ReadToEndAsync();
                Name = e.File.Name;
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

                if (string.IsNullOrEmpty(Name))
                {
                    Name = "example.txt";
                }

                if (BlazorDownloadFileService != null)
                    await BlazorDownloadFileService.DownloadFileAsync(Name, bytes);
            }

            if(modalRef != null)
                await modalRef.Hide();

            await Closed.InvokeAsync();
        }

    }
}
