using ReadHtml.Common;
using ReadHtml.Handles;

namespace ReadHtml.NUnitTests
{
    public class AngleSharpUtilsTests
    {
        private readonly string startupPath = Path.Join(Environment.CurrentDirectory, (@"..\..\..\"));

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestReadFilePathEmpty()
        {
            string pathEmpty = string.Empty;

            string expected = "Path does not exist";

            //act
            try
            {
                AngleSharpUtils.ReadFilePath(pathEmpty);

            }
            catch (System.IO.IOException io)
            {
                // Assert
                StringAssert.Contains(expected, io.Message);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void TestReadFilePathNotTxt()
        {
            string pathNotTxt = Path.Join(startupPath, "input", "sample_not_txt.exe");

            string expected = "Wrong file format";

            //act
            try
            {
                AngleSharpUtils.ReadFilePath(pathNotTxt);

            }
            catch (System.IO.IOException io)
            {
                // Assert
                StringAssert.Contains(expected, io.Message);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void TestReadFilePathNotHtml()
        {
            string pathNotTxt = Path.Join(startupPath, "input", "sample_not_html.txt");

            string expected = "Not text html";

            //act
            try
            {
                AngleSharpUtils.ReadFilePath(pathNotTxt);

            }
            catch (Exception e)
            {
                // Assert
                StringAssert.Contains(expected, e.Message);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void TestReadFilePathNotData()
        {
            string pathNotData = Path.Join(startupPath, "input", "sample_not_data.txt");

            string expected = "No data in file";

            //act
            try
            {
                AngleSharpUtils.ReadFilePath(pathNotData);

            }
            catch (System.IO.IOException io)
            {
                // Assert
                StringAssert.Contains(expected, io.Message);
                return;
            }

            Assert.Fail("The expected exception was not thrown.");
        }

        [Test]
        public void TestReadFilePathNormal()
        {
            string pathInput = Path.Join(startupPath, "input", "sample_normal.txt");

            string expected = @"<h2>CHẲNG NGẠI GÌ U23 HÀN QUỐC</h2>";

            string actual = AngleSharpUtils.ReadFilePath(pathInput);

            StringAssert.Contains(expected, actual);
        }

        [Test]
        public void TestGetDataFromDocumentNormal()
        {
            string pathInput = Path.Join(startupPath, "input", "sample_normal.txt");

            string expected = "CHẲNG NGẠI GÌ U23 HÀN QUỐC";

            var document = AngleSharpUtils.GetDocument(pathInput);

            var actual = AngleSharpUtils.GetDataFromDoc(document.Result, "h2");

            StringAssert.Contains(expected, actual[0].TextContent);
        }

        [Test]
        public void TestGetDataFromDocumentAbnormal()
        {
            string pathInput = Path.Join(startupPath, "input", "sample_normal.txt");

            string expected = "There is no data or the query is incorrect";

            try
            {
                var document = AngleSharpUtils.GetDocument(pathInput);

                var data = AngleSharpUtils.DataFromDocQuery(document.Result, "h3");
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                // Assert
                StringAssert.Contains(expected, e.Message);
                return;
            }
            Assert.Fail("The expected exception was not thrown.");
        }
    }
}