using AngleSharp;
using AngleSharp.Dom;
using ReadHtml.Constant;

namespace ReadHtml.Common
{
    public static class AngleSharpUtils
    {
        public static async Task<IDocument> ReadPathHtml(string path)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var html = File.ReadAllText(path);

            return await context.OpenAsync(req => req.Content(html));
        }

        public static async Task<IDocument> ConvertElementToDoc(string html)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            return await context.OpenAsync(req => req.Content(html));
        }

        public static IHtmlCollection<IElement> GetAllData(IDocument doc, string query)
        {
            var data = doc.QuerySelectorAll(query);

            return data;

        }
    }
}
