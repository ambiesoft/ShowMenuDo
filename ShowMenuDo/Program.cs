using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShowMenuDo
{
    class MenuText
    {
         public string Text { get; set; }
        public string FirstLetter { get; set; }
        public MenuText(string text, string firstLetter)
        {
            Text = text;
            FirstLetter = firstLetter;
        }
    }
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length > 0 &&
                ((args[0] == "--version") ||
                (args[0] == "-v")))
            {
                MessageBox.Show("ShowMenuDo ver1.0.0",
                    Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MenuText[] menuTexts = { 
                new MenuText("フライト", "ふ"),
                new MenuText("空港駐車場", "く"),
                new MenuText("花の配達オプション", "は"),
                new MenuText("ストリーミングプラットフォーム", "す"),
                new MenuText("クレジットレポート", "く"),
                new MenuText("レシピ", "れ"),
                new MenuText("Elden Ring", "え"),
            };
            // sort menuTexts by FirstLetter
            Array.Sort(menuTexts, (x, y) => string.Compare(x.FirstLetter, y.FirstLetter, StringComparison.Ordinal));

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Create context menu and items
            var cxtMenu = new ContextMenuStrip();

            for(int i = 0; i < menuTexts.Length; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(menuTexts[i].Text);
                item.Tag = menuTexts[i];
                item.Click += CopyItem_Click;
                cxtMenu.Items.Add(item);
            }
            // Create a tiny invisible owner form so the context menu properly closes when clicking outside.
            using var ownerForm = new Form
            {
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                FormBorderStyle = FormBorderStyle.None,
                Opacity = 0,
                Size = new Size(1, 1),
                // Place the form at the cursor so PointToClient conversion is straightforward.
                Location = new Point(Cursor.Position.X, Cursor.Position.Y)
            };

            // When the context menu closes, close the owner form so the message loop exits.
            cxtMenu.Closed += (s, e) =>
            {
                // Ensure the form is closed on the UI thread.
                if (!ownerForm.IsDisposed && ownerForm.Visible)
                {
                    ownerForm.BeginInvoke((Action)(() => ownerForm.Close()));
                }
            };

            // Show the owner form (invisible) and then show the context menu relative to it.
            ownerForm.Show();
            Point clientPoint = ownerForm.PointToClient(Cursor.Position);
            cxtMenu.Show(ownerForm, clientPoint, ToolStripDropDownDirection.AboveRight);

            // Start a message loop tied to the owner form so closing the form ends the app.
            Application.Run(ownerForm);
        }

        private static void CopyItem_Click(object? sender, EventArgs e)
        {
            try
            {
                // Example action: place a sample text on the clipboard.
                if (sender is ToolStripMenuItem item && item.Tag is MenuText menuText)
                {
                    Clipboard.SetText(menuText.Text);
                }
            }
            catch
            {
                // Ignore clipboard failures silently.
            }
        }
    }
}