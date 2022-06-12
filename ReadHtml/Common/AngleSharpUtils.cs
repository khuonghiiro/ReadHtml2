using AngleSharp;
using AngleSharp.Dom;
using ReadHtml.Constant;
using System.Text.RegularExpressions;

namespace ReadHtml.Common
{
    public static class AngleSharpUtils
    {
        public static async Task<IDocument> ReadPathHtml(string path)
        {
            var html = string.Empty;
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            try
            {
                html = File.ReadAllText(path);
            }
            catch(Exception)
            {
                Console.WriteLine("Error read file");
            }
            

            return await context.OpenAsync(req => req.Content(html));
        }

        public static async Task<IDocument> ConvertElementToDoc(string html)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            return await context.OpenAsync(req => req.Content(html));
        }

        public static IHtmlCollection<IElement> GetAllTagHtml(IDocument doc, string query)
        {
            var data = doc.QuerySelectorAll(query);

            return data;

        }
    }
}
