[![Nuget](https://img.shields.io/nuget/v/WinForms.Library?label=WinForms.Library)](https://www.nuget.org/packages/WinForms.Library/) [![Nuget](https://img.shields.io/nuget/v/AO.FileSystem?label=AO.FileSystem)](https://www.nuget.org/packages/AO.FileSystem/)

This is a collection of helpers and data binding utillities for WinForms development. Please see the [Wiki](https://github.com/adamosoftware/WinForms.Library/wiki) for latest info.

There's also a separate Nuget package **AO.FileSystem** that removes the WinForms dependency, and provides several file search methods:

# WinForms.Library.FileSystem [FileSystem_DotNetSearch.cs](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs)
## Methods
- void [EnumFiles](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L28)
 (string path, string searchPattern, Func<FileInfo, EnumFileResult> fileFound)
- void [EnumDirectories](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L50)
 (string path, [ Func<DirectoryInfo, EnumFileResult> starting ], [ Func<DirectoryInfo, EnumFileResult> ending ])
- Task\<IEnumerable\<FileInfo\>\> [FindFilesAsync](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L69)
 (string path, string searchPattern, [ Func<FileInfo, bool> filter ])
- IEnumerable\<string\> [FindFolders](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L113)
 (string rootPath, string query, [ int minQueryLength ], [ int maxResults ], [ IProgress<string> progress ], [ CancellationTokenSource cancellationTokenSource ])
- Task\<IEnumerable\<string\>\> [FindFoldersAsync](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L150)
 (string rootPath, string query, [ int minQueryLength ], [ int maxResults ], [ IProgress<string> progress ], [ CancellationTokenSource cancellationTokenSource ])


# WinForms.Library.PathUtil [PathUtil.cs](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/PathUtil.cs)
## Methods
- string [GetCommonPath](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/PathUtil.cs#L16)
 (IEnumerable<string> fileNames, [ char separator ], [ bool samePathReturnsParent ])
- Dictionary\<string, string\> [UniquifyFiles](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/PathUtil.cs#L52)
 (IEnumerable<string> fileNames)
- string [GetParentFolder](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/PathUtil.cs#L90)
 (string path)
- string [ReplaceExtension](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/PathUtil.cs#L97)
 (string fileName, string newExtension)
- string [EnvironmentPath](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/PathUtil.cs#L109)
 (SpecialFolder specialFolder, params string[] parts)
- string [GetFilenameAtDepth](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/PathUtil.cs#L126)
 ()
