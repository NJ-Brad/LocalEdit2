using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit2.Modals;
using LocalEdit2.FlowTypes;
using System.Text.Json;
//using StardustDL.RazorComponents.Markdown;
using Octokit;

namespace LocalEdit2.Pages
{
    public partial class FlowEditor : ComponentBase
    {
        public string diagramOneText { get; set; } = string.Empty;
        public string diagramTwoText { get; set; } = string.Empty;

        public Validations? validations { get; set; }

        protected override Task OnInitializedAsync()
        {
//            Document = new()
//            {
//                items = new List<FlowItem>(new[]
//            {
////            C4TestData.InternalPerson,
//            new FlowItem{id = "Q1", title="Screen One"},
//            new FlowItem{id = "Q2", title="Screen Two"},
//            new FlowItem{id = "Q3", title="Screen Three"},
//            new FlowItem{id = "Q4", title="Screen Four"}
//        })
//            };

            return base.OnInitializedAsync();
        }

        // setting to null allows the toolbar buttons to be disabled properly
        FlowItem? SelectedItemRow
        {
            get => selectedItemRow;
            set
            {
                selectedItemRow = value;

                upAllowed = false;
                downAllowed = false;


                if (selectedItemRow != null)
                {
                    int? numItems = Document?.items.Count;

                    if (numItems > 1) 
                    {
                        int rowPosition = GetPosition(selectedItemRow.id ?? "");
                        if (rowPosition != -1)  // there is a selection
                        {
                            if(rowPosition > 0)
                            {
                                upAllowed = true;
                            }
                            if(rowPosition < numItems - 1)
                            {
                                downAllowed = true;
                            }
                        }
                    }
                }
            }
        }

        private int GetPosition(string itemId)
        {
            int rtnVal = -1;    // not found

            for(int pos = 0; pos < Document?.items.Count; pos ++)
            {
                if (Document?.items[pos].id == itemId)
                {
                    rtnVal = pos;
                }
            }

            return rtnVal;
        }

        bool upAllowed { get; set; } = false;
        bool downAllowed { get; set; } = false;

        //FlowRelationship? SelectedRelationshipRow { get; set; }

        FlowDocument? Document { get; set; } = new FlowDocument();

        string MarkdownText { get; set; } = string.Empty;

        bool adding = false;

        string selectedTab = "general";

        private async Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                //GenerateMarkdown();

                await UpdatePreview();
            }

            if (selectedTab == "preview2")
            {
                //GenerateMarkdown();

                await UpdatePreview2();
            }

            //return Task.CompletedTask;
        }

        private Task UpdatePreview()
        {
            diagramOneText = Document == null ? "" : FlowPublisher.Publish(Document);
            return Task.CompletedTask;
        }

        private Task UpdatePreview2()
        {
            diagramTwoText = Document == null ? "" : SequencePublisher.Publish(Document);
            return Task.CompletedTask;
        }

        private Task NewFlow()
        {
            if (FileManagementModalRef != null)
                FileManagementModalRef.Name = "New_Flow.json";

            Document = new()
            {
                items = new List<FlowItem>(new[]
            {
    //            C4TestData.InternalPerson,
            new FlowItem{id = "Q1", title="Screen One"},
            new FlowItem{id = "Q2", title="Screen Two"},
            new FlowItem{id = "Q3", title="Screen Three"},
            new FlowItem{id = "Q4", title="Screen Four"}
        })
            };

            return Task.CompletedTask;
        }

        private FlowItemModal? FlowItemModalRef;
        private FlowItem? selectedItemRow = null;

        private Task ShowItemModal()
        {
            if (SelectedItemRow == null)
            {
                return Task.CompletedTask;
            }
            //FlowItemModalRef.Item = selectedItemRow;

            FlowItemModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task AddNewItem()
        {
            FlowItem newItem = new()
            {
                //newItem.ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
                title = "New Screen"
            };

            SelectedItemRow = newItem;
            Document?.items.Add(newItem);
            adding = true;

            InvokeAsync(() => StateHasChanged());

            return ShowItemModal();
        }

        private Task OnFlowItemModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (FlowItemModalRef?.Result == ModalResult.Cancel)
                {
                    if (SelectedItemRow != null)
                        Document?.items.Remove(SelectedItemRow);
                    SelectedItemRow = null;
                }
            }
            adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task DeleteItem()
        {
            if (SelectedItemRow != null)
            {
                Document?.items.Remove(SelectedItemRow);
                SelectedItemRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ItemUp()
        {
            if (SelectedItemRow != null)
            {
                int position = GetPosition(SelectedItemRow.id ?? "");

                if (position != -1)
                {
                    Document?.items.Remove(SelectedItemRow);
                    Document?.items.Insert(position - 1, SelectedItemRow);
                    // enable buttons appropriately
                    SelectedItemRow = SelectedItemRow;
                }
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ItemDown()
        {
            if (SelectedItemRow != null)
            {
                int position = GetPosition(SelectedItemRow.id ?? "");

                if(position != -1)
                {
                    Document?.items.Remove(SelectedItemRow);
                    Document?.items.Insert(position +1, SelectedItemRow);
                    // enable buttons appropriately
                    SelectedItemRow = SelectedItemRow;
                }
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        FileManagementModal? FileManagementModalRef { get; set; }
        GitManagementModal? GitManagementModalRef { get; set; }

        private Task LoadFile()
        {
            FileManagementModalRef?.LoadFile();

            return Task.CompletedTask;
        }

        private Task LoadFromGitHub()
        {
            // in the future I will want to prompt for credential information
            GitManagementModalRef?.Ask();

            return Task.CompletedTask;
        }


        private Task SaveFile()
        {
            string fileText = JsonSerializer.Serialize(Document, new JsonSerializerOptions { WriteIndented = true }); ;

            FileManagementModalRef?.SaveFile(fileText);

            return Task.CompletedTask;
        }

        private Task ExportFile()
        {
            {
                GenerateMarkdown();

                if (FileManagementModalRef != null)
                {
                    FileManagementModalRef.Name = "Flow.md";
                    FileManagementModalRef.SaveFile(MarkdownText);
                }
            }

            return Task.CompletedTask;
        }

        private Task ExportHtml()
        {
            //            if (Validate().Result)
            {
                string htmlText = GenerateHtml().Result;

                if (FileManagementModalRef != null)
                {
                    FileManagementModalRef.Name = "Flow.html";
                    FileManagementModalRef.SaveFile(htmlText);
                }
            }
            return Task.CompletedTask;
        }

        private Task OnFileManagementModalClosed()
        {
            if (FileManagementModalRef?.Result == ModalResult.OK)
            {
                if (string.IsNullOrEmpty(FileManagementModalRef.FileText))
                    Document = null;
                else
                    Document = JsonSerializer.Deserialize(FileManagementModalRef.FileText, typeof(FlowDocument)) as FlowDocument;
                InvokeAsync(() => StateHasChanged());
            }

            return Task.CompletedTask;
        }

        private async Task OnGitManagementModalClosed()
        {
            if (GitManagementModalRef?.Result == ModalResult.OK)
            {
                try
                {
                    var gitHubClient = new GitHubClient(new ProductHeaderValue("LocalEdit"));

                    if (GitManagementModalRef.Token != "")
                    {
                        gitHubClient.Credentials = new Credentials(GitManagementModalRef.Token);
                    }
                    else if (GitManagementModalRef.UserID != "")
                    {
                        gitHubClient.Credentials = new Credentials(GitManagementModalRef.UserID, GitManagementModalRef.Password);
                    }
                    //var user = await gitHubClient.User.Get("nj-brad");
                    //Console.WriteLine($"Woah! Brad has {user.PublicRepos} public repositories.");

                    //Repository repo = await gitHubClient.Repository.Get("nj-brad", "localedit");

                    //var docs = await gitHubClient.Repository
                    //                    .Content
                    //                    .GetAllContents("nj-brad", "localedit", "LocalEdit/SampleFiles/sample.flow");

                    var fileContent = await gitHubClient.Repository
                                .Content
                                .GetRawContent("nj-brad", "localedit", "LocalEdit/SampleFiles/sample.flow");

                    var fileText = System.Text.Encoding.UTF8.GetString(fileContent, 0, fileContent.Length);

                    Document = JsonSerializer.Deserialize(fileText, typeof(FlowDocument)) as FlowDocument;
                }
                catch
                {
                    Document = null;
                }

                //InvokeAsync(() => StateHasChanged());
            }
            //return Task.CompletedTask;
            return;
        }


        private Task GenerateMarkdown()
        {
            if (Document != null)
                MarkdownText = MarkdownGenerator.WrapMermaid(FlowPublisher.Publish(Document));

            //            markdownRef.Value = MarkdownText;
            return Task.CompletedTask;
        }

        private Task<string> GenerateHtml()
        {
            string htmlText = string.Empty;
            if (Document != null)
                htmlText = HtmlGenerator.WrapMermaid(FlowPublisher.Publish(Document));
            return Task.FromResult(htmlText);
        }

    }
}
