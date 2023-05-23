using Microsoft.AspNetCore.Components;
using Blazorise;
//using LocalEdit.C4Types;
using LocalEdit2.Modals;
//using LocalEdit.PlanTypes;
using Blazorise.Components;
using System.Text.Json;

namespace LocalEdit2.Pages
{
    public partial class MarkdownEditor : ComponentBase
    {
        // Add
        // Click Add
        // Create new record
        // set modalObject to new record
        // Open editor
        // When editor closes
        // If cancelled - exit
        // else add record to tree and model

        // Update
        // click edit
        // create new record
        // copy selected record to new record
        // set modalObject to new record
        // open editor
        // When editor closes
        // If cancelled - exit
        // else update model and tree

        string markdownValue = "# EasyMDE \n Go ahead, play around with the editor! Be sure to check out **bold**, *italic*, [links](https://google.com) and all the other features. You can type the Markdown syntax, use the toolbar, or use shortcuts like `ctrl-b` or `cmd-b`.";

        string? markdownHtml { get; set; }

        protected override void OnInitialized()
        {
//            markdownHtml = Markdig.Markdown.ToHtml(markdownValue ?? string.Empty);

            base.OnInitialized();
        }

        Task OnMarkdownValueChanged(string value)
        {
            markdownValue = value;

//            markdownHtml = Markdig.Markdown.ToHtml(markdownValue ?? string.Empty);

            return Task.CompletedTask;
        }

        DocumentManagementModal? DocumentManagementModalRef { get; set; }

        private Task NewMdDocument()
        {
            if(DocumentManagementModalRef != null)
                DocumentManagementModalRef.Document.DocumentTitle = "New_Document.md";

            markdownValue = "";

            return Task.CompletedTask;
        }

        private Task LoadFile()
        {
            //if (selectedItemRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //flowItemModalRef.item = selectedItemRow;

            DocumentManagementModalRef?.LoadFile();
            //DocumentManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task SaveFile()
        {
            string fileText = markdownValue;
            //if (selectedItemRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //flowItemModalRef.item = selectedItemRow;

            if (DocumentManagementModalRef != null)
                DocumentManagementModalRef.SaveFile(fileText);

            //DocumentManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnDocumentManagementModalClosed()
        {
            if (DocumentManagementModalRef?.Result == ModalResult.OK)
            {
                if ((DocumentManagementModalRef != null) && (DocumentManagementModalRef.FileText != null))
                    markdownValue = DocumentManagementModalRef.FileText;
                InvokeAsync(() => StateHasChanged());
            }

            return Task.CompletedTask;
        }
    }
}
