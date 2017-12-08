using EpubSharp;
using EpubSharp.Format;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpubSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read an epub file
            EpubBook book = EpubReader.Read(@"C:\dev\ePub\KortextEpub\EpubSharp.Tests\Samples\Good-and-Evil.epub");

            // Read metadata
            string title = book.Title;
            List<string> authors = book.Authors.ToList();
            byte[] imageCover = book.CoverImage;

            // Get table of contents
            ICollection<EpubChapter> chapters = book.TableOfContents;

            // Get contained files
            ICollection<EpubTextFile> html = book.Resources.Html;
            ICollection<EpubTextFile> css = book.Resources.Css;
            ICollection<EpubByteFile> images = book.Resources.Images;
            ICollection<EpubByteFile> fonts = book.Resources.Fonts;

            // Convert to plain text
            string text = book.ToPlainText();

            // Access internal EPUB format specific data structures.
            EpubFormat format = book.Format;
            OcfDocument ocf = format.Ocf;
            OpfDocument opf = format.Opf;
            NcxDocument ncx = format.Ncx;
            NavDocument nav = format.Nav;

            // Create an EPUB
            EpubWriter.Write(book, "new.epub");

            ///////////////////////////////////

            //writing an epub doc
            EpubWriter writer = new EpubWriter();

            writer.AddAuthor("Foo Bar");
//            writer.SetCover(imageCover, ImageFormat.Png);

            writer.Write("new.epub");
        }
    }
}
