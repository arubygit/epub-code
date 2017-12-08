﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EpubSharp.Format;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EpubSharp.Tests
{
    [TestClass]
    public class EpubReaderTests
    {
        [TestMethod]
        public void ReadBogtyvenFormatTest()
        {
            var book = EpubReader.Read(Cwd.Combine(@"Samples/Bogtyven.epub"));
            var format = book.Format;

            Assert.IsNotNull(format);

            Assert.IsNotNull(format.Ocf);
            Assert.Equals(1, format.Ocf.RootFiles.Count);
            Assert.Equals("OPS/9788711332412.opf", format.Ocf.RootFiles.ElementAt(0).FullPath);
            Assert.Equals("application/oebps-package+xml", format.Ocf.RootFiles.ElementAt(0).MediaType);
            Assert.Equals("OPS/9788711332412.opf", format.Ocf.RootFilePath);

            Assert.IsNotNull(format.Opf);
            Assert.Equals("ISBN9788711332412", format.Opf.UniqueIdentifier);
            Assert.Equals(EpubVersion.Epub3, format.Opf.EpubVersion);

            /*
            <guide>
                <reference type="cover" href="xhtml/cover.xhtml"/>
                <reference type="title-page" href="xhtml/title.xhtml"/>
                <reference type="chapter" href="xhtml/prologue.xhtml"/>
                <reference type="copyright-page" href="xhtml/copyright.xhtml"/>
            </guide>
             */
            Assert.IsNotNull(format.Opf.Guide);
            Assert.Equals(4, format.Opf.Guide.References.Count);

            Assert.Equals("xhtml/cover.xhtml", format.Opf.Guide.References.ElementAt(0).Href);
            Assert.Equals(null, format.Opf.Guide.References.ElementAt(0).Title);
            Assert.Equals("cover", format.Opf.Guide.References.ElementAt(0).Type);

            Assert.Equals("xhtml/title.xhtml", format.Opf.Guide.References.ElementAt(1).Href);
            Assert.Equals(null, format.Opf.Guide.References.ElementAt(1).Title);
            Assert.Equals("title-page", format.Opf.Guide.References.ElementAt(1).Type);

            Assert.Equals("xhtml/prologue.xhtml", format.Opf.Guide.References.ElementAt(2).Href);
            Assert.Equals(null, format.Opf.Guide.References.ElementAt(2).Title);
            Assert.Equals("chapter", format.Opf.Guide.References.ElementAt(2).Type);

            Assert.Equals("xhtml/copyright.xhtml", format.Opf.Guide.References.ElementAt(3).Href);
            Assert.Equals(null, format.Opf.Guide.References.ElementAt(3).Title);
            Assert.Equals("copyright-page", format.Opf.Guide.References.ElementAt(3).Type);
                      
            Assert.IsNotNull(format.Opf.Manifest);
            Assert.Equals(150, format.Opf.Manifest.Items.Count);

            // <item id="body097" href="xhtml/chapter_083.xhtml" media-type="application/xhtml+xml" properties="svg"/>
            var item = format.Opf.Manifest.Items.First(e => e.Id == "body097");
            Assert.Equals("xhtml/chapter_083.xhtml", item.Href);
            Assert.Equals("application/xhtml+xml", item.MediaType);
            Assert.Equals(1, item.Properties.Count);
            Assert.Equals("svg", item.Properties.ElementAt(0));
            Assert.IsNull(item.Fallback);
            Assert.IsNull(item.FallbackStyle);
            Assert.IsNull(item.RequiredModules);
            Assert.IsNull(item.RequiredNamespace);

            // <item id="css2" href="styles/big.css" media-type="text/css"/>
            item = format.Opf.Manifest.Items.First(e => e.Id == "css2");
            Assert.Equals("styles/big.css", item.Href);
            Assert.Equals("text/css", item.MediaType);
            Assert.Equals(0, item.Properties.Count);
            Assert.IsNull(item.Fallback);
            Assert.IsNull(item.FallbackStyle);
            Assert.IsNull(item.RequiredModules);
            Assert.IsNull(item.RequiredNamespace);

            /*
            <metadata xmlns:dc="http://purl.org/dc/elements/1.1/">
                <dc:title>Bogtyven</dc:title>
                <dc:creator id="creator_01">Markus Zusak</dc:creator>
                <dc:publisher>Lindhardt og Ringhof</dc:publisher>
                <dc:rights>All rights reserved Lindhardt og Ringhof Forlag A/S</dc:rights>
                <dc:identifier id="ISBN9788711332412">9788711332412</dc:identifier>
                <dc:source>urn:isbn:9788711359327</dc:source>
                <dc:language>da</dc:language>
                <dc:date>2014-04-01</dc:date>
                <meta refines="#creator_01" property="role">aut</meta>
                <meta refines="#creator_01" property="file-as">Zusak, Markus</meta>
                <meta property="dcterms:modified">2014-03-19T02:42:00Z</meta>
                <meta property="ibooks:version">1.0.0</meta>
                <meta name="cover" content="cover-image"/>
                <meta property="rendition:layout">reflowable</meta>
                <meta property="ibooks:respect-image-size-class">img_ibooks</meta>
                <meta property="ibooks:specified-fonts">true</meta>
            </metadata>
             */
            Assert.IsNotNull(format.Opf.Metadata);
            Assert.Equals(0, format.Opf.Metadata.Contributors.Count);
            Assert.Equals(0, format.Opf.Metadata.Coverages.Count);
            Assert.Equals(1, format.Opf.Metadata.Creators.Count);
            Assert.Equals("Markus Zusak", format.Opf.Metadata.Creators.ElementAt(0).Text);
            Assert.Equals(1, format.Opf.Metadata.Dates.Count);
            Assert.Equals("2014-04-01", format.Opf.Metadata.Dates.ElementAt(0).Text);
            Assert.Equals(0, format.Opf.Metadata.Descriptions.Count);
            Assert.Equals(0, format.Opf.Metadata.Formats.Count);

            Assert.Equals(1, format.Opf.Metadata.Identifiers.Count);
            Assert.Equals("9788711332412", format.Opf.Metadata.Identifiers.ElementAt(0).Text);
            Assert.Equals("ISBN9788711332412", format.Opf.Metadata.Identifiers.ElementAt(0).Id);

            Assert.Equals(1, format.Opf.Metadata.Languages.Count);
            Assert.Equals("da", format.Opf.Metadata.Languages.ElementAt(0));

            Assert.Equals(8, format.Opf.Metadata.Metas.Count);
            Assert.IsTrue(format.Opf.Metadata.Metas.All(e => e.Id == null));
            Assert.IsTrue(format.Opf.Metadata.Metas.All(e => e.Scheme == null));

            var meta = format.Opf.Metadata.Metas.Single(e => e.Property == "dcterms:modified");
            Assert.Equals("2014-03-19T02:42:00Z", meta.Text);
            Assert.IsNull(meta.Id);
            Assert.IsNull(meta.Name);
            Assert.IsNull(meta.Refines);
            Assert.IsNull(meta.Scheme);

            meta = format.Opf.Metadata.Metas.Single(e => e.Property == "ibooks:version");
            Assert.Equals("1.0.0", meta.Text);
            Assert.IsNull(meta.Id);
            Assert.IsNull(meta.Name);
            Assert.IsNull(meta.Refines);
            Assert.IsNull(meta.Scheme);

            meta = format.Opf.Metadata.Metas.Single(e => e.Property == "rendition:layout");
            Assert.Equals("reflowable", meta.Text);
            Assert.IsNull(meta.Id);
            Assert.IsNull(meta.Name);
            Assert.IsNull(meta.Refines);
            Assert.IsNull(meta.Scheme);

            meta = format.Opf.Metadata.Metas.Single(e => e.Property == "ibooks:respect-image-size-class");
            Assert.Equals("img_ibooks", meta.Text);
            Assert.IsNull(meta.Id);
            Assert.IsNull(meta.Name);
            Assert.IsNull(meta.Refines);
            Assert.IsNull(meta.Scheme);

            meta = format.Opf.Metadata.Metas.Single(e => e.Property == "ibooks:specified-fonts");
            Assert.Equals("true", meta.Text);
            Assert.IsNull(meta.Id);
            Assert.IsNull(meta.Name);
            Assert.IsNull(meta.Refines);
            Assert.IsNull(meta.Scheme);

            meta = format.Opf.Metadata.Metas.Single(e => e.Property == "role");
            Assert.Equals("aut", meta.Text);
            Assert.Equals("#creator_01", meta.Refines);
            Assert.IsNull(meta.Id);
            Assert.IsNull(meta.Name);
            Assert.IsNull(meta.Scheme);

            meta = format.Opf.Metadata.Metas.Single(e => e.Property == "file-as");
            Assert.Equals("Zusak, Markus", meta.Text);
            Assert.Equals("#creator_01", meta.Refines);
            Assert.IsNull(meta.Id);
            Assert.IsNull(meta.Name);
            Assert.IsNull(meta.Scheme);

            Assert.Equals(1, format.Opf.Metadata.Publishers.Count);
            Assert.Equals("Lindhardt og Ringhof", format.Opf.Metadata.Publishers.ElementAt(0));

            Assert.Equals(0, format.Opf.Metadata.Relations.Count);

            Assert.Equals(1, format.Opf.Metadata.Rights.Count);
            Assert.Equals("All rights reserved Lindhardt og Ringhof Forlag A/S", format.Opf.Metadata.Rights.ElementAt(0));

            Assert.Equals(1, format.Opf.Metadata.Sources.Count);
            Assert.Equals("urn:isbn:9788711359327", format.Opf.Metadata.Sources.ElementAt(0));

            Assert.Equals(0, format.Opf.Metadata.Subjects.Count);
            Assert.Equals(0, format.Opf.Metadata.Types.Count);

            Assert.Equals(1, format.Opf.Metadata.Titles.Count);
            Assert.Equals("Bogtyven", format.Opf.Metadata.Titles.ElementAt(0));

            Assert.Equals(1, format.Opf.Metadata.Identifiers.Count);
            Assert.IsNull(format.Opf.Metadata.Identifiers.ElementAt(0).Scheme);
            Assert.Equals("ISBN9788711332412", format.Opf.Metadata.Identifiers.ElementAt(0).Id);
            Assert.Equals("9788711332412", format.Opf.Metadata.Identifiers.ElementAt(0).Text);

            Assert.Equals("ncx", format.Opf.Spine.Toc);
            Assert.Equals(108, format.Opf.Spine.ItemRefs.Count);
            Assert.Equals(6, format.Opf.Spine.ItemRefs.Count(e => e.Properties.Contains("page-spread-right")));
            Assert.Equals(1, format.Opf.Spine.ItemRefs.Count(e => e.IdRef == "body044_01"));

            Assert.IsNull(format.Ncx.DocAuthor);
            Assert.Equals("Bogtyven", format.Ncx.DocTitle);

            /*
            <head>
                <meta name="dtb:uid" content="9788711332412"/>
                <meta name="dtb:depth" content="1"/>
                <meta name="dtb:totalPageCount" content="568"/>
            </head>
             */
            Assert.Equals(3, format.Ncx.Meta.Count);

            Assert.Equals("dtb:uid", format.Ncx.Meta.ElementAt(0).Name);
            Assert.Equals("9788711332412", format.Ncx.Meta.ElementAt(0).Content);
            Assert.IsNull(format.Ncx.Meta.ElementAt(0).Scheme);

            Assert.Equals("dtb:depth", format.Ncx.Meta.ElementAt(1).Name);
            Assert.Equals("1", format.Ncx.Meta.ElementAt(1).Content);
            Assert.IsNull(format.Ncx.Meta.ElementAt(1).Scheme);

            Assert.Equals("dtb:totalPageCount", format.Ncx.Meta.ElementAt(2).Name);
            Assert.Equals("568", format.Ncx.Meta.ElementAt(2).Content);
            Assert.IsNull(format.Ncx.Meta.ElementAt(2).Scheme);

            Assert.IsNull(format.Ncx.NavList);
            Assert.IsNull(format.Ncx.PageList);

            Assert.IsNotNull(format.Ncx.NavMap);
            
            Assert.IsNotNull(format.Ncx.NavMap.Dom);
            Assert.Equals(111, format.Ncx.NavMap.NavPoints.Count);
            foreach (var point in format.Ncx.NavMap.NavPoints)
            {
                Assert.IsNotNull(point.Id);
                Assert.IsNotNull(point.PlayOrder);
                Assert.IsNotNull(point.ContentSrc);
                Assert.IsNotNull(point.NavLabelText);
                Assert.IsNull(point.Class);
                Assert.IsFalse(point.NavPoints.Any());
            }

            // <navPoint id="navPoint-38" playOrder="38"><navLabel><text>– Rosas vrede</text></navLabel><content src="chapter_032.xhtml"/></navPoint>
            var navPoint = format.Ncx.NavMap.NavPoints.Single(e => e.Id == "navPoint-38");
            Assert.Equals(38, navPoint.PlayOrder);
            Assert.Equals("– Rosas vrede", navPoint.NavLabelText);
            Assert.Equals("chapter_032.xhtml", navPoint.ContentSrc);

            Assert.Equals("Bogtyven", format.Nav.Head.Title);

            /*
                <link rel="stylesheet" href="../styles/general.css" type="text/css"/>
                <link rel="stylesheet" media="(min-width:550px) and (orientation:portrait)" href="../styles/big.css" type="text/css"/>
             */
            Assert.Equals(2, format.Nav.Head.Links.Count);

            Assert.Equals(null, format.Nav.Head.Links.ElementAt(0).Class);
            Assert.Equals(null, format.Nav.Head.Links.ElementAt(0).Title);
            Assert.Equals("../styles/general.css", format.Nav.Head.Links.ElementAt(0).Href);
            Assert.Equals("stylesheet", format.Nav.Head.Links.ElementAt(0).Rel);
            Assert.Equals("text/css", format.Nav.Head.Links.ElementAt(0).Type);
            Assert.Equals(null, format.Nav.Head.Links.ElementAt(0).Media);

            Assert.Equals(null, format.Nav.Head.Links.ElementAt(1).Class);
            Assert.Equals(null, format.Nav.Head.Links.ElementAt(1).Title);
            Assert.Equals("../styles/big.css", format.Nav.Head.Links.ElementAt(1).Href);
            Assert.Equals("stylesheet", format.Nav.Head.Links.ElementAt(1).Rel);
            Assert.Equals("text/css", format.Nav.Head.Links.ElementAt(1).Type);
            Assert.Equals("(min-width:550px) and (orientation:portrait)", format.Nav.Head.Links.ElementAt(1).Media);

            Assert.Equals(1, format.Nav.Head.Metas.Count);
            Assert.Equals("utf-8", format.Nav.Head.Metas.ElementAt(0).Charset);
            Assert.IsNull(format.Nav.Head.Metas.ElementAt(0).Name);
            Assert.IsNull(format.Nav.Head.Metas.ElementAt(0).Content);

            Assert.IsNotNull(format.Nav.Body);

            /*
             <nav epub:type="toc" id="toc"></nav>
             <nav epub:type="landmarks" class="hide"></nav>
             <nav epub:type="page-list" class="hide"></nav>
             */
            Assert.Equals(3, format.Nav.Body.Navs.Count);

            Assert.IsNotNull(format.Nav.Body.Navs.ElementAt(0).Dom);
            Assert.Equals("toc", format.Nav.Body.Navs.ElementAt(0).Type);
            Assert.Equals("toc", format.Nav.Body.Navs.ElementAt(0).Id);
            Assert.IsNull(format.Nav.Body.Navs.ElementAt(0).Class);
            Assert.IsNull(format.Nav.Body.Navs.ElementAt(0).Hidden);

            Assert.IsNotNull(format.Nav.Body.Navs.ElementAt(1).Dom);
            Assert.Equals("landmarks", format.Nav.Body.Navs.ElementAt(1).Type);
            Assert.Equals("hide", format.Nav.Body.Navs.ElementAt(1).Class);
            Assert.IsNull(format.Nav.Body.Navs.ElementAt(1).Id);
            Assert.IsNull(format.Nav.Body.Navs.ElementAt(1).Hidden);

            Assert.IsNotNull(format.Nav.Body.Navs.ElementAt(2).Dom);
            Assert.Equals("page-list", format.Nav.Body.Navs.ElementAt(2).Type);
            Assert.Equals("hide", format.Nav.Body.Navs.ElementAt(2).Class);
            Assert.IsNull(format.Nav.Body.Navs.ElementAt(2).Id);
            Assert.IsNull(format.Nav.Body.Navs.ElementAt(2).Hidden);
        }
    }
}
