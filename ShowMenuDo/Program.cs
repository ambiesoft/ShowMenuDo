using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShowMenuDo
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] menuTexts = { 
                "フライト",
                "空港駐車場",
                "花の配達オプション",
            "ストリーミングプラットフォーム"};
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // Create context menu and items
            var cxtMenu = new ContextMenuStrip();

            for(int i = 0; i < menuTexts.Length; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(menuTexts[i]);
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
            cxtMenu.Show(ownerForm, clientPoint);

            // Start a message loop tied to the owner form so closing the form ends the app.
            Application.Run(ownerForm);
        }

        private static void CopyItem_Click(object? sender, EventArgs e)
        {
            try
            {
                // Example action: place a sample text on the clipboard.
                if (sender is ToolStripMenuItem item && item.Tag is string text)
                {
                    Clipboard.SetText(text);
                }
            }
            catch
            {
                // Ignore clipboard failures silently.
            }
        }
    }
}