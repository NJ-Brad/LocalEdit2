using System.Text;

namespace LocalEdit2
{
    public class HtmlGenerator
    {
        public static string WrapMermaid(string mermaidString)
        {
            return $@"<!DOCTYPE html >
 <html lang = ""en"" >
    <head >
      <meta charset = ""utf-8"" />
     </head >
     <body >
       <pre class=""mermaid"">
{mermaidString}
"
+ @"</pre>
    <script type = ""module"" >
            import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@10/dist/mermaid.esm.min.mjs';
            mermaid.initialize({startOnLoad: true });
    </script >
  </body >
</html >
";
        }
        public static string WrapMermaid(string headingOne, string mermaidStringOne, string headingTwo = "", string mermaidStringTwo = "", string headingThree = "", string mermaidStringThree = "")
        {
            StringBuilder sb = new();

            sb.Append(GetHeader());
            sb.Append(WrapHeading(headingOne));
            sb.Append(WrapContent(mermaidStringOne));
            sb.Append(WrapHeading(headingTwo));
            sb.Append(WrapContent(mermaidStringTwo));
            sb.Append(WrapHeading(headingThree));
            sb.Append(WrapContent(mermaidStringThree));
            sb.Append(GetFooting());

            return sb.ToString();
        }

        public static string GetHeader()
        {
            return $@"<!DOCTYPE html>
 <html lang = ""en"" >
    <head >
      <meta charset = ""utf-8"" />
     </head >
     <body >
";
        }

        public static string WrapHeading(string headingText)
        {
            string rtnVal = "";
            if (!string.IsNullOrEmpty(headingText))
            {
                rtnVal = @$"<h1>{headingText}</h1>
";
            }

            return rtnVal;
        }
        public static string WrapContent(string contentText)
        {
            string rtnVal = "";
            if (!string.IsNullOrEmpty(contentText))
            {
                rtnVal = @$"<pre class=""mermaid"">
{contentText}
</pre>";
            }

            return rtnVal;

        }

        public static string GetFooting()
        {
            return $@"<script type = ""module"" >
      import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@9/dist/mermaid.esm.min.mjs';
            mermaid.initialize({{startOnLoad: true }});
    </script >
  </body >
</html >
";

        }
    }
}

