using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
