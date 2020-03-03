using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
