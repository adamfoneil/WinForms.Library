using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
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

            var results = FileSystem.FindFolders(@"C:\Users\Adam\OneDrive", "ferry");

            //var results = FileSystem.FindFolders(@"C:\Users\Adam\Source", "license");
        }

        [TestMethod]
        public void EnumDirectories()
        {
            FileSystem.EnumDirectories(@"C:\Users\Adam\SkyDrive\Pictures", (di) =>
            {
                Debug.Print($"going down: {di.FullName}");
                return EnumFileResult.Continue;
            }, (di) =>
            {
                Debug.Print($"going up: {di.FullName}");
                return EnumFileResult.Continue;
            });
        }

        [TestMethod]
        public void FindNearestFiles()
        {
            var ratings = new List<string>();
            FileSystem.EnumFiles(@"C:\Users\Adam\SkyDrive\Pictures", "ratings.json", (fi) =>
            {
                ratings.Add(fi.FullName);
                return EnumFileResult.NextFolder;
            });
            Assert.IsTrue(ratings.Any());
        }
    }
}
