namespace OrgPortalMonitor
{
  partial class UnlockKeyDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.label1 = new System.Windows.Forms.Label();
      this.KeyValueInput = new System.Windows.Forms.TextBox();
      this.OkBtn = new System.Windows.Forms.Button();
      this.CancelBtn = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(146, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Enter side-loading unlock key";
      // 
      // KeyValueInput
      // 
      this.KeyValueInput.Location = new System.Drawing.Point(15, 25);
      this.KeyValueInput.Name = "KeyValueInput";
      this.KeyValueInput.Size = new System.Drawing.Size(578, 20);
      this.KeyValueInput.TabIndex = 1;
      // 
      // OkBtn
      // 
      this.OkBtn.Location = new System.Drawing.Point(437, 51);
      this.OkBtn.Name = "OkBtn";
      this.OkBtn.Size = new System.Drawing.Size(75, 23);
      this.OkBtn.TabIndex = 2;
      this.OkBtn.Text = "&OK";
      this.OkBtn.UseVisualStyleBackColor = true;
      this.OkBtn.Click += new System.EventHandler(this.OkButton_Click);
      // 
      // CancelBtn
      // 
      this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.CancelBtn.Location = new System.Drawing.Point(518, 51);
      this.CancelBtn.Name = "CancelBtn";
      this.CancelBtn.Size = new System.Drawing.Size(75, 23);
      this.CancelBtn.TabIndex = 3;
      this.CancelBtn.Text = "&Cancel";
      this.CancelBtn.UseVisualStyleBackColor = true;
      this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
      // 
      // UnlockKeyDialog
      // 
      this.AcceptButton = this.OkBtn;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(605, 87);
      this.Controls.Add(this.CancelBtn);
      this.Controls.Add(this.OkBtn);
      this.Controls.Add(this.KeyValueInput);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "UnlockKeyDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Unlock Key";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox KeyValueInput;
    private System.Windows.Forms.Button OkBtn;
    private System.Windows.Forms.Button CancelBtn;
  }
}