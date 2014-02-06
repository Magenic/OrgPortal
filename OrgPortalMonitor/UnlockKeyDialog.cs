using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrgPortalMonitor
{
  public partial class UnlockKeyDialog : Form
  {
    public UnlockKeyDialog()
    {
      InitializeComponent();
    }

    public string KeyValue { get; private set; }

    private void OkButton_Click(object sender, EventArgs e)
    {
      KeyValue = KeyValueInput.Text;
      this.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.Close();
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
      this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.Close();
    }
  }
}
