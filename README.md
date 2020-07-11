[![Nuget](https://img.shields.io/nuget/v/WinForms.Library?label=WinForms.Library)](https://www.nuget.org/packages/WinForms.Library/) [![Nuget](https://img.shields.io/nuget/v/AO.FileSystem?label=AO.FileSystem)](https://www.nuget.org/packages/AO.FileSystem/)

This is a collection of helpers and data binding utillities for WinForms development. Please see the [Wiki](https://github.com/adamosoftware/WinForms.Library/wiki) for latest info.

There's also a separate Nuget package **AO.FileSystem** that removes the WinForms dependency, and provides several file search methods:

# WinForms.Library.FileSystem [FileSystem_DotNetSearch.cs](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L26)
## Methods
- void [EnumFiles](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L28)
 (string path, string searchPattern, Func<FileInfo, EnumFileResult> fileFound)
- void [EnumDirectories](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L50)
 (string path, [ Func<DirectoryInfo, EnumFileResult> traversingDown ], [ Func<DirectoryInfo, EnumFileResult> traversingUp ])
- Task\<IEnumerable\<FileInfo\>\> [FindFilesAsync](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L71)
 (string path, string searchPattern, [ Func<FileInfo, bool> filter ])
- IEnumerable\<string\> [FindFolders](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L115)
 (string rootPath, string query, [ int minQueryLength ], [ int maxResults ], [ IProgress<string> progress ], [ CancellationTokenSource cancellationTokenSource ])
- Task\<IEnumerable\<string\>\> [FindFoldersAsync](https://github.com/adamfoneil/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L152)
 (string rootPath, string query, [ int minQueryLength ], [ int maxResults ], [ IProgress<string> progress ], [ CancellationTokenSource cancellationTokenSource ])
