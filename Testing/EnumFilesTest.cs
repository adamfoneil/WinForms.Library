using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using WinForms.Library;
using WinForms.Library.Models;

namespace Testing
{
    [TestClass]
    public class EnumFilesTest
    {
        [TestMethod]
        public void EnumRepoFolder()
        {
            string folder = @"C:\Users\Adam\Source\Repos";
            List<ListItem<string>> results = new List<ListItem<string>>();
            FileSystem.EnumFiles(folder, "*.sln", fileFound: (fi) =>
            {
                results.Add(new ListItem<string>(fi.FullName, fi.FullName.Substring(folder.Length + 1)));
                return EnumFileResult.NextFolder;
            });
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void FindDirectories()
        {
            //var results = FileSystem.FindFolders(@"C:\Users\Adam\SkyDrive", "ferry");

            var results = FileSystem.FindFolders(@"C:\Users\Adam\Source", "license");
        }
    }
}
