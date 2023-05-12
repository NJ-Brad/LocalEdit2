using Blazorise;
using LocalEdit2.Shared;
using System.Text;

namespace LocalEdit2.FlowTypes
{
    public class FlowSequencePublisher
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

            // go through again and add all of the connections
            foreach (var item in Flow.items)
            {
                if (item.NextItems != null)
                {
                    foreach (var rel in item.NextItems)
                    {
                        rel.From = Utils.VOD(item.title);
                        sb.Append(MermaidConnection(rel));
                    }
                }
            }

            return sb.ToString();
        }

        private static string MermaidHeader(FlowDocument Flow)
        {
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("graph TD");

            sb.AppendLine("%% Created by LocalEdit");
            sb.AppendLine("sequenceDiagram");

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

            sb.AppendLine(@$"{indentation}participant {item.title}");

            return sb.ToString();
        }

        private static string MermaidConnection(FlowRelationship rel, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);

            string from = rel.From;
            string to = rel.To;

            sb.AppendLine($"{indentation}{from}->>{to}: {rel.Label}");

            // solid
            // Alice->> John: Hello John, how are you?

            // dotted
            //John-- >> Alice: Great!

            return sb.ToString();
        }


    }
}