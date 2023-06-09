﻿using LocalEdit2.C4Types;
using LocalEdit2.Modals;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace LocalEdit2.Pages
{
    public partial class Index
    {
        public string DocumentText { get; set; } = "[Empty]";
        public string DocumentId { get; set; } = "[Empty]";
        public string DocumentName { get; set; } = "[Empty]";

        [CascadingParameter] 
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private async Task LoadFile()
        {
            // https://stackoverflow.com/questions/60264657/get-current-user-in-a-blazor-component
            //var authState = await authenticationStateTask;
            //var user = authState.User;

            //if (user.Identity.IsAuthenticated)
            //{
            //    Console.WriteLine($"{user.Identity.Name} is authenticated.");
            //}

            DocumentManagementModalRef?.LoadFile();

            //return Task.CompletedTask;
        }

        DocumentManagementModal? DocumentManagementModalRef { get; set; }

        private Task OnDocumentManagementModalClosed()
        {
            if ((DocumentManagementModalRef?.Result == ModalResult.OK) || (DocumentManagementModalRef?.Result == ModalResult.Saved))
            {
                if (!string.IsNullOrEmpty(DocumentManagementModalRef.Document.Content))
                {
                    //Document = JsonSerializer.Deserialize(FileManagementModalRef.FileText, typeof(C4Workspace)) as C4Workspace;
                    DocumentText = DocumentManagementModalRef.Document.Content;
                    DocumentName = DocumentManagementModalRef.Document.DocumentTitle;
                    DocumentId = DocumentManagementModalRef.Document.Id;
                }
                else
                {
                    //Document = null;
                    DocumentText = "[Empty]";
                    DocumentName = string.Empty;
                    DocumentId = string.Empty;
                }
                InvokeAsync(() => StateHasChanged());
            }
            //if (adding)
            //{
            //    // remove the new item, if add was cancelled
            //    if (flowRelationshipModalRef.Result == ModalResult.Cancel)
            //    {
            //        FlowRelationships.Remove(selectedRelationshipRow);
            //    }
            //}
            //adding = false;

            return Task.CompletedTask;
        }

        private Task SaveFile()
        {
            //string fileText = JsonSerializer.Serialize(Document, new JsonSerializerOptions { WriteIndented = true }); ;
            string fileText = DocumentText ;

            DocumentManagementModalRef?.SaveFile(fileText);
            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }
    }
}
