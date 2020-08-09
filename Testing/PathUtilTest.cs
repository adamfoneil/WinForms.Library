using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WinForms.Library;

namespace Testing
{
    [TestClass]
    public class PathUtilTest
    {
        [TestMethod]
        public void GetCommonBasePathSimpleCase()
        {
            var files = new string[]
            {
                @"c:\this\that\other\alpha.jpg",
                @"c:\this\that\other\bingo.jpg",
                @"c:\this\that\other\whatever.txt",
                @"c:\this\that\jonga\hellcat.png"
            };

            var basePath = PathUtil.GetCommonPath(files, '\\');
            Assert.IsTrue(basePath.Equals(@"c:\this\that"));
        }

        [TestMethod]
        public void GetCommonBasePathParentDirectory()
        {
            /*
            when all files are in the same folder, then we back up a level
            */
            var files = new string[]
            {
                @"c:\this\that\other\alpha.jpg",
                @"c:\this\that\other\bingo.jpg",
                @"c:\this\that\other\whatever.txt",
                @"c:\this\that\other\hellcat.png"
            };

            var basePath = PathUtil.GetCommonPath(files, '\\', true);
            Assert.IsTrue(basePath.Equals(@"c:\this\that"));
        }

        [TestMethod]
        public void UniquifyFilesTest()
        {
            var fileNames = new string[]
            {
                "this/that/other/hello.txt",
                "this/that/whatever.jpg",
                "this/that/friday/data.json",
                "this/begin/whatever.jpg"
            };

            var unique = PathUtil.UniquifyFiles(fileNames, '/');

            Assert.IsTrue(unique.Select(kp => kp).SequenceEqual(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("hello.txt", "this/that/other/hello.txt"),
                new KeyValuePair<string, string>("that/whatever.jpg", "this/that/whatever.jpg"),
                new KeyValuePair<string, string>("data.json", "this/that/friday/data.json"),
                new KeyValuePair<string, string>("begin/whatever.jpg", "this/begin/whatever.jpg")
            }));

            fileNames = new string[]
            {
                "this/that/other/hello.txt",
                "this/that/other/goodbye.txt",
                "this/that/other/enchanted.txt"
            };

            unique = PathUtil.UniquifyFiles(fileNames);

            Assert.IsTrue(unique.Select(kp => kp).SequenceEqual(new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("hello.txt", "this/that/other/hello.txt"),
                new KeyValuePair<string, string>("goodbye.txt", "this/that/other/goodbye.txt"),
                new KeyValuePair<string, string>("enchanted.txt", "this/that/other/enchanted.txt"),
            }));
        }
    }
}
