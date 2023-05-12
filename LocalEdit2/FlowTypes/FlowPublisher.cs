using LocalEdit2.FlowTypes;
using LocalEdit2.Shared;
using System.Text;

namespace LocalEdit2.FlowTypes
{
    public class FlowPublisher
    {
        public static string Publish(FlowDocument Flow)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(MermaidHeader(Flow));

            // go through and get all of the items created
            foreach (var item in Flow.items)
            {
                //item = workspace.items[itmNum];
                sb.Append(MermaidItem(item));
            }

            const bool createDefaultConnection = false;

            // go through again and add all of the connections
            string prevItemId = "";
            foreach (var item in Flow.items)
            {
                if (createDefaultConnection && (prevItemId != ""))
                {
                    // create default connection for "next", by default
                    FlowRelationship rel = new();
                    rel.From = prevItemId;
                    rel.To = Utils.VOD(item.id);
                    rel.Label = " ";
                    sb.Append(MermaidConnection(rel));

                    // reset
                    prevItemId = "";
                }

                if ((item.NextItems == null) || (item.NextItems.Count == 0))
                {
                    prevItemId = Utils.VOD(item.id);
                }

                if (item.NextItems != null)
                {
                    foreach (FlowRelationship rel in item.NextItems)
                    {
                        sb.Append(MermaidConnection(rel));
                    }
                }

                //if ((item.linkLogic == null) || (item.linkLogic.Count == 0))
                //{
                //    prevItemId = Utils.VOD(item.id);
                //}

                //if (item.linkLogic != null)
                //{
                //    foreach (LinkLogic linkLogic in item.linkLogic)
                //    {
                //        FlowRelationship rel = new();
                //        rel.From = Utils.VOD(item.id);
                //        rel.To = linkLogic.jumpToItemId;
                //        rel.Label = linkLogic.asString;
                //        sb.Append(MermaidConnection(rel));
                //    }
                //}
            }

            return sb.ToString();
        }

        private static string MermaidHeader(FlowDocument flow)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("graph TD");
            // classDef borderless stroke-width:0px
            // classDef darkBlue fill:#00008B, color:#fff
            // classDef brightBlue fill:#6082B6, color:#fff
            // classDef gray fill:#62524F, color:#fff
            // classDef gray2 fill:#4F625B, color:#fff

            // ");

            return sb.ToString();
        }

        private static string BuildIndentation(int level)
        {
            string rtnVal = "";

            for (var i = 0; i < (4 * level); i++)
            {
                rtnVal = rtnVal + " ";
            }
            return rtnVal;
        }

        private static string MermaidItem(FlowItem item, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);

            // https://bobbyhadz.com/blog/javascript-typeerror-replaceall-is-not-a-function
            string brokenLabel = String.Join("<br/>", item.title.Split("`"));

            brokenLabel = $"\"{brokenLabel}\"";

            sb.AppendLine(@$"{indentation}{item.id}[{brokenLabel}]");

            return sb.ToString();
        }

        private static string MermaidConnection(FlowRelationship rel, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);

            string from = rel.From;
            string to = rel.To;

            sb.AppendLine($"{indentation}{from}--\"{rel.Label}\"-->{to}");

            return sb.ToString();
        }


    }
}
