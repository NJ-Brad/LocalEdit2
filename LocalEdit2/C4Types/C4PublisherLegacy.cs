using System.Collections.ObjectModel;
using System.Text;
namespace LocalEdit2.C4Types
{
    public class C4PublisherLegacy
    {
        public static string Publish(C4Workspace workspace)
        {
            StringBuilder sb = new ();

            return sb.ToString();
        }

        Dictionary<string, string> redirections = new();

        public string diagramType = "";

        public static string Publish(C4Workspace workspace, string diagramType)
        {
            C4PublisherLegacy me = new ();
            return me.Publish_(workspace, diagramType);
        }

        public string Publish_(C4Workspace workspace, string diagramType)
        {
            //diagramType = diagramType;
            redirections.Clear();

            string rtnVal = "";
            switch (diagramType)
            {
                case "Context":
                    rtnVal = PublishMermaidContext(workspace);
                    break;
                case "Container":
                    rtnVal = PublishMermaidContainer(workspace);
                    break;
                case "Component":
                    rtnVal = PublishMermaidComponent(workspace);
                    break;
                default:
                    rtnVal = PublishMermaid(workspace);
                    break;
            }

            return rtnVal;
        }

        private string PublishMermaidComponent(C4Workspace workspace)
        {
            return this.PublishMermaid(workspace);
        }

        private string PublishMermaid(C4Workspace workspace)
    {
            StringBuilder sb = new ();

        sb.Append(MermaidHeader(workspace));

        foreach (C4Item item in workspace.Model)
        {
            sb.Append(MermaidItem(item));
        }

        foreach (C4Relationship rel in workspace.Relationships)
        {
            sb.Append(MermaidConnection(rel));
        }

        return sb.ToString();
    }

        private string PublishMermaidContext(C4Workspace workspace)
    {
            StringBuilder sb = new ();

        sb.Append(MermaidHeader(workspace));

        CreateContextRedirects(workspace.Model);

        foreach (C4Item item in workspace.Model)
        {
            sb.Append(MermaidItem(item));
        }

            List<string> connections = new ();
        string newConn;
        foreach (C4Relationship rel in workspace.Relationships)
        {
            newConn = MermaidConnection(rel);

            if (!connections.Contains(newConn))
            {
                sb.Append(MermaidConnection(rel));
        }
    }

        return sb.ToString();
    }

        private string PublishMermaidContainer(C4Workspace workspace)
{
        StringBuilder sb = new ();

        sb.Append(MermaidHeader(workspace));

    CreateContainerRedirects(workspace.Model);

    foreach (C4Item item in workspace.Model)
    {
        sb.Append(MermaidItem(item));
        }

    foreach (C4Relationship rel in workspace.Relationships)
    {
        sb.Append(MermaidConnection(rel));
        }

    return sb.ToString();
}


    private void CreateContextRedirects(ObservableCollection<C4Item> items, string redirectTo = "")
        {

            C4Item item;
            for (var itmNum = 0; itmNum < items.Count; itmNum++)
            {
                item = items[itmNum];

                if (redirectTo != "")
                {
                    redirections.Add(item.Alias, redirectTo);
                    if (item.Children.Count > 0)
                    {
                        CreateContextRedirects(item.Children, redirectTo);
                    }
                }
                else
                {
                    switch (item.ItemType)
                    {
                        case C4TypeEnum.System:
                        case C4TypeEnum.Database:
                            CreateContextRedirects(item.Children, item.Alias);
                            break;
                        default:
                            // drill down beyond the first level
                            CreateContextRedirects(item.Children);
                            break;
                    }
                }
            }
        }

        private void CreateContainerRedirects(ObservableCollection<C4Item> items, string redirectTo = "")
        {
            C4Item item;
            for (var itmNum = 0; itmNum < items.Count; itmNum++)
            {
                item = items[itmNum];
                if (redirectTo != "")
                {
                    redirections.Add(item.Alias, redirectTo);
                    if (item.Children.Count > 0)
                    {
                        CreateContainerRedirects(item.Children, redirectTo);
                    }
                }
                else
                {
                    switch (item.ItemType)
                    {
                        case C4TypeEnum.Container:
                            CreateContainerRedirects(item.Children, item.Alias);
                            break;
                        default:
                            CreateContainerRedirects(item.Children);
                            break;
                    }
                }
            }
        }

        private string MermaidHeader(C4Workspace workspace)
        {
            StringBuilder sb = new ();
        sb.Append("flowchart TD");
            sb.Append("\r\n");
            // classDef borderless stroke-width:0px
            // classDef darkBlue fill:#00008B, color:#fff
            // classDef brightBlue fill:#6082B6, color:#fff
            // classDef gray fill:#62524F, color:#fff
            // classDef gray2 fill:#4F625B, color:#fff

            // ");

            return sb.ToString();
        }

    //private string PlantHeader(C4Workspace workspace, string diagramType = "Component")
    //{
    //    StringBuilder sb = new();

    //    sb.AppendLine($"C4{diagramType}");

    //    return sb.ToString();
    //}

    private string BuildIndentation(int level)
        {
            string rtnVal = "";

            for (var i = 0; i < (4 * level); i++)
            {
                rtnVal = rtnVal + " ";
            }
            return rtnVal;
        }

        private string MermaidItem(C4Item item, int indent = 1)
        {
            StringBuilder sb = new ();

        string indentation = BuildIndentation(indent);
            string displayType = item.ItemType.ToString();
            bool goDeeper = true;

            switch (item.ItemType)
            {
                case C4TypeEnum.Person:
                    if (item.IsExternal)
                    {
                        displayType = "External Person";
                    }
                    else
                    {
                        displayType = "Person";
                    }
break;
                case C4TypeEnum.System:
                    if (item.IsExternal)
{
    displayType = "External System";
}
else
{
                        //if (this.ciEquals(this.diagramType, "Context"))
                        if (diagramType.Equals("Context", StringComparison.OrdinalIgnoreCase))
                        {
        goDeeper = false;
        displayType = "System";
    }
    else if (item.Children.Count == 0)
    {
        displayType = "System";
    }
    else
    {
        displayType = "System Boundary";
    }
}
break;
                case C4TypeEnum.Container:
                    if (item.IsExternal)
{
    displayType = "External Container";
}
else
{
    if (diagramType.Equals("Container", StringComparison.OrdinalIgnoreCase))
    {
        goDeeper = false;
        displayType = "Container";
    }
    else if (item.Children.Count == 0)
    {
        displayType = "Container";
    }
    else
    {
        displayType = "Container Boundary";
    }
}
break;
                case C4TypeEnum.Database:
                    if (item.IsExternal)
{
    displayType = "External Database";
}
else
{
    if (diagramType.Equals("Container", StringComparison.OrdinalIgnoreCase))
    {
        goDeeper = false;
        displayType = "Database";
    }
    else if (item.Children.Count == 0)
    {
        displayType = "Database";
    }
    else
    {
        displayType = "Database Boundary";
    }
}
break;
            }

            string displayLabel = $"\"<strong><u>{item.Text}</u></strong>";
            //string displayLabel = $"\"&lt;strong&gt;&lt;u&gt;{item.Text}&lt;/u&gt;&lt;/strong&gt;";
            string brokenDescription = item.Description.Replace("`", "<br/>");

            if (item.Description.Length != 0)
            {
                displayLabel = displayLabel + $"<br />{brokenDescription}";
            }

            displayLabel += $"<br /> #171;{displayType}#187;\"";

            if (!goDeeper || (item.Children.Count == 0))
{
    sb.AppendLine($"{indentation}{item.Alias}[{displayLabel}]");
}
else
{
    sb.AppendLine($"{indentation}subgraph {item.Alias}[{displayLabel}]");
    indent++;

                C4Item item2;
    for (var itmNum = 0; itmNum < item.Children.Count; itmNum++)
    {
        item2 = item.Children[itmNum];
        sb.AppendLine(MermaidItem(item2, indent).TrimEnd());
    }
    sb.AppendLine($"{indentation}end");
}

return sb.ToString();
        }

        private string MermaidConnection(C4Relationship rel, int indent = 1)
        {
        StringBuilder sb = new ();

        string indentation = BuildIndentation(indent);

        string from = rel.From;
            string to = rel.To;
            bool redirected = false;

            if (this.redirections.ContainsKey(from))
            {
            from = this.redirections[from]!;
            redirected = true;
        }

            if (this.redirections.ContainsKey(to))
            {
            to = this.redirections[to]!;
            redirected = true;
        }

            if (from == to)
            {
                return "";
            }

            if (redirected || (rel.Technology.Length == 0))
            {
                sb.AppendLine($@"{indentation}{from}--""{rel.Text}""-->{to}");
            }
            else
{
    sb.AppendLine($@"{indentation}{from}--""{rel.Text}<br>[{rel.Technology}]""-->{to}");
}

return sb.ToString();
        }

        public string label = "";
        public string description = "";
        public bool external = false;
        public string technology = "";
        public bool database = false;
        //private string _id = "";

    }
}
