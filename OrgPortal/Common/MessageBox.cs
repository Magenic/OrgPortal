using System;
using System.Composition;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace OrgPortal.Common
{
    [Export(typeof(IMessageBox))]
    public sealed class MessageBox : IMessageBox
    {
        public async Task<bool?> ShowAsync(string content, MessageBoxButtons messageBoxButtons = MessageBoxButtons.Ok)
        {
            var messageDialog = new MessageDialog(content);
            bool? result = null;
            MessageBox.AddButtons(messageDialog, messageBoxButtons,
                new UICommandInvokedHandler((cmd) => result = true),
                new UICommandInvokedHandler((cmd) => result = false));

            await messageDialog.ShowAsync();

            return result;
        }

        public async Task<bool?> ShowAsync(string content, string title, MessageBoxButtons messageBoxButtons = MessageBoxButtons.Ok)
        {
            var messageDialog = new MessageDialog(content, title);
            bool? result = null;
            MessageBox.AddButtons(messageDialog, messageBoxButtons,
                new UICommandInvokedHandler((cmd) => result = true),
                new UICommandInvokedHandler((cmd) => result = false));

            await messageDialog.ShowAsync();

            return result;
        }

        private static void AddButtons(
            MessageDialog messageDialog,
            MessageBoxButtons messageBoxButtons,
            UICommandInvokedHandler positiveHandler,
            UICommandInvokedHandler negativeHandler)
        {
            if ((messageBoxButtons & MessageBoxButtons.Ok) == MessageBoxButtons.Ok)
            {
                messageDialog.Commands.Add(new UICommand("OK", positiveHandler));
            }

            if ((messageBoxButtons & MessageBoxButtons.Cancel) == MessageBoxButtons.Cancel)
            {
                messageDialog.Commands.Add(new UICommand("Cancel", negativeHandler));
            }

            if ((messageBoxButtons & MessageBoxButtons.Yes) == MessageBoxButtons.Yes)
            {
                messageDialog.Commands.Add(new UICommand("Yes", positiveHandler));
            }

            if ((messageBoxButtons & MessageBoxButtons.No) == MessageBoxButtons.No)
            {
                messageDialog.Commands.Add(new UICommand("No", negativeHandler));
            }
        }
    }
}