namespace OrgPortalMonitor
{
  partial class Form1
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
      ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
      this.SuspendLayout();
      // 
      // notifyIcon1
      // 
      this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
      this.notifyIcon1.Text = "OrgPortal";
      this.notifyIcon1.Visible = true;
      this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
      this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
      // 
      // textBox1
      // 
      this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBox1.Location = new System.Drawing.Point(0, 0);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.ReadOnly = true;
      this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.textBox1.Size = new System.Drawing.Size(521, 469);
      this.textBox1.TabIndex = 0;
      // 
      // fileSystemWatcher1
      // 
      this.fileSystemWatcher1.EnableRaisingEvents = true;
      this.fileSystemWatcher1.Filter = "*.req";
      this.fileSystemWatcher1.SynchronizingObject = this;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(521, 469);
      this.Controls.Add(this.textBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "Form1";
      this.ShowInTaskbar = false;
      this.Text = "OrgPortal Monitor";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
      this.Load += new System.EventHandler(this.Form1_Load);
      ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private System.Windows.Forms.TextBox textBox1;
    private System.IO.FileSystemWatcher fileSystemWatcher1;
  }
}

