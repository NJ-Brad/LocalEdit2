//using BlazorPanzoom;
//using BlazorPanzoom;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
//using SkiaSharp;
//using Svg.Skia;
using System.Text;

namespace LocalEdit2.Shared
{
    public partial class Mermaid : ComponentBase
    {
        // https://stackoverflow.com/questions/58346600/why-do-blazor-components-and-elements-not-have-id-attributes
        [Parameter] public string Id { get; set; } = Guid.NewGuid().ToString("N");

        //[Parameter]
        //public RenderFragment? ChildContent { get; set; }

        public async Task TriggerClick()
        {
            var input2 = "graph LR \n" +
                "A[Brad] --- B[Load Balancer] \n" +
                "B-->C[Server01] \n" +
                "B-->D(Server02) \n";

            //            await JSRuntime.InvokeVoidAsync("renderSampleMermaidDiagram");
            await JSRuntime.InvokeVoidAsync("renderMermaidDiagram", this.Id, input2);
            //await JSRuntime.InvokeVoidAsync("renderMermaidDiagram2", "output");
        }

        public string SvgText { get; set; } = "";

        public async Task DisplayDiagram(string input)
        {
            SvgText = await JSRuntime.InvokeAsync<string>("generateMermaidSvg", input);
            //// https://stackoverflow.com/questions/60785749/using-svgs-in-blazor-page
            //ToPng(SvgText);

            await JSRuntime.InvokeVoidAsync("renderMermaidDiagram", this.Id, input);
            //await JSRuntime.InvokeVoidAsync("renderMermaidDiagramAsync", this.Id, input);

            await InvokeAsync(() => StateHasChanged());
        }

        public string GenerateSvg(string input)
        {
            SvgText = JSRuntime.InvokeAsync<string>("generateMermaidSvg", input).Result;
            return SvgText;
        }


        // https://stackoverflow.com/questions/1879395/how-do-i-generate-a-stream-from-a-string
        //public static Stream ToStream(this string value, Encoding encoding)
        //              => new MemoryStream(encoding.GetBytes(value ?? string.Empty));
        //public static Stream ToStream(this string value) => ToStream(value, Encoding.UTF8);
        public Stream ToStream(string value, Encoding encoding)
                      => new MemoryStream(encoding.GetBytes(value ?? string.Empty));
        public Stream ToStream(string value) => ToStream(value, Encoding.UTF8);


        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        void ToPng(string svgText)
        {

            //            // https://stackoverflow.com/questions/53854432/how-to-produce-a-system-drawing-image-from-svg-on-net-core

            //            var svgSrc = Path.Combine(Directory.GetCurrentDirectory(), "img.svg");
            //            string svgSaveAs = "xyz.png";
            //            var quality = 100;

            //            var svg = new SkiaSharp.Extended.Svg.SKSvg();
            //            //var pict = svg.Load(svgSrc);

            //            SKPicture pict = null;

            //            using (var stream = GenerateStreamFromString(svgText))
            //            {
            //                // ... Do stuff to stream
            //                pict = svg.Load(stream);
            //            }

            ////            var pict = svg.Load(ToStream(svgText));

            //            var dimen = new SkiaSharp.SKSizeI(
            //                (int)Math.Ceiling(pict.CullRect.Width),
            //                (int)Math.Ceiling(pict.CullRect.Height)
            //            );
            //            var matrix = SKMatrix.MakeScale(1, 1);
            //            var img = SKImage.FromPicture(pict, dimen, matrix);

            //            // convert to PNG
            //            var skdata = img.Encode(SkiaSharp.SKEncodedImageFormat.Png, quality);
            //            using (var stream = File.OpenWrite(svgSaveAs))
            //            {
            //                skdata.SaveTo(stream);
            //            }

            return;
        }


        //private RenderFragment AddContent(string textContent) => builder =>
        //{
        //    builder.AddContent(1, textContent);
        //};

        //private RenderFragment AddContent2(string textContent) => builder =>
        //{
        //    builder.OpenElement(0, "h1");
        //    builder.AddContent(1, textContent);
        //    builder.CloseElement();
        //};

        private double _rangeValue = 1.0;

        private double RangeValue
        {
            get => _rangeValue;
            set
            {
                _rangeValue = value;
//                _ = _panzoom?.ZoomAsync(value);
            }
        }

        //private bool _panEnabled = true;

        //private bool PanEnabled
        //{
        //    get => _panEnabled;
        //    set
        //    {
        //        _panEnabled = value;
        //        _panzoom.SetOptionsAsync(new PanzoomOptions { DisablePan = !_panEnabled });
        //    }
        //}

//        private Panzoom? _panzoom { get; set; }

        //private async Task OnZoomInClick(MouseEventArgs args)
        //{
        //    await _panzoom.ZoomInAsync();
        //    await UpdateSlider();
        //}

        //private async Task OnZoomOutClick(MouseEventArgs args)
        //{
        //    await _panzoom.ZoomOutAsync();
        //    await UpdateSlider();
        //}

        //private async Task OnResetClick(MouseEventArgs args)
        //{
        //    await _panzoom.ResetAsync();
        //    await UpdateSlider();
        //}

        //private async Task UpdateSlider()
        //{
        //    var scale = await _panzoom.GetScaleAsync();
        //    _rangeValue = scale;
        //}

    }
}
