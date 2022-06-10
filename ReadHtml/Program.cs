using Newtonsoft.Json;
using ReadHtml.Common;
using ReadHtml.Constant;
using ReadHtml.Handle;

string path = @"F:\sample.txt";

int index = Constants.INDEX;

HandleTagHtml handle = new HandleTagHtml();

var doc = await AngleSharpUtils.ReadPathHtml(path);

string? query = Constants.DIV_CLASS_PARENT;

var data = AngleSharpUtils.GetAllData(doc, query);

var lst = await handle.GetListRoot(index, data[0], query);


string json = JsonConvert.SerializeObject(lst, new JsonSerializerSettings()
{
    NullValueHandling = NullValueHandling.Ignore,
    Formatting = Formatting.Indented,
    DefaultValueHandling = DefaultValueHandling.Ignore
});
Console.WriteLine(json);