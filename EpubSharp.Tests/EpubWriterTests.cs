using System.IO;
using System.Linq;
using EpubSharp.Format;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpubSharp.Tests
{
    [TestClass]
    public class EpubWriterTests
    {
        [TestMethod]
        public void CanWriteTest()
        {
            var book = EpubReader.Read(Cwd.Combine(@"Samples/epub-assorted/Inversions - Iain M. Banks.epub"));
            var writer = new EpubWriter(book);
            writer.Write(new MemoryStream());
        }

        [TestMethod]
        public void CanCreateEmptyEpubTest()
        {
            var epub = WriteAndRead(new EpubWriter());

            Assert.IsNull(epub.Title);
            Assert.Equals(0, epub.Authors.Count());
            Assert.IsNull(epub.CoverImage);

            Assert.Equals(0, epub.Resources.Html.Count);
            Assert.Equals(0, epub.Resources.Css.Count);
            Assert.Equals(0, epub.Resources.Images.Count);
            Assert.Equals(0, epub.Resources.Fonts.Count);
            Assert.Equals(1, epub.Resources.Other.Count); // ncx
            
            Assert.Equals(0, epub.SpecialResources.HtmlInReadingOrder.Count);
            Assert.IsNull(epub.SpecialResources.Ocf);
            Assert.IsNotNull(epub.SpecialResources.Opf);

            Assert.Equals(0, epub.TableOfContents.Count);

            Assert.IsNotNull(epub.Format.Ocf);
            Assert.IsNotNull(epub.Format.Opf);
            Assert.IsNotNull(epub.Format.Ncx);
            Assert.IsNull(epub.Format.Nav);
        }

        [TestMethod]
        public void AddRemoveAuthorTest()
        {
            var writer = new EpubWriter();

            writer.AddAuthor("Foo Bar");
            var epub = WriteAndRead(writer);
            Assert.Equals(1, epub.Authors.Count());

            writer.AddAuthor("Zoo Gar");
            epub = WriteAndRead(writer);
            Assert.Equals(2, epub.Authors.Count());

            writer.RemoveAuthor("Foo Bar");
            epub = WriteAndRead(writer);
            Assert.Equals(1, epub.Authors.Count());
            Assert.Equals("Zoo Gar", epub.Authors.First());

            writer.RemoveAuthor("Unexisting");
            epub = WriteAndRead(writer);
            Assert.Equals(1, epub.Authors.Count());

            writer.ClearAuthors();
            epub = WriteAndRead(writer);
            Assert.Equals(0, epub.Authors.Count());

            writer.RemoveAuthor("Unexisting");
            writer.ClearAuthors();
        }

        [TestMethod]
        public void AddRemoveTitleTest()
        {
            var writer = new EpubWriter();

            writer.SetTitle("Title1");
            var epub = WriteAndRead(writer);
            Assert.Equals("Title1", epub.Title);

            writer.SetTitle("Title2");
            epub = WriteAndRead(writer);
            Assert.Equals("Title2", epub.Title);

            writer.RemoveTitle();
            epub = WriteAndRead(writer);
            Assert.IsNull(epub.Title);

            writer.RemoveTitle();
        }

        [TestMethod]
        public void SetCoverTest()
        {
            var writer = new EpubWriter();
            writer.SetCover(File.ReadAllBytes(Cwd.Combine("Cover.png")), ImageFormat.Png);

            var epub = WriteAndRead(writer);

            Assert.Equals(1, epub.Resources.Images.Count);
            Assert.IsNotNull(epub.CoverImage);
        }

        [TestMethod]
        public void RemoveCoverTest()
        {
            var epub1 = EpubReader.Read(Cwd.Combine(@"Samples/epub-assorted/Inversions - Iain M. Banks.epub"));

            var writer = new EpubWriter(EpubWriter.MakeCopy(epub1));
            writer.RemoveCover();

            var epub2 = WriteAndRead(writer);

            Assert.IsNotNull(epub1.CoverImage);
            Assert.IsNull(epub2.CoverImage);
            Assert.Equals(epub1.Resources.Images.Count - 1, epub2.Resources.Images.Count);
        }

        [TestMethod]
        public void RemoveCoverWhenThereIsNoCoverTest()
        {
            var writer = new EpubWriter();
            writer.RemoveCover();
            writer.RemoveCover();
        }

        [TestMethod]
        public void CanAddChapterTest()
        {
            var writer = new EpubWriter();
            var chapters = new[]
            {
                writer.AddChapter("Chapter 1", "bla bla bla"),
                writer.AddChapter("Chapter 2", "foo bar")
            };
            var epub = WriteAndRead(writer);

            Assert.Equals("Chapter 1", chapters[0].Title);
            Assert.Equals("Chapter 2", chapters[1].Title);

            Assert.Equals(2, epub.TableOfContents.Count);
            for (var i = 0; i < chapters.Length; ++i)
            {
                Assert.Equals(chapters[i].Title, epub.TableOfContents[i].Title);
                Assert.Equals(chapters[i].FileName, epub.TableOfContents[i].FileName);
                Assert.Equals(chapters[i].Anchor, epub.TableOfContents[i].Anchor);
                Assert.Equals(0, chapters[i].SubChapters.Count);
                Assert.Equals(0, epub.TableOfContents[i].SubChapters.Count);
            }
        }

        [TestMethod]
        public void ClearChaptersTest()
        {
            var writer = new EpubWriter();
            writer.AddChapter("Chapter 1", "bla bla bla");
            writer.AddChapter("Chapter 2", "foo bar");
            writer.AddChapter("Chapter 3", "fooz barz");

            var epub = WriteAndRead(writer);
            Assert.Equals(3, epub.TableOfContents.Count);

            writer = new EpubWriter(epub);
            writer.ClearChapters();
            
            epub = WriteAndRead(writer);
            Assert.Equals(0, epub.TableOfContents.Count);
        }

        [TestMethod]
        public void ClearBogtyvenChaptersTest()
        {
            var writer = new EpubWriter(EpubReader.Read(Cwd.Combine(@"Samples/epub-assorted/bogtyven.epub")));
            writer.ClearChapters();

            var epub = WriteAndRead(writer);
            Assert.Equals(0, epub.TableOfContents.Count);
        }

        [TestMethod]
        public void AddFileTest()
        {
            var writer = new EpubWriter();
            writer.AddFile("style.css", "body {}", EpubContentType.Css);
            writer.AddFile("img.jpeg", new byte[] { 0x42 }, EpubContentType.ImageJpeg);
            writer.AddFile("font.ttf", new byte[] { 0x24 }, EpubContentType.FontTruetype);

            var epub = WriteAndRead(writer);

            Assert.Equals(1, epub.Resources.Css.Count);
            Assert.Equals("style.css", epub.Resources.Css.First().FileName);
            Assert.Equals("body {}", epub.Resources.Css.First().TextContent);

            Assert.Equals(1, epub.Resources.Images.Count);
            Assert.Equals("img.jpeg", epub.Resources.Images.First().FileName);
            Assert.Equals(1, epub.Resources.Images.First().Content.Length);
            Assert.Equals(0x42, epub.Resources.Images.First().Content.First());

            Assert.Equals(1, epub.Resources.Fonts.Count);
            Assert.Equals("font.ttf", epub.Resources.Fonts.First().FileName);
            Assert.Equals(1, epub.Resources.Fonts.First().Content.Length);
            Assert.Equals(0x24, epub.Resources.Fonts.First().Content.First());
        }

        private EpubBook WriteAndRead(EpubWriter writer)
        {
            var stream = new MemoryStream();
            writer.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var epub = EpubReader.Read(stream, false);
            return epub;
        }
    }
}
