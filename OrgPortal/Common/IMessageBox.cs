using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortal.Common
{
    [Flags]
    public enum MessageBoxButtons
    {
        Ok = 1,
        Cancel = 2,
        OkCancel = Ok | Cancel,
        Yes = 4,
        No = 8,
        YesNo = Yes | No
    }

    public interface IMessageBox
    {
        Task<bool?> ShowAsync(string content, MessageBoxButtons messageBoxButtons = MessageBoxButtons.Ok);
        Task<bool?> ShowAsync(string content, string title, MessageBoxButtons messageBoxButtons = MessageBoxButtons.Ok);
    }
}