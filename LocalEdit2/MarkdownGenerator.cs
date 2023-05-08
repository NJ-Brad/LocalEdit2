using System.Text;

namespace LocalEdit2
{
    public class MarkdownGenerator
    {
        public static string WrapMermaid(string mermaidString)
        {
            return $@"```mermaid
{mermaidString}
```";
        }
        public static string WrapMermaid(string headingOne, string mermaidStringOne, string headingTwo = "", string mermaidStringTwo = "", string headingThree = "", string mermaidStringThree = "")
        {
            StringBuilder sb = new();

            sb.Append(WrapHeading(headingOne));
            sb.AppendLine(WrapContent(mermaidStringOne));
            sb.Append(WrapHeading(headingTwo));
            sb.AppendLine(WrapContent(mermaidStringTwo));
            sb.Append(WrapHeading(headingThree));
            sb.AppendLine(WrapContent(mermaidStringThree));

            return sb.ToString();
        }

        public static string WrapHeading(string headingText)
        {
            string rtnVal = "";
            if (!string.IsNullOrEmpty(headingText))
            {
                rtnVal = @$"# {headingText}  
";
            }

            return rtnVal;
        }
        public static string WrapContent(string contentText)
        {
            string rtnVal = "";
            if (!string.IsNullOrEmpty(contentText))
            {
                rtnVal = $@"```mermaid
{contentText}
```";
            }

            return rtnVal;

        }
    }
}

