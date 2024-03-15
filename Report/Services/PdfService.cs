using DinkToPdf;
using DinkToPdf.Contracts;
using RazorLight;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Report.Services
{
    public class PdfService : IPdfService
    {
        private readonly IConverter _converter;
        private readonly IRazorLightEngine _razorLightEngine;
        public PdfService(IConverter converter, IRazorLightEngine razorLightEngine)
        {
            _converter = converter;
            _razorLightEngine = razorLightEngine;
        }
        public async Task<byte[]> CreateDemoReport()
        {
            System.Dynamic.ExpandoObject keyValuePairs = new System.Dynamic.ExpandoObject();
            //data.logo = "test";

            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Files", "demo1.cshtml");
            string content = await _razorLightEngine.CompileRenderAsync(filePath, keyValuePairs);

            return ConvertToPdf(new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top=10,Bottom=10,Left=10, Right=10},
                DocumentTitle = "DemoReport"
            }, new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = content,
                WebSettings = { DefaultEncoding = "utf-8"},
                FooterSettings = {FontName = "Sarabun",FontSize = 10 ,Line = false, Left="",Right = ""}
            });
        }


        private byte[] ConvertToPdf(GlobalSettings globalSettings,ObjectSettings objectSettings)
        {
            return _converter.Convert(new HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            });
        }
    }
}
