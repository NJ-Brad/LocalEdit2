using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit2.C4Types;
using LocalEdit2.Modals;
using LocalEdit2.FlowTypes;
using Blazorise.Components;
using System.Text.Json;
//using StardustDL.RazorComponents.Markdown;
using LocalEdit2.Shared;
using LocalEdit2.PlanTypes;
using LocalEdit2.FlowTypes;
using Octokit;
using System.Linq.Expressions;

namespace LocalEdit2.Pages
{
    public partial class FlowEditor : ComponentBase
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

        //void Edit()
        //{

        //}

        //Mermaid? MermaidOne { get; set; }
        public string diagramOneText { get; set; } = string.Empty;
        public string diagramTwoText { get; set; } = string.Empty;

        public Validations? validations { get; set; }

        protected override Task OnInitializedAsync()
        {
            Document = new()
            {
                items = new List<FlowItem>(new[]
            {
//            C4TestData.InternalPerson,
            new FlowItem{id = "Q1", title="Question One"},
            new FlowItem{id = "Q2", title="Question Two"},
            new FlowItem{id = "Q3", title="Question Three"},
            new FlowItem{id = "Q4", title="Question Four"}
        })
            };

            //    this.Document.Relationships = new List<FlowRelationship>(new[]
            //    {
            //    new FlowRelationship{ From="Question One", To ="Question Two", title= "Step One"},
            //    new FlowRelationship{ From="Question One", To ="Question Three", title="Alt QuestionFlow"},
            //    new FlowRelationship{ From="Question Three", To ="Question Four", title="Step One"},
            //    new FlowRelationship{ From="Question Two", To ="Question Four", title="Weird QuestionFlow"},
            //    new FlowRelationship{ From="Question Four", To ="Question One", title="Vicious Cycle"}
            //});

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
                        int rowPosition = GetPosition(selectedItemRow.id);
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

        private async Task UpdatePreview()
        {
            diagramOneText = FlowPublisher.Publish(Document);

            //if ((Document != null) && (MermaidOne != null))
            //{
            //    // convert to "normal" flow
            //    FlowDocument fd = LocalEdit2.FlowTypes.LpeConverter.ToFlowDocument(Document);

            //    await MermaidOne.DisplayDiagram(FlowPublisher.Publish(Document));
            //}

    //        diagramOneText = @"graph TD
    //Q1[""Question One""]
    //Q2[""Question Two""]
    //Q3[""Question Three""]
    //Q4[""Question Four""]
    //Q1--""New Relationship""-->Q2
    //";

    //        diagramOneText = @"graph TD
    //Q1[""Question One""]
    //Q2[""Question Two""]
    //Q3[""Question Three""]
    //Q4[""Question Four""]";

        }

        private async Task UpdatePreview2()
        {
            diagramTwoText = SequencePublisher.Publish(Document);
        }

            private Task NewQuestionFlow()
        {
            if (FileManagementModalRef != null)
                FileManagementModalRef.Name = "New_QuestionFlow.json";

            Document = new()
            {
                items = new List<FlowItem>(new[]
            {
    //            C4TestData.InternalPerson,
            new FlowItem{id = "Q1", title="Question One"},
            new FlowItem{id = "Q2", title="Question Two"},
            new FlowItem{id = "Q3", title="Question Three"},
            new FlowItem{id = "Q4", title="Question Four"}
        })
            };

            //this.Document.Relationships = new List<FlowRelationship>(new[]
            //{
            //new FlowRelationship{ From="Question One", To ="Question Two", title= "Step One"},
            //new FlowRelationship{ From="Question One", To ="Question Three", title="Alt QuestionFlow"},
            //new FlowRelationship{ From="Question Three", To ="Question Four", title="Step One"},
            //new FlowRelationship{ From="Question Two", To ="Question Four", title="Weird QuestionFlow"},
            //new FlowRelationship{ From="Question Four", To ="Question One", title="Vicious Cycle"}
            //});

            //ResetValidation();

            return Task.CompletedTask;
        }

        //        List<FlowItem> FlowItems = new List<FlowItem>(new[]
        //        {
        ////            C4TestData.InternalPerson,
        //            new FlowItem{ID = "Q1", ItemType=FlowItemType.Question, title="Question One"},
        //            new FlowItem{ID = "Q2", ItemType=FlowItemType.Question, title="Question Two"},
        //            new FlowItem{ID = "Q3", ItemType=FlowItemType.Question, title="Question Three"},
        //            new FlowItem{ID = "Q4", ItemType=FlowItemType.Question, title="Question Four"}
        //        });

        //        List<FlowRelationship> FlowRelationships = new List<FlowRelationship>(new[]
        //        {
        //            new FlowRelationship{ From="Q1", To ="Q2", title= "Step One"},
        //            new FlowRelationship{ From="Q1", To ="Q3", title="Alt QuestionFlow"},
        //            new FlowRelationship{ From="Q3", To ="Q4", title="Step One"},
        //            new FlowRelationship{ From="Q2", To ="Q4", title="Weird QuestionFlow"},
        //            new FlowRelationship{ From="Q4", To ="Q1", title="Vicious Cycle"}
        //        });

        private FlowItemModal? FlowItemModalRef;
        private FlowItem? selectedItemRow = null;

        //private FlowRelationshipModal? FlowRelationshipModalRef;

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
                title = "New Question"
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
                int position = GetPosition(SelectedItemRow.id);

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
                int position = GetPosition(SelectedItemRow.id);

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


        //private Task ShowRelationshipModal()
        //{
        //    //if (selectedRelationshipRow == null)
        //    //{
        //    //    return Task.CompletedTask;
        //    //}
        //    //FlowRelationshipModalRef.item = selectedRelationshipRow;

        //    //FlowRelationshipModalRef?.ShowModal();

        //    ////InvokeAsync(() => StateHasChanged());

        //    return Task.CompletedTask;
        //}

        //private Task AddNewRelationship()
        //{
        //    //FlowRelationship newRelationship = new FlowRelationship();
        //    //newRelationship.title = "New Relationship";

        //    //selectedRelationshipRow = newRelationship;
        //    //Document.Relationships.Add(newRelationship);
        //    //adding = true;

        //    return ShowRelationshipModal();
        //}

        //private string DecodeQuestionFlowId(string id)
        //{
        //    string rtnVal = id;

        //    foreach (FlowItem fi in Document.Items)
        //    {
        //        if(fi.ID == id)
        //        {
        //            rtnVal = fi.title;
        //            break;
        //        }
        //    }

        //    return rtnVal;
        //}

        //private Task DeleteRelationship()
        //{
        //    //if (selectedRelationshipRow != null)
        //    //{
        //    //    Document.Relationships.Remove(selectedRelationshipRow);
        //    //    selectedRelationshipRow = null;
        //    //}

        //    //InvokeAsync(() => StateHasChanged());

        //    return Task.CompletedTask;
        //}

        //private Task OnFlowRelationshipModalClosed()
        //{
        //    //if (adding)
        //    //{
        //    //    // remove the new item, if add was cancelled
        //    //    if (FlowRelationshipModalRef.Result == ModalResult.Cancel)
        //    //    {
        //    //        Document.Relationships.Remove(selectedRelationshipRow);
        //    //        selectedRelationshipRow = null;
        //    //    }
        //    //}
        //    //adding = false;

        //    //InvokeAsync(() => StateHasChanged());

        //    return Task.CompletedTask;
        //}

        FileManagementModal? FileManagementModalRef { get; set; }
        GitManagementModal? GitManagementModalRef { get; set; }


        //bool isLpeFile = false;

        private Task LoadFile()
        {
            //isLpeFile = false;

            FileManagementModalRef?.LoadFile();

            return Task.CompletedTask;
        }

        private Task LoadFromGitHub()
        {
            // in the future I will want to prompt for credential information
            GitManagementModalRef?.Ask();

            return Task.CompletedTask;
        }

        private Task LoadLpeFile()
        {
            //isLpeFile = true;

            FileManagementModalRef?.LoadFile();

            return Task.CompletedTask;
        }


        private Task SaveFile()
        {
            string fileText = JsonSerializer.Serialize(Document, new JsonSerializerOptions { WriteIndented = true }); ;
            //if (selectedItemRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //FlowItemModalRef.item = selectedItemRow;

            FileManagementModalRef?.SaveFile(fileText);
            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task ExportFile()
        {
            //          if (Validate().Result)
            {
                GenerateMarkdown();

                if (FileManagementModalRef != null)
                {
                    FileManagementModalRef.Name = "QuestionFlow.md";
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
                    FileManagementModalRef.Name = "QuestionFlow.html";
                    FileManagementModalRef.SaveFile(htmlText);
                }
            }
            return Task.CompletedTask;
        }


        //        MarkdownRenderer markdownRef = null;

        //void OnClickNode(string nodeId)
        //{
        //    // TODO: do something with nodeId
        //}

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
            //if (adding)
            //{
            //    // remove the new item, if add was cancelled
            //    if (FlowRelationshipModalRef.Result == ModalResult.Cancel)
            //    {
            //        FlowRelationships.Remove(selectedRelationshipRow);
            //    }
            //}
            //adding = false;

            return Task.CompletedTask;
        }

        private async Task OnGitManagementModalClosed()
        {
            if (GitManagementModalRef?.Result == ModalResult.OK)
            {
                //    if (string.IsNullOrEmpty(FileManagementModalRef.FileText))
                //        Document = null;
                //    else
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

                InvokeAsync(() => StateHasChanged());
            }
            //return Task.CompletedTask;
            return;
        }


        //        private string GenerateMermaidText(QuestionFlowDocument document)
        private Task GenerateMarkdown()
        {
            //mermaidText = QuestionFlowPublisher.Publish(Document);
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
