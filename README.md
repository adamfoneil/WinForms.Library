Nuget package: **WinForms.Library**

**Update 2/17/19** Originally, I didn't think I was going to create a Nuget package for this, but I changed my mind because it was getting too big. I realized I needed a general-purpose data binding solution after all for [DocumentManager](https://github.com/adamosoftware/WinForms.Library/blob/master/WinForms.Library/DocumentManager.cs). Otherwise I would have custom data binding code in several places. I adapted some stuff I'd written a good while ago in [Postulate.WinForms](https://github.com/adamosoftware/Postulate.WinForms), simplifying and removing the ORM dependency.

To see an example of the data binding in use, see [Form1](https://github.com/adamosoftware/WinForms.Library/blob/master/WinForms.SampleApp/Form1.cs#L21) from the Sample App in this repo. This is just a dummy form that demonstrates several binding features you can use with `DocumentManager`.

![img](https://adamosoftware.blob.core.windows.net:443/images/doc-manager-binding.png)

You can see it's easy to setup data binding from controls to properties of an object using lambda syntax. The implementation is [here](https://github.com/adamosoftware/WinForms.Library/blob/master/WinForms.Library/DocumentManager_Controls.cs).

---

I started this because I needed to use my [DocumentManager](https://github.com/adamosoftware/WinForms.Library/blob/master/WinForms.Library/DocumentManager.cs) class from another project in my [Delivery](https://github.com/adamosoftware/Delivery) app, and I didn't want to duplicate it. I could've made this into a Nuget package, but I'm sort of committed to moving all new package development to .NET Standard, and I don't think WinForms is setup for that, although I could be wrong. (I know WinForms is coming to .NET Core 3.0 soon, which means .NET Standard compatibility, but I don't really have time to wait for that nor inclination to get into details of that now.)

`DocumentManager` is a general-purpose save/open json single document interface handler using my [JsonSettings](https://github.com/adamosoftware/JsonSettings) project underneath. It displays file open and save dialogs, and can prompt to save a document when a form is closing -- stuff like that. It's not a data binding solution. You have to do data binding on controls separately.
