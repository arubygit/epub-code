﻿using EpubSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EpubSharp.Tests
{
    [TestClass]
    public class EpubBookTests
    {
        [TestMethod]
        public void EpubAsPlainTextTest1()
        {
            var book = EpubReader.Read(Cwd.Combine(@"Samples/epub-assorted/boothbyg3249432494-8epub.epub"));
            //File.WriteAllText(Cwd.Join("Samples/epub-assorted/boothbyg3249432494-8epub.txt", book.ToPlainText()));

            Func<string, string> normalize = text => text.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            var expected = File.ReadAllText(Cwd.Combine(@"Samples/epub-assorted/boothbyg3249432494-8epub.txt"));
            var actual = book.ToPlainText();
            Assert.Equals(normalize(expected), normalize(actual));

            var lines = actual.Split('\n').Select(str => str.Trim()).ToList();
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "I. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "II. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "III. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "IV. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "V. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "VI. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "VII. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "VIII. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "IX. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "X. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "XI. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "XII. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "XIII. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "XIV. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "XV. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "XVI. KAPITEL."));
            Assert.IsNotNull(lines.SingleOrDefault(e => e == "XVII. KAPITEL."));
        }

        [TestMethod]
        public void EpubAsPlainTextTest2()
        {
            var book = EpubReader.Read(Cwd.Combine(@"Samples/epub-assorted/iOS Hackers Handbook.epub"));
            //File.WriteAllText(Cwd.Join("Samples/epub-assorted/iOS Hackers Handbook.txt", book.ToPlainText()));

            Func<string, string> normalize = text => text.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            var expected = File.ReadAllText(Cwd.Combine(@"Samples/epub-assorted/iOS Hackers Handbook.txt"));
            var actual = book.ToPlainText();
            Assert.Equals(normalize(expected), normalize(actual));
            
            var trimmed = string.Join("\n", actual.Split('\n').Select(str => str.Trim()));
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 1\niOS Security Basics").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 2\niOS in the Enterprise").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 3\nEncryption").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 4\nCode Signing and Memory Protections").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 5\nSandboxing").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 6\nFuzzing iOS Applications").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 7\nExploitation").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 8\nReturn-Oriented Programming").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 9\nKernel Debugging and Exploitation").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 10\nJailbreaking").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "Chapter 11\nBaseband Attacks").Count);
            Assert.Equals(1, Regex.Matches(trimmed, "How This Book Is Organized").Count);
            Assert.Equals(2, Regex.Matches(trimmed, "Appendix: Resources").Count);
            Assert.Equals(2, Regex.Matches(trimmed, "Case Study: Pwn2Own 2010").Count);
        }             
    }
}
