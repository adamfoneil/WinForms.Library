using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinForms.Library
{
	public static partial class FileSystem
	{
		public enum IconSize
		{
			Small = 1,
			Large = 0
		}

		[DllImport("shell32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct SHFILEINFO
		{
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		};

		[Flags]
		private enum SHGFI : int
		{
			/// <summary>get icon</summary>
			Icon = 0x000000100,

			/// <summary>get display name</summary>
			DisplayName = 0x000000200,

			/// <summary>get type name</summary>
			TypeName = 0x000000400,

			/// <summary>get attributes</summary>
			Attributes = 0x000000800,

			/// <summary>get icon location</summary>
			IconLocation = 0x000001000,

			/// <summary>return exe type</summary>
			ExeType = 0x000002000,

			/// <summary>get system icon index</summary>
			SysIconIndex = 0x000004000,

			/// <summary>put a link overlay on icon</summary>
			LinkOverlay = 0x000008000,

			/// <summary>show icon in selected state</summary>
			Selected = 0x000010000,

			/// <summary>get only specified attributes</summary>
			Attr_Specified = 0x000020000,

			/// <summary>get large icon</summary>
			LargeIcon = 0x000000000,

			/// <summary>get small icon</summary>
			SmallIcon = 0x000000001,

			/// <summary>get open icon</summary>
			OpenIcon = 0x000000002,

			/// <summary>get shell size icon</summary>
			ShellIconSize = 0x000000004,

			/// <summary>pszPath is a pidl</summary>
			PIDL = 0x000000008,

			/// <summary>use passed dwFileAttribute</summary>
			UseFileAttributes = 0x000000010,

			/// <summary>apply the appropriate overlays</summary>
			AddOverlays = 0x000000020,

			/// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
			OverlayIndex = 0x000000040,
		}

		public static string AddIcon(ImageList imageList, string path, IconSize size)
		{
			string ext = Path.GetExtension(path);
			if (imageList.Images.ContainsKey(ext)) return ext;

			var icon = GetIcon(path, size);
			imageList.Images.Add(ext, icon);
			return ext;
		}

        public static Bitmap GetIcon(string path, IconSize size, out string typeName)
        {
            // help from https://pontusmunck.com/2007/02/01/preserving-transparency-when-converting-icon-to-bitmap/
            // and http://www.pinvoke.net/default.aspx/shell32.SHGetFileInfo

            SHFILEINFO info = new SHFILEINFO();
            int cbFileInfo = Marshal.SizeOf(info);
            SHGFI flags = SHGFI.Icon | (SHGFI)size | SHGFI.UseFileAttributes | SHGFI.TypeName;
            SHGetFileInfo(path, 256, ref info, (uint)cbFileInfo, (uint)flags);
            typeName = info.szTypeName;
            using (Icon ico = Icon.FromHandle(info.hIcon))
            {
                Bitmap bmp = new Bitmap(ico.Size.Width, ico.Size.Height);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawIcon(ico, 0, 0);
                }
                return bmp;
            }
        }


        public static Bitmap GetIcon(string path, IconSize size)
		{
            return GetIcon(path, size, out string typeName);
		}
         
        public static string GetFileType(string path)
        {
            GetIcon(path, IconSize.Small, out string typeName);
            return typeName;
        }
	}
}