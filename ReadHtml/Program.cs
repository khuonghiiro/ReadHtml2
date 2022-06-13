using Newtonsoft.Json;
using ReadHtml.Common;
using ReadHtml.Constant;
using ReadHtml.Handles;

string path = @"D:\sample.txt";

var handleHtml = new HandleHtml();

var doc = await AngleSharpUtils.GetDocument(path);

var data = AngleSharpUtils.GetDataFromDoc(doc, Constants.DIV_CLASS_PARENT);

var listRoot = await handleHtml.GetListRoot(data[0]);

string json = JsonConvert.SerializeObject(listRoot, new JsonSerializerSettings()
{
    NullValueHandling = NullValueHandling.Ignore,
    Formatting = Formatting.Indented,
    DefaultValueHandling = DefaultValueHandling.Ignore
});
Console.OutputEncoding = System.Text.Encoding.Default;
Console.WriteLine(json);