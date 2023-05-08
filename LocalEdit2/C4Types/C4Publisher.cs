using System.Collections.ObjectModel;
using System.Text;
namespace LocalEdit2.C4Types
{
    public class C4Publisher
    {
        public static string Publish(C4Workspace workspace)
        {
            StringBuilder sb = new StringBuilder();

            //sb.Append(MermaidHeader(workspace));

            //foreach (var item in workspace.Items)
            //{
            //    //item = workspace.items[itmNum];
            //    sb.Append(MermaidItem(item));
            //}

            //foreach (var rel in workspace.Relationships)
            //{
            //    sb.Append(MermaidConnection(rel));
            //}

            return sb.ToString();
        }

        Dictionary<string, string> redirections = new();

    public string diagramType = "";

        public static string Publish(C4Workspace? workspace, string diagramType)
        {
            if (workspace == null)
                return string.Empty;

            C4Publisher me = new C4Publisher();
            return me.Publish_(workspace, diagramType);
        }

        //    public string Publish(C4Workspace workspace, string diagramType, string format)
        public string Publish_(C4Workspace workspace, string diagramType)
        {

        this.diagramType = diagramType;
        redirections.Clear();

        string rtnVal = "";
//        switch (format) {
//            case "MERMAID":
//                switch (diagramType) {
//                    case "Context":
//                        rtnVal = this.PublishMermaidContext(workspace);
//                        break;
//                    case "Container":
//                        rtnVal = this.PublishMermaidContainer(workspace);
//                        break;
//                    case "Component":
//                        rtnVal = this.PublishMermaidComponent(workspace);
//                        break;
//                    default:
//                        rtnVal = this.PublishMermaid(workspace);
//                        break;
//                }
//break;
//            case "PLANT":
                switch (diagramType)
{
    case "Context":
        rtnVal = this.PublishPlantContext(workspace);
        break;
    case "Container":
        rtnVal = this.PublishPlantContainer(workspace);
        break;
    case "Component":
        rtnVal = this.PublishPlantComponent(workspace);
        break;
    default:
        rtnVal = this.PublishPlant(workspace);
        break;
}
//break;
//        }

        return rtnVal;
    }

//    private publishMermaidComponent(workspace: C4Workspace): string
//{
//    return this.publishMermaid(workspace);
//}

//private publishMermaid(workspace: C4Workspace): string
//{
//    var sb: StringBuilder = new StringBuilder();

//    sb.append(this.mermaidHeader(workspace));

//    for (var item of workspace.items)
//    {
//        sb.append(this.mermaidItem(item));
//    }

//    for (var rel of workspace.relationships)
//    {
//        sb.append(this.mermaidConnection(rel));
//    }

//    return sb.text;
//}

//private publishMermaidContext(workspace: C4Workspace): string
//{
//    var sb: StringBuilder = new StringBuilder();

//    sb.append(this.mermaidHeader(workspace));

//    this.createContextRedirects(workspace.items);

//    for (var item of workspace.items)
//    {
//        sb.append(this.mermaidItem(item));
//    }

//    var connections: string[] = [];
//    var newConn: string;
//    for (var rel of workspace.relationships)
//    {
//        newConn = this.mermaidConnection(rel);

//        if (!this.isInList(newConn, connections))
//        {
//            sb.append(this.mermaidConnection(rel));
//        }
//    }

//    return sb.text;
//}

//private publishMermaidContainer(workspace: C4Workspace): string
//{
//    var sb: StringBuilder = new StringBuilder();

//    sb.append(this.mermaidHeader(workspace));

//    this.createContainerRedirects(workspace.items);

//    for (var item of workspace.items)
//    {
//        sb.append(this.mermaidItem(item));
//    }

//    for (var rel of workspace.relationships)
//    {
//        sb.append(this.mermaidConnection(rel));
//    }

//    return sb.text;
//}

private string PublishPlantComponent(C4Workspace workspace)
{
    return this.PublishPlant(workspace);
}

private string PublishPlant(C4Workspace workspace)
{
        StringBuilder sb = new StringBuilder();

    sb.Append(PlantHeader(workspace));

    foreach (C4Item item in workspace.Model)
    {
        sb.Append(PlantItem(item));
    }

    foreach (C4Relationship rel in workspace.Relationships)
    {
        sb.Append(PlantConnection(rel));
    }

            sb = new(sb.ToString().TrimEnd());
            return sb.ToString();
        }

        private string PublishPlantContext(C4Workspace workspace)
{
            StringBuilder sb = new StringBuilder();

    sb.Append(PlantHeader(workspace, "Context"));

    CreateContextRedirects(workspace.Model);

    foreach (C4Item item in workspace.Model)
    {
        sb.Append(PlantItem(item));
    }

            List<string> connections = new ();
    string newConn;
    foreach (C4Relationship rel in workspace.Relationships)
    {
        newConn = PlantConnection(rel);

                if (!connections.Contains(newConn))
                {
            connections.Add(newConn);
            sb.Append(PlantConnection(rel));
        }
    }
            sb = new(sb.ToString().TrimEnd());
            return sb.ToString();
}

private string PublishPlantContainer(C4Workspace workspace)
{
        StringBuilder sb = new ();

    sb.Append(PlantHeader(workspace, "Container"));

    CreateContainerRedirects(workspace.Model);

    foreach (C4Item item in workspace.Model)
    {
        sb.Append(PlantItem(item));
    }

    foreach (C4Relationship rel in workspace.Relationships)
    {
        sb.Append(PlantConnection(rel));
    }

            sb = new(sb.ToString().TrimEnd());
            return sb.ToString();
        }

        //private isInList(lookFor: string, lookIn: string[]): boolean
        //{
        //    var rtnVal: boolean = false;

        //    for (var lookInItem of lookIn)
        //    {
        //        if (this.ciEquals(lookFor, lookInItem))
        //        {
        //            rtnVal = true;
        //        }
        //    }

        //    return rtnVal;
        //}

        private void CreateContextRedirects(ObservableCollection<C4Item> items, string redirectTo = "") {

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

private void CreateContainerRedirects(ObservableCollection<C4Item> items, string redirectTo = "") {
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

//private mermaidHeader(workspace: C4Workspace): string
//{
//    var sb: StringBuilder = new StringBuilder();
//    sb.append("flowchart TB");
//    sb.append("\r\n");
//    // classDef borderless stroke-width:0px
//    // classDef darkBlue fill:#00008B, color:#fff
//    // classDef brightBlue fill:#6082B6, color:#fff
//    // classDef gray fill:#62524F, color:#fff
//    // classDef gray2 fill:#4F625B, color:#fff

//    // ");

//    return sb.text;
//}

private string PlantHeader(C4Workspace workspace, string diagramType = "Component")
{
            StringBuilder sb = new ();

    sb.AppendLine($"C4{diagramType}");

    return sb.ToString();
}

private string BuildIndentation(int level) {
    string rtnVal = "";

    for (var i = 0; i < (4 * level); i++)
    {
        rtnVal = rtnVal + " ";
    }
    return rtnVal;
}

//private mermaidItem(item: C4Item, indent: number = 1): string
//{
//    var sb: StringBuilder = new StringBuilder();

//    var indentation: string = this.buildIndentation(indent);
//    var displayType: string = item.itemType;
//    var goDeeper: boolean = true;

//    switch (item.itemType)
//    {
//        case "PERSON":
//            if (item.external)
//            {
//                displayType = "External Person";
//            }
//            else
//            {
//                displayType = "Person";
//            }
//            break;
//        case "SYSTEM":
//            if (item.external)
//            {
//                displayType = "External System";
//            }
//            else
//            {
//                if (this.ciEquals(this.diagramType, "Context"))
//                {
//                    goDeeper = false;
//                    displayType = "System";
//                }
//                else if (item.items.length === 0)
//                {
//                    displayType = "System";
//                }
//                else
//                {
//                    displayType = "System Boundary";
//                }
//            }
//            break;
//        case "CONTAINER":
//            if (item.external)
//            {
//                displayType = "External Container";
//            }
//            else
//            {
//                if (this.ciEquals(this.diagramType, "Container"))
//                {
//                    goDeeper = false;
//                    displayType = "Container";
//                }
//                else if (item.items.length === 0)
//                {
//                    displayType = "Container";
//                }
//                else
//                {
//                    displayType = "Container Boundary";
//                }
//            }
//            break;
//        case "DATABASE":
//            if (item.external)
//            {
//                displayType = "External Database";
//            }
//            else
//            {
//                if (this.ciEquals(this.diagramType, "Container"))
//                {
//                    goDeeper = false;
//                    displayType = "Database";
//                }
//                else if (item.items.length === 0)
//                {
//                    displayType = "Database";
//                }
//                else
//                {
//                    displayType = "Database Boundary";
//                }
//            }
//            break;
//    }

//    var displayLabel: string = `\"<strong><u>${item.label}</u></strong>`;
//        var brokenDescription: string = item.description.replace("`", "<br/>");

//    if (item.description.length !== 0)
//    {
//        displayLabel = displayLabel + `< br />${ brokenDescription}`;
//    }

//    displayLabel += `< br /> &#171;${displayType}&#187;\"`;

//        if (!goDeeper || (item.items.length === 0))
//    {
//        sb.append(`${ indentation}${ item.id}
//        [${ displayLabel}]`);
//        sb.append("\r\n");
//    }
//    else
//    {
//        sb.append(`${ indentation}
//        subgraph ${ item.id}
//        [${ displayLabel}]`);
//        sb.append("\r\n");
//        indent++;

//        var item2: C4Item;
//        for (var itmNum = 0; itmNum < item.items.length; itmNum++)
//        {
//            item2 = item.items[itmNum];
//            sb.append(this.mermaidItem(item2, indent).trimEnd());
//            sb.append("\r\n");
//        }
//        sb.append(`${ indentation}
//        end`);
//        sb.append("\r\n");
//    }

//    return sb.text;
//}

//private mermaidConnection(rel: C4Relationship, indent: number = 1): string
//{
//    var sb: StringBuilder = new StringBuilder();

//    var indentation: string = this.buildIndentation(indent);

//    var from: string = rel.from;
//    var to: string = rel.to;
//    var redirected: boolean = false;

//    if (this.redirections.has(from))
//    {
//        from = this.redirections.get(from)!;
//        redirected = true;
//    }

//    if (this.redirections.has(to))
//    {
//        to = this.redirections.get(to)!;
//        redirected = true;
//    }

//    if (from === to)
//    {
//        return "";
//    }

//    if (redirected || (rel.technology.length === 0))
//    {
//        sb.append(`${ indentation}${ from}
//        --\"${rel.label}\"-->${to}`);
//            sb.append("\r\n");
//    }
//    else
//    {
//        sb.append(`${ indentation}${ from}
//        --\"${rel.label}<br>[${rel.technology}]\"-->${to}`);
//            sb.append("\r\n");
//    }

//    return sb.text;
//}

private string PlantItem(C4Item item, int indent = 1)
{
        StringBuilder sb = new StringBuilder();

    string indentation = BuildIndentation(indent);
    string displayType = item.ItemType.ToString();
    bool goDeeper = true;

    switch (item.ItemType)
    {
        case C4TypeEnum.Person:
            displayType = "Person";
            break;
                case C4TypeEnum.System:
            if (item.IsExternal)
            {
                displayType = "System";
            }
            else
            {
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
                    displayType = "System_Boundary";
                }
            }
            break;
                case C4TypeEnum.Container:
            if (item.IsExternal)
            {
                displayType = "Container";
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
                    displayType = "Container_Boundary";
                }
            }
            break;
                case C4TypeEnum.EnterpriseBoundary:
            displayType = "Enterprise_Boundary";
            break;
                case C4TypeEnum.Database:
            goDeeper = false;
            displayType = this.diagramType;
            break;
    }

    string plantText = FormatPlantItem(displayType, item);

    if (!goDeeper || (item.Children.Count == 0))
    {
        sb.AppendLine($"{indentation}{plantText}");
    }
    else
    {
        sb.AppendLine(@$"{indentation}{plantText}
        {{");
            indent++;

            C4Item item2;
            for (var itmNum = 0; itmNum < item.Children.Count; itmNum++)
            {
                item2 = item.Children[itmNum];
                sb.AppendLine(PlantItem(item2, indent).TrimEnd());
            }
            // somehow an extra blank line got added.  this is an attempt to remove it
                sb = new (sb.ToString().TrimEnd());
            sb.AppendLine($@"{indentation}
        }}");
    }

    return sb.ToString();
}

public string label = "";
public string description = "";
public bool external = false;
public string technology = "";
public bool database = false;
//private string _id = "";


private string FormatPlantItem(string command, C4Item item)
{
        StringBuilder sb = new ();

    sb.Append(command);

    if (item.IsDatabase)
    {
        sb.Append("Db");
    }
    if (item.IsExternal)
    {
        sb.Append("_Ext");
    }

    sb.Append("(");

    sb.Append(item.Alias);

    if (item.Text != "")
    {
        sb.Append($", \"{item.Text}\"");

            if (item.Description != "")
        {
            sb.Append($", \"{item.Description}\"");

                if (item.Technology != "")
            {
                sb.Append($", \"{item.Technology}\"");
                }
        }
    }

    sb.Append(")");

    return sb.ToString();
}

private string PlantConnection(C4Relationship rel, int indent = 1)
{
        StringBuilder sb = new ();

    string indentation = BuildIndentation(indent);

    string from = rel.From;
    string to = rel.To;
    bool redirected = false;

    if (redirections.ContainsKey(from))
    {
        from = redirections[from];
        redirected = true;
    }

    if (redirections.ContainsKey(to))
    {
        to = redirections[to];
        redirected = true;
    }

    if (from == to)
    {
        return "";
    }

    if (redirected || (rel.Technology.Length == 0))
    {
        sb.AppendLine(@$"{indentation}
                Rel({from}, {to}, ""{rel.Text}"")");
        }
    else
    {
        sb.AppendLine(@$"{indentation}
        Rel({from}, {to}, ""{rel.Text}"", ""{rel.Technology}"")");
    }

    return sb.ToString();
}

//// https://stackoverflow.com/questions/2140627/how-to-do-case-insensitive-string-comparison
//ciEquals(a: string, b: string) {
//    return typeof a === 'string' && typeof b === 'string'
//        ? a.localeCompare(b, undefined, { sensitivity: 'accent' }) === 0
//            : a === b;
//}
    }
}
