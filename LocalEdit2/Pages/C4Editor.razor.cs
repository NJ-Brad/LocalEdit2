using Microsoft.AspNetCore.Components;
using Blazorise;
using LocalEdit2.C4Types;
using LocalEdit2.Modals;
using System.Text.Json;
//using StardustDL.RazorComponents.Markdown;
using LocalEdit2.Shared;
using System.Reflection.Metadata;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using static LocalEdit2.Pages.Index;
using System;
using Blazorise.DataGrid;
using System.Xml.Schema;

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
        C4FlatItemEditModal? c4FlatItemModalRef = null;

//        bool dataHasChanged = false;

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

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

        private Task ShowFlatItemModal(C4TypeEnum parentType)
        {
            //if (selectedFlatItem == null)
            //{
            //    return Task.CompletedTask;
            //}

            if (c4FlatItemModalRef != null)
            {
                c4FlatItemModalRef.ParentType = parentType;
                c4FlatItemModalRef.SelectedNode = SelectedFlatItem;

                c4FlatItemModalRef?.ShowModal();
            }

            return Task.CompletedTask;
        }

        bool adding;

        private Task AddNewItem()
        {
            if (Document == null)
                return Task.CompletedTask;

            selectedNodeAtTimeOfClick = SelectedNode;
            parentOfNode = FindParent(selectedNodeAtTimeOfClick, Document.Model);

            C4Item newItem = new ();

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
        }

        private Task AddNewFlatItem()
        {
            if (Document == null)
                return Task.CompletedTask;

            // Add a new item, at this level, after this item and before the next item at this level
            int insertLoc = Document.FlatModel.GetAddIndex(selectedFlatItem);

            C4FlatItem newItem = new();
            newItem.Level = selectedFlatItem == null ? 0 : selectedFlatItem.Level;
            newItem.ParentAlias = selectedFlatItem == null ? "" : selectedFlatItem.ParentAlias;
            if (insertLoc != -1)
            {
                Document.FlatModel.Insert(insertLoc, newItem);
            }
            else
            {
                Document.FlatModel.Add(newItem);
            }

            C4FlatItem? parent = selectedFlatItem == null ? null : Document.FlatModel.FindByAlias(selectedFlatItem.ParentAlias);
            

            C4TypeEnum parentType = parent == null ? C4TypeEnum.Unknown : parent.ItemType;
            SelectedFlatItem = newItem;
            adding = true;

//            InvokeAsync(() => StateHasChanged());

            return ShowFlatItemModal(parentType);
        }

        private Task AddNewChildItem()
        {
            selectedNodeAtTimeOfClick = SelectedNode;
            parentOfNode = SelectedNode;

            C4Item newItem = new ();

            parentNode = SelectedNode;

            SelectedNode?.Children.Add(newItem);
            SelectedNode = newItem;
            adding = true;

            InvokeAsync(() => StateHasChanged());

            return ShowItemModal();
        }

        private Task AddNewChildFlatItem()
        {
            if (Document == null)
                return Task.CompletedTask;

            // selection HAS to exist, or we shouldn't be here
            if(selectedFlatItem == null)
            {
                throw new ArgumentNullException(nameof(selectedFlatItem));
            }

            // Add a new item, at this level, after this item and before the next item at this level
            int insertLoc = Document.FlatModel.GetAddChildIndex(selectedFlatItem);

            C4FlatItem newItem = new();
            newItem.Level = selectedFlatItem.Level + 1;
            newItem.ParentAlias = selectedFlatItem.Alias;

            if (insertLoc != -1)
            {
                Document.FlatModel.Insert(insertLoc, newItem);
            }
            else
            {
                Document.FlatModel.Add(newItem);
            }

            C4TypeEnum parentType = SelectedFlatItem == null ? C4TypeEnum.Unknown : SelectedFlatItem.ItemType;

            SelectedFlatItem = newItem;
            adding = true;

            //            InvokeAsync(() => StateHasChanged());

            // use the current item as the parent item, for item type filtering
            return ShowFlatItemModal(parentType);
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


        private Task EditFlatItem()
        {
            if(Document == null)
                throw new ArgumentNullException(nameof (Document));

            C4FlatItem? parent = selectedFlatItem == null ? null : Document.FlatModel.FindByAlias(selectedFlatItem.ParentAlias);

            C4TypeEnum parentType = parent == null ? C4TypeEnum.Unknown : parent.ItemType;

            return ShowFlatItemModal(parentType);
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
                    new C4Item{ItemType=C4TypeEnum.System, Text ="Web Application", Technology="Java, Spring MVC", Description="Delivers the static content and the Internet banking SPA" }
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

        C4FlatItem? selectedFlatItem = null;

        string MarkdownText { get; set; } = string.Empty;

        Mermaid? MermaidOne { get; set; }
        Mermaid? MermaidTwo { get; set; }
        Mermaid? MermaidThree { get; set; }

        public C4Item? SelectedNode { get => selectedNode; 
            set
            {
                selectedNode = value; 
                if(Document != null)
                    parentNode = FindParent(value, Document.Model);
            }
        }

        public C4FlatItem? SelectedFlatItem
        {
            get => selectedFlatItem;
            set
            {
                selectedFlatItem = value;
                //if (Document != null)
                //    parentNode = FindParent(value, Document.Model);
            }
        }

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

            InvokeAsync(() => StateHasChanged());

            return Task.CompletedTask;
        }

        private Task OnC4FlatItemModalClosed()
        {
            if (Document == null)
                throw new ArgumentNullException(nameof(Document));

            if (adding)
            {
                // remove the new item, if add was cancelled
                if (c4FlatItemModalRef?.Result == ModalResult.Cancel)
                {
                    if (SelectedFlatItem != null)
                    {
                        int itemIndex = Document.FlatModel.IndexOf(SelectedFlatItem);

                        if(itemIndex != -1)
                        {
                            Document.FlatModel.RemoveAt(itemIndex);
                        }
                    }
                    SelectedFlatItem = null;
                }
            }
            adding = false;

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

        private Task DeleteFlatItem()
        {
            if (Document == null)
                throw new ArgumentNullException(nameof(Document));

            if (SelectedFlatItem != null)
            {
                int itemIndex = Document.FlatModel.IndexOf(SelectedFlatItem);

                if (itemIndex != -1)
                {
                    Document.FlatModel.RemoveAt(itemIndex);
                }

                SelectedFlatItem = null;
            }

            return Task.CompletedTask;
        }

    //protected string TestVal { get; set; }

    private async Task GenerateMarkdown()
        {
            MarkdownText = MarkdownGenerator.WrapMermaid("Context Diagram", C4Publisher.Publish(Document, "Context"),
                "Container Diagram", C4Publisher.Publish(Document, "Container"),
                "Component Diagram", C4Publisher.Publish(Document, "Component"));

            await InvokeAsync(() => StateHasChanged());

        }

        private Task<string> GenerateHtml()
        {
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

        string diagramOneText = string.Empty;
        string diagramTwoText = string.Empty;
        string diagramThreeText = string.Empty;

        private Task OnSelectedTabChanged(string name)
        {
            selectedTab = name;

            if (selectedTab == "preview")
            {
                diagramOneText = C4Publisher.Publish(Document, "Context");
                diagramTwoText = C4Publisher.Publish(Document, "Container");
                diagramThreeText = C4Publisher.Publish(Document, "Component");
            }

            return Task.CompletedTask;
        }

        // https://stackoverflow.com/questions/66965107/blazor-how-to-conditionally-style-an-element
        private string StyleForLevel(int n)
        {
            //if (n > 100) return "";
            //if (n < 1) return "color:red";
            //return "background:lightgreen";

            int emValue = n * 2;

            return $"padding-left:{emValue}em";
        }

        // https://v094.blazorise.com/docs/extensions/datagrid/
        void OnSelectedRowStyling(C4FlatItem item, DataGridRowStyling styling)
        {
            //styling.Background = Background.Info;
            styling.Background = Background.Default;
            //styling.Color = Color.Default;
        }
    }
}
