using AngleSharp;
using AngleSharp.Dom;
using ReadHtml.Constant;
using System.Text.RegularExpressions;

namespace ReadHtml.Common
{
    public static class AngleSharpUtils
    {
        /// <summary>
        /// Get Document data
        /// </summary>
        /// <param name="path">Link file path</param>
        /// <returns>Document data</returns>
        public static async Task<IDocument> GetDocument(string path)
        {
            var html = string.Empty;
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            try
            {
                html = ReadFilePath(path);
            }
            catch (System.IO.IOException io)
            {
                Console.WriteLine(io.Message);
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                System.Environment.Exit(0);
            }

            return await context.OpenAsync(req => req.Content(html));
        }

        /// <summary>
        /// Get data from Document
        /// </summary>
        /// <param name="doc">Document</param>
        /// <param name="query">select query get data from document</param>
        /// <returns>Data html collection element</returns>
        public static IHtmlCollection<IElement> GetDataFromDoc(IDocument doc, string query)
        {

            IHtmlCollection<IElement>? data = null!;

            try
            {
                data = DataFromDocQuery(doc, query);
            }
            catch (System.ArgumentOutOfRangeException ae)
            {
                Console.WriteLine(ae.Message);
                System.Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                System.Environment.Exit(0);
            }

            return data;
        }

        /// <summary>
        /// Read file path
        /// </summary>
        /// <param name="path">Link file path</param>
        /// <returns>String text</returns>
        /// <exception cref="System.IO.IOException"></exception>
        public static string ReadFilePath(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                throw new System.IO.IOException("Path does not exist");
            }

            if (!path.EndsWith(Constants.FILE_EXTENSION_TXT))
            {
                throw new System.IO.IOException("Wrong file format");
            }

            string text = File.ReadAllText(path);

            if (text == string.Empty)
            {
                throw new System.IO.IOException("No data in file");
            }

            if (!IsTextHtml(text))
            {
                throw new Exception("Not text html");
            }

            return text;
        }

        /// <summary>
        /// Check text html
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>True or false</returns>
        private static bool IsTextHtml(string text)
        {
            Regex tagRegex = new Regex(@"<\s*([^ >]+)[^>]*>.*?<\s*/\s*\1\s*>");

            if (tagRegex.IsMatch(text))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get data from document and query
        /// </summary>
        /// <param name="doc">Document</param>
        /// <param name="query">Query</param>
        /// <returns>Data collection element</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static IHtmlCollection<IElement> DataFromDocQuery(IDocument doc, string query)
        {
            var data = doc.QuerySelectorAll(query);

            if (data.Length == 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(doc), data.Length, "There is no data or the query is incorrect");
            }

            return data;
        }

    }
}
