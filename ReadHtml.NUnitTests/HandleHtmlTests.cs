using ReadHtml.Handles;

namespace ReadHtml.NUnitTests
{
    public class HandleHtmlTests
    {
        private HandleHtml _handleHtml { get; set; } = null!;

        [SetUp]
        public void Setup()
        {
            _handleHtml = new HandleHtml();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}