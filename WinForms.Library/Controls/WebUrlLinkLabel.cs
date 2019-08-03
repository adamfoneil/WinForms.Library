using System.Diagnostics;
using System.Windows.Forms;

namespace WinForms.Library.Controls
{
    public class WebUrlLinkLabel : LinkLabel
    {
        public string Url { get; set; }

        protected override void OnLinkClicked(LinkLabelLinkClickedEventArgs e)
        {
            base.OnLinkClicked(e);
            Process.Start(Url);
        }
    }
}