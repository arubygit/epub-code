using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpubSharp.Tests
{
    [TestClass]
    public class EpubArchiveTests
    {
        [TestMethod]
        public void FindEntryTest()
        {
            var archive = new EpubArchive(Cwd.Combine("Samples/epub-assorted/Bogtyven.epub"));
            Assert.IsNotNull(archive.FindEntry("META-INF/container.xml"));
            Assert.IsNull(archive.FindEntry("UNEXISTING_ENTRY"));
        }
    }
}
