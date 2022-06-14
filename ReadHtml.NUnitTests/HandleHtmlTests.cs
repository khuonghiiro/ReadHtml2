using Newtonsoft.Json;
using ReadHtml.Common;
using ReadHtml.Constant;
using ReadHtml.Handles;
using ReadHtml.Models;

namespace ReadHtml.NUnitTests
{
    public class HandleHtmlTests
    {
        private HandleHtml _handleHtml { get; set; } = null!;

        private readonly string startupPath =Path.Join(Environment.CurrentDirectory,(@"..\..\..\"));

        [SetUp]
        public void Setup()
        {
            _handleHtml = new HandleHtml();       
        }

        [Test]
        public void TestListRootNormal()
        {
            string? pathAct = Path.Join(startupPath, "Input", "sample_input_01.txt");

            string pathExp = Path.Join(startupPath, "Output", "sample_output_01.txt");

            string expected = File.ReadAllText(pathExp);

            var doc = AngleSharpUtils.GetDocument(pathAct);

            var data = AngleSharpUtils.GetDataFromDoc(doc.Result, Constants.DIV_CLASS_PARENT);

            var listRoot = _handleHtml.GetListRoot(data[0]);

            string actual = JsonConvert.SerializeObject(listRoot, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void TestRootNormal()
        {
            string? pathAct = Path.Join(startupPath, "Input", "sample_normal.txt");

            Root expected = new()
            {
                Index = 1,
                Type = "h2",
                Value = "CHẲNG NGẠI GÌ U23 HÀN QUỐC"
            };

            var doc = AngleSharpUtils.GetDocument(pathAct);

            var data = AngleSharpUtils.GetDataFromDoc(doc.Result, "h2");

            var actual = _handleHtml.GetRoot(1, data[0]);

            Assert.IsTrue(actual.Equals(expected));
        }
    }
}