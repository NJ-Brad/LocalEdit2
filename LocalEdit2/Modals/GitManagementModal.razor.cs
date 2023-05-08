using Blazor.DownloadFileFast.Interfaces;
using Blazorise;
using Microsoft.AspNetCore.Components;
using System.Text;

namespace LocalEdit.Modals
{
    public partial class GitManagementModal : LE_ModalBase
    {
        public string? UserID { get; set; } = "";
        public string? Password { get; set; } = "";
        public string? Token { get; set; } = "";

        public Task Ask()
        {
            modalVisible = true;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        async Task LogIn()
        {
            Result = ModalResult.OK;
            if (modalRef != null)
                await modalRef.Hide();

            await Closed.InvokeAsync();
        }
    }
}
