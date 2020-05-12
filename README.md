[![Nuget](https://img.shields.io/nuget/v/WinForms.Library?label=WinForms.Library)](https://www.nuget.org/packages/WinForms.Library/) [![Nuget](https://img.shields.io/nuget/v/AO.FileSystem?label=AO.FileSystem)](https://www.nuget.org/packages/AO.FileSystem/)

This is a collection of helpers and data binding utillities for WinForms development. Please see the [Wiki](https://github.com/adamosoftware/WinForms.Library/wiki) for latest info.

There's also a separate Nuget package **AO.FileSystem** that removes the WinForms dependency, and provides a couple methods for working with local file system:

# WinForms.Library.FileSystem [FileSystem_DotNetSearch.cs](https://github.com/adamosoftware/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs)
## Methods
- void [EnumFiles](https://github.com/adamosoftware/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L28)
 (string path, string searchPattern, [ Func<DirectoryInfo, EnumFileResult> directoryFound ], [ Func<FileInfo, EnumFileResult> fileFound ])
- Task\<IEnumerable\<FileInfo\>\> [FindFilesAsync](https://github.com/adamosoftware/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L68)
 (string path, string searchPattern, [ Func<FileInfo, bool> filter ])
- IEnumerable\<string\> [FindFolders](https://github.com/adamosoftware/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L112)
 (string rootPath, string query, [ int minQueryLength ], [ int maxResults ], [ IProgress<string> progress ], [ CancellationTokenSource cancellationTokenSource ])
- Task\<IEnumerable\<string\>\> [FindFoldersAsync](https://github.com/adamosoftware/WinForms.Library/blob/master/WinForms.Library/FileSystem_DotNetSearch.cs#L149)
 (string rootPath, string query, [ int minQueryLength ], [ int maxResults ], [ IProgress<string> progress ], [ CancellationTokenSource cancellationTokenSource ])
