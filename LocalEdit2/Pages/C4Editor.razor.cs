using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit2.C4Types;
using LocalEdit2.Modals;
using System.Text.Json;
//using StardustDL.RazorComponents.Markdown;
using LocalEdit2.Shared;
using System.Reflection.Metadata;
using System.Collections.ObjectModel;

namespace LocalEdit2.Pages
{
    public partial class C4Editor : ComponentBase
    {
        // need a state diagram for add / edit

        //** Edit in place **//
        // Add
        // Click Add
        // Create new record
        // Open editor
        // When editor closes
        // If cancelled - exit
        // else add record to tree and model

        // Update
        // click edit
        // open editor
        // When editor closes
        // If cancelled - revert changes from model back to tree - Model = undo for any edits that were made
        // else update model to match tree

        //** Edit copy **//
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

        //void SelChanged(C4Item item)
        //{
        //    this.SelectedNode = item;
        //    parentNode = FindParent(this.SelectedNode, Document.Model);
        //}

        C4Workspace? Document { get; set; } = new C4Workspace();
//        MarkdownRenderer markdownRef;
        C4ItemEditModal? c4ItemModalRef = null;

        Blazorise.TreeView.TreeView<C4Item>? c4Tree { get; set; }

        private Task ShowItemModal()
        {
            if (SelectedNode == null)
            {
                return Task.CompletedTask;
            }

            if (c4ItemModalRef != null)
            {
                //c4ItemModalRef.ParentType = parentNode == null ? C4TypeEnum.Unknown : parentNode.ItemType;
                c4ItemModalRef.ParentType = parentOfNode == null ? C4TypeEnum.Unknown : parentOfNode.ItemType;
                c4ItemModalRef.SelectedNode = SelectedNode;

                c4ItemModalRef?.ShowModal();
            }

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        bool adding;

        // Is this a case where I need to wait to create the new item?
        // For consistency, i should create the item, then remove it on close, if I hit cancel
        // I could create the item, once I know the type...  Bleep.  I know that I am creating an item.
        // I just dow't know the node type
        // If the user cancels on the node type selection, call the cancel method
        // This will allow the main editor to only need to call the single modal

        private Task AddNewItem()
        {
            //C4Item testItem = new();
            //testItem.Text = "New Question";
            //Document.Model.Add(testItem);

            //C4Workspace? holdMe = Document;
            //Document = holdMe;

            //InvokeAsync(() => StateHasChanged());


            //return Task.CompletedTask;


            if (Document == null)
                return Task.CompletedTask;

            selectedNodeAtTimeOfClick = SelectedNode;
            parentOfNode = FindParent(selectedNodeAtTimeOfClick, Document.Model);

            C4Item newItem = new ();
            //newItem.ItemType = FlowItemType.Question;
            //newItem.ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
            //newItem.Label = "New Question";

            if (parentOfNode == null)
            {
                Document.Model.Add(newItem);
            }
            else
            {
                parentOfNode.Children.Add(newItem);
            }
            SelectedNode = newItem;
            adding = true;

            InvokeAsync(() => StateHasChanged());

            return ShowItemModal();
            //return Task.CompletedTask;
        }

        private Task AddNewChildItem()
        {
            selectedNodeAtTimeOfClick = SelectedNode;
            parentOfNode = SelectedNode;

            C4Item newItem = new ();
            //newItem.ItemType = FlowItemType.Question;
            //newItem.ID = Guid.NewGuid().ToString().Replace('-', '_').ToUpper();
            //newItem.Label = "New Question";

            parentNode = SelectedNode;

            SelectedNode?.Children.Add(newItem);
            SelectedNode = newItem;
            adding = true;

            InvokeAsync(() => StateHasChanged());

            return ShowItemModal();
        }

        private Task EditItem()
        {
            selectedNodeAtTimeOfClick = SelectedNode;
            if ((Document != null) && (Document.Model != null))
            {
                parentOfNode = FindParent(selectedNodeAtTimeOfClick, Document.Model);
            }
            else
                parentOfNode = null;

            return ShowItemModal();
        }


        private C4Item? FindParent(C4Item? NodeInQuestion, IEnumerable<C4Item> collection)
        {
            C4Item? parentNode = null;
            if (NodeInQuestion == null)
                return null;

            foreach (C4Item potential in collection)
            {
                // if it doesn't have childrent, it cannot be a parent
                if(potential.Children.Count > 0)
                {
                    foreach (C4Item child in potential.Children)
                    {
                        if (child.Alias == NodeInQuestion.Alias)
                        {
                            parentNode = potential;
                        }
                        else
                        {
                            parentNode = FindParent(NodeInQuestion, child.Children);
                        }

                        if (parentNode != null)
                            break;
                    }
                }
                if (parentNode != null)
                    break;
            }

            return parentNode;
        }

        //IEnumerable<C4Item> C4Items1 = new[]
        //{
        //    C4TestData.InternalPerson,
        //    C4TestData.ExternalPerson,
        //    C4TestData.Boundary,
        //    C4TestData.SystemBoundary,
        //    C4TestData.EnterpriseBoundary,
        //    C4TestData.ContainerBoundary,
        //    C4TestData.Component,
        //    C4TestData.Database,
        //    C4TestData.Container,
        //    C4TestData.Node,
        //    C4TestData.InternalSystem,
        //    C4TestData.ExternalSystem,
        //    C4TestData.InternalDatabaseSystem,
        //    C4TestData.ExternalDatabaseSystem
        //};


        //    IEnumerable<Item> Items = new[]
        //       {
        //    new Item { Text = "Item 1" },
        //    new Item {
        //        Text = "Item 2",
        //        Children = new []
        //{
        //            new Item { Text = "Item 2.1" },
        //            new Item { Text = "Item 2.2", Children = new []
        //    {
        //                new Item { Text = "Item 2.2.1" },
        //                new Item { Text = "Item 2.2.2" },
        //                new Item { Text = "Item 2.2.3" },
        //                new Item { Text = "Item 2.2.4" }
        //            }
        //        },
        //        new Item { Text = "Item 2.3" },
        //        new Item { Text = "Item 2.4" }
        //        }
        //    },
        //    new Item { Text = "Item 3" },
        //};

        private Task NewC4Document()
        {
            Document = new()
            {
                //Model = new List<C4Item>(new[]
                Model = new ObservableCollection<C4Item>(new[]
{
            new C4Item{ItemType=C4TypeEnum.Person, Text="Customer", Description="A customer of the bank, with personal bank accounts", IsExternal=true},
            new C4Item{ItemType=C4TypeEnum.EnterpriseBoundary, Text="Internet Banking",
                Children=new ObservableCollection<C4Item>( new[]{
                    new C4Item{ItemType=C4TypeEnum.Container, Text ="Web Application", Technology="Java, Spring MVC", Description="Delivers the static content and the Internet banking SPA" }
                })
            }
        })
            };
            return Task.CompletedTask;
        }

        private Task LoadFile()
        {
            FileManagementModalRef?.LoadFile();

            return Task.CompletedTask;
        }

        FileManagementModal? FileManagementModalRef { get; set; }

        private Task OnFileManagementModalClosed()
        {
            if (FileManagementModalRef?.Result == ModalResult.OK)
            {
                if((FileManagementModalRef != null) &&(!string.IsNullOrEmpty(FileManagementModalRef.FileText)))
                    Document = JsonSerializer.Deserialize(FileManagementModalRef.FileText, typeof(C4Workspace)) as C4Workspace;
                else
                    Document = null;
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
            string fileText = JsonSerializer.Serialize(Document, new JsonSerializerOptions { WriteIndented = true }); ;
            //if (selectedItemRow == null)
            //{
            //    return Task.CompletedTask;
            //}
            //flowItemModalRef.item = selectedItemRow;

            FileManagementModalRef?.SaveFile(fileText);
            //fileManagementModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        C4Item? selectedNodeAtTimeOfClick = null;
        C4Item? parentOfNode = null;
        C4Item? selectedNode = null;
        C4Item? parentNode = null;
        //C4Item? potentialParentNode = null;
//        C4Item? newItem = null;

        //private Modal? modalRef;
        //private Modal? NewItemModalRef { get; set; } = null;
        //private Modal? C4ItemModalRef;


        //private bool cancelClose;

        //private bool modalVisible;
        //private bool newItemModalVisible;

        //private bool cancelled = false;

//        private Task ShowModal()
//        {
////            modalVisible = true;

//            InvokeAsync(() => StateHasChanged());

//            return Task.CompletedTask;
//        }

        //        private Task ShowNewItemModal()
        //private Task ShowNewItemModal(C4Item? parentNode)
        //{
        //    potentialParentNode = parentNode;
            
        //    //newItemModalVisible = true;

        //    InvokeAsync(() => StateHasChanged());

        //    return Task.CompletedTask;
        //}

        //private Task HideModal()
        //{
        //    modalVisible = false;

        //    return Task.CompletedTask;
        //}

        //private Task CloseModal()
        //{
        //    // possibly add a check for changed and prompt to lose changes

        //    cancelClose = false;
        //    cancelled = true;

        //    return modalRef.Hide();
        //}

        //C4TypeEnum? NewItemType { get; set; }

        //private Task CloseNewItemModal()
        //{
        //    newItemType = null;

        //    if(NewItemModalRef == null)
        //        return Task.CompletedTask;

        //    return NewItemModalRef.Close(CloseReason.EscapeClosing);
        //}

        //private Task CreateItem(C4TypeEnum itemType)
        //{
        //    newItemType = itemType;

        //    newItem = new C4Item() { ItemType = itemType };
        //    //            selectedNode = newItem;

        //    if (NewItemModalRef == null)
        //        return Task.CompletedTask;

        //    return NewItemModalRef.Close(CloseReason.EscapeClosing);
        //}

        //private static bool ShouldShow(C4TypeEnum itemType, C4Item? parentNode)
        //{
        //    bool rtnVal = false;

        //    if (parentNode == null)
        //    {
        //        switch (itemType)
        //        {
        //            case C4TypeEnum.Person:
        //            case C4TypeEnum.System:
        //            case C4TypeEnum.EnterpriseBoundary:
        //                rtnVal = true;
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        switch (parentNode.ItemType)
        //        {
        //            case C4TypeEnum.Person:
        //                // nothing is allowed
        //                break;
        //            case C4TypeEnum.System:
        //                switch (itemType)
        //                {
        //                    case C4TypeEnum.Container:
        //                    case C4TypeEnum.Database:
        //                        rtnVal = true;
        //                        break;
        //                }
        //                break;
        //            case C4TypeEnum.EnterpriseBoundary:
        //                switch (itemType)
        //                {
        //                    case C4TypeEnum.Person:
        //                    case C4TypeEnum.System:
        //                        rtnVal = true;
        //                        break;
        //                }
        //                break;
        //            case C4TypeEnum.Container:
        //                switch (itemType)
        //                {
        //                    case C4TypeEnum.Component:
        //                        rtnVal = true;
        //                        break;
        //                }
        //                break;

        //        }
        //    }

        //    return rtnVal;

        //}

        //private async Task TryCloseModal()
        //{
        //    // add a check for validity

        //    cancelClose = true;

        //    if (await propEditor.IsValid())
        //    {
        //        cancelClose = false;
        //    }

        //    await modalRef.Hide();
        //}

//        private Task OnModelOpened()
//        {
//            // reset, for the next attempt to close
//            cancelClose = false;
////            propEditor.ResetValidation();
//            cancelled = false;

//            return Task.CompletedTask;
//        }

        //private Task OnNewItemModalOpened()
        //{
        //    //// reset, for the next attempt to close
        //    //cancelClose = false;
        //    ////            propEditor.ResetValidation();
        //    //cancelled = false;

        //    return Task.CompletedTask;
        //}

        string MarkdownText { get; set; } = string.Empty;

        Mermaid? MermaidOne { get; set; }
        Mermaid? MermaidTwo { get; set; }
        Mermaid? MermaidThree { get; set; }

        //string diagramOneDefinition = "One";
        //string diagramTwoDefinition = "Two";
        //string diagramThreeDefinition = "Three";

        //void OnClickNode(string nodeId)
        //{
        //    // TODO: do something with nodeId
        //}

        public C4Item? SelectedNode { get => selectedNode; 
            set
            {
                selectedNode = value; 
                if(Document != null)
                    parentNode = FindParent(value, Document.Model);
            }
        }

        //private Task OnModalClosing(ModalClosingEventArgs e)
        //{
        //    // just set Cancel to prevent modal from closing

        //    if (e.CloseReason == CloseReason.EscapeClosing)
        //    {
        //        CloseModal();
        //    }

        //    if (cancelClose || e.CloseReason != CloseReason.UserClosing)
        //    {
        //        e.Cancel = true;
        //    }

        //    return Task.CompletedTask;
        //}

        //private Task OnNewItemModalClosed()
        //{
        //    if(NewItemType != null)
        //    {
        //        ShowModal();
        //    }
        //    return Task.CompletedTask;
        //}

            private Task OnC4ItemModalClosed()
        { 
            if(adding)
            {
                // remove the new item, if add was cancelled
                if (c4ItemModalRef?.Result == ModalResult.Cancel)
                {
                    if (SelectedNode != null)
                    {
                        if (parentNode != null)
                        {
                            parentNode.Children.Remove(SelectedNode);
                        }
                        else
                        {
                            Document?.Model.Remove(SelectedNode);
                        }
                    }
                    SelectedNode = null;
                }
            }
            adding = false;

            if (SelectedNode != null)
            {
                c4Tree.ExpandAll();
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        C4Relationship? SelectedRelationshipRow { get; set; } = new();

        // this will need to go deeper, to find child items
        private string DecodeFlowId(string id)
        {
            string rtnVal = id;

            if (C4RelationshipModalRef != null)
            {
                //foreach (C4Item fi in Document.Model)
                foreach (C4Item fi in C4RelationshipModalRef.AllItems)
                {
                    if (fi.Alias == id)
                    {
                        rtnVal = fi.Text;
                        break;
                    }
                }
            }

            return rtnVal;
        }

        private Task AddNewRelationship()
        {
            C4Relationship newRelationship = new()
            {
                Text = "New Relationship"
            };

            SelectedRelationshipRow = newRelationship;
            Document?.Relationships.Add(newRelationship);
            adding = true;

            return ShowRelationshipModal();
        }

        C4RelationshipModal? C4RelationshipModalRef { get; set; } = null;

        private Task ShowRelationshipModal()
        {
            if (SelectedRelationshipRow == null)
            {
                return Task.CompletedTask;
            }

//            C4RelationshipModalRef.Item = SelectedRelationshipRow;

            C4RelationshipModalRef?.ShowModal();

            //InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnC4RelationshipModalClosed()
        {
            if (adding)
            {
                // remove the new item, if add was cancelled
                if (C4RelationshipModalRef?.Result == ModalResult.Cancel)
                {
                    if (SelectedRelationshipRow != null)
                    {
                        Document?.Relationships.Remove(SelectedRelationshipRow);
                        SelectedRelationshipRow = null;
                    }
                }
            }
            adding = false;

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }


        private Task DeleteRelationship()
        {
            if (SelectedRelationshipRow != null)
            {
                Document?.Relationships.Remove(SelectedRelationshipRow);
                SelectedRelationshipRow = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }


        //private Task OnNewItemModalClosing(ModalClosingEventArgs e)
        //{
        //    // just set Cancel to prevent modal from closing

        //    //if (e.CloseReason == CloseReason.EscapeClosing)
        //    //{
        //    //    CloseModal();
        //    //}

        //    //if (cancelClose || e.CloseReason != CloseReason.UserClosing)
        //    //{
        //    //    e.Cancel = true;
        //    //}

        //    return Task.CompletedTask;
        //}

        private Task DeleteItem()
        {
            if (SelectedNode != null)
            {
                if (parentNode != null)
                {
                    parentNode.Children.Remove(SelectedNode);
                }
                else
                {
                    Document?.Model.Remove(SelectedNode);
                }
                SelectedNode = null;
            }

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        //protected string TestVal { get; set; }

        private async Task GenerateMarkdown()
        {
            //MarkdownText = MarkdownGenerator.WrapMermaid(C4Publisher.Publish(Document));

            //MarkdownText = MarkdownGenerator.WrapMermaid(C4Publisher.Publish(Document, "Context"));
            //testVal = MarkdownText;

            //MarkdownText = MarkdownGenerator.WrapMermaid("Context Diagram", C4PublisherLegacy.Publish(Document, "Context"),
            //    "Container Diagram", C4PublisherLegacy.Publish(Document, "Container"),
            //    "Component Diagram", C4PublisherLegacy.Publish(Document, "Component"));

            MarkdownText = MarkdownGenerator.WrapMermaid("Context Diagram", C4Publisher.Publish(Document, "Context"),
                "Container Diagram", C4Publisher.Publish(Document, "Container"),
                "Component Diagram", C4Publisher.Publish(Document, "Component"));

            //await mermaidOne.DisplayDiagram(C4Publisher.Publish(Document, "Context"));
            //await mermaidTwo.DisplayDiagram(C4Publisher.Publish(Document, "Container"));
            //await mermaidThree.DisplayDiagram(C4Publisher.Publish(Document, "Component"));

            //await mermaidOne.DisplayDiagram(C4PublisherLegacy.Publish(Document, "Context"));
            //await mermaidTwo.DisplayDiagram(C4PublisherLegacy.Publish(Document, "Container"));
            //await mermaidThree.DisplayDiagram(C4PublisherLegacy.Publish(Document, "Component"));

            //diagramOneDefinition = C4PublisherLegacy.Publish(Document, "Context");
            //diagramTwoDefinition = C4PublisherLegacy.Publish(Document, "Container");
            //diagramThreeDefinition = C4PublisherLegacy.Publish(Document, "Component");
            //testVal = diagramOneDefinition;

            //markdownRef.Value = MarkdownText;

            //            markdownRef.Value = @"# Preview not available:  
            //## The version of Mermaid used by this control is out of date";
            //return Task.CompletedTask;

            await InvokeAsync(() => StateHasChanged());

        }

        private Task<string> GenerateHtml()
        {
            //            string htmlText = HtmlGenerator.WrapMermaid(C4Publisher.Publish(Document));

            string htmlText = HtmlGenerator.WrapMermaid("Context Diagram", C4Publisher.Publish(Document, "Context"),
                "Container Diagram", C4Publisher.Publish(Document, "Container"),
                "Component Diagram", C4Publisher.Publish(Document, "Component"));

            return Task.FromResult(htmlText);
        }

        private async Task ExportFile()
        {
            //          if (Validate().Result)
            {
                await GenerateMarkdown();

                if (FileManagementModalRef != null)
                {
                    FileManagementModalRef.Name = "Flow.md";
                    FileManagementModalRef?.SaveFile(MarkdownText);
                }
            }

            //return Task.CompletedTask;
        }

        private Task ExportHtml()
        {
            //            if (Validate().Result)
            {
                string htmlText = GenerateHtml().Result;

                if (FileManagementModalRef != null)
                {
                    FileManagementModalRef.Name = "flow.html";
                    FileManagementModalRef.SaveFile(htmlText);
                }
            }
            return Task.CompletedTask;
        }

        string selectedTab = "general";

        private async Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                //await GenerateMarkdown();

                if(MermaidOne != null)
                    await MermaidOne.DisplayDiagram(C4Publisher.Publish(Document, "Context"));
                if (MermaidTwo != null)
                    await MermaidTwo.DisplayDiagram(C4Publisher.Publish(Document, "Container"));
                if (MermaidThree != null)
                    await MermaidThree.DisplayDiagram(C4Publisher.Publish(Document, "Component"));
                
                //_ = InvokeAsync(() => StateHasChanged());
            }

            return;
        }

    }
}
