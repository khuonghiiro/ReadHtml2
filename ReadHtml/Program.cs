using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using ReadHtml.Common;
using ReadHtml.Constant;
using ReadHtml.Handles;
using System.Text.RegularExpressions;

string path = @"D:\sample.txt";

var handleHtml = new HandleHtml();

var doc = await AngleSharpUtils.ReadPathHtml(path);

string? query = Constants.DIV_CLASS_PARENT;

var data = AngleSharpUtils.GetAllTagHtml(doc, query);

var lst = await handleHtml.GetListRoot(data[0]);

string json = JsonConvert.SerializeObject(lst, new JsonSerializerSettings()
{
    NullValueHandling = NullValueHandling.Ignore,
    Formatting = Formatting.Indented,
    DefaultValueHandling = DefaultValueHandling.Ignore
});
Console.WriteLine(json);