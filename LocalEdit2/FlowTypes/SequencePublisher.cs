using Blazorise;
using System.Text;
using static LocalEdit2.Pages.Index;

namespace LocalEdit2.FlowTypes
{
    public class SequencePublisher
    {
        public static string Publish(FlowDocument Sequence)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(MermaidHeader(Sequence));

            foreach (var item in Sequence.items)
            {
                //item = workspace.items[itmNum];
                sb.Append(MermaidItem(item));
            }

            foreach (var item in Sequence.items)
            {
                foreach (var rel in item.NextItems)
                {
                    sb.Append(MermaidConnection(rel, Sequence.items));
                }
            }

            return sb.ToString();
        }

        private static string MermaidHeader(FlowDocument Sequence)
        {
            StringBuilder sb = new StringBuilder();

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

        private static string DecodeFlowId(List<FlowItem> items, string id)
        {
            string rtnVal = id;

            foreach (FlowItem fi in items)
            {
                if (fi.id == id)
                {
                    rtnVal = fi.title;
                    break;
                }
            }

            return rtnVal;
        }

        private static string MermaidConnection(FlowRelationship rel, List<FlowItem> items, int indent = 1)
        {
            StringBuilder sb = new StringBuilder();

            string indentation = BuildIndentation(indent);

            string from = DecodeFlowId(items, rel.From);
            string to = DecodeFlowId(items, rel.To);

            sb.AppendLine($"{indentation}{from}->>{to}: {rel.Label}");

            // solid
            // Alice->> John: Hello John, how are you?

            // dotted
            //John-- >> Alice: Great!

            return sb.ToString();
        }


    }
}