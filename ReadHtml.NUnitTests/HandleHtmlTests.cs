using ReadHtml.Common;
using ReadHtml.Handles;
using ReadHtml.Models;

namespace ReadHtml.NUnitTests
{
    public class HandleHtmlTests
    {
        private HandleHtml HandleHtml { get; set; } = null!;

        private readonly string startupPath = Path.Join(Environment.CurrentDirectory, (@"..\..\..\"));

        [SetUp]
        public void Setup()
        {
            HandleHtml = new HandleHtml();
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

            var actual = HandleHtml.GetRoot(1, data[0]);

            Assert.That(actual.Result, Is.EqualTo(expected));
        }
    }
}