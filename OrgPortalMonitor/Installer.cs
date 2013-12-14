using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortalMonitor
{
  public class Installer
  {
    public string TempPath { get; set; }
    public string LocalPath { get; set; }
    public System.Windows.Forms.NotifyIcon NotifyIcon { get; set; }
    public System.Windows.Forms.TextBox Output { get; set; }
    public FileSystemWatcher Watcher { get; set; }

    public Installer(System.Windows.Forms.NotifyIcon notifyIcon, System.Windows.Forms.TextBox output, FileSystemWatcher watcher)
    {
      this.NotifyIcon = notifyIcon;
      this.Output = output;
      this.Watcher = watcher;

      this.Output.AppendText("Monitor started at " + DateTime.Now + Environment.NewLine);
      this.NotifyIcon.ShowBalloonTip(500, "OrgPortal", "The OrgPortal monitor has started", System.Windows.Forms.ToolTipIcon.None);

      var tempPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
      if (!tempPath.EndsWith(@"\"))
        tempPath += @"\";
      tempPath += @"Packages\OrgPortal_m64ba5zfsemg0\TempState\";
      if (!System.IO.Directory.Exists(tempPath))
        System.IO.Directory.CreateDirectory(tempPath);
      this.TempPath = tempPath;

      var localPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
      if (!localPath.EndsWith(@"\"))
        localPath += @"\";
      localPath += @"Packages\OrgPortal_m64ba5zfsemg0\LocalState\";
      if (!System.IO.Directory.Exists(localPath))
        System.IO.Directory.CreateDirectory(localPath);
      this.LocalPath = localPath;

      this.Watcher.Path = this.TempPath;
    }

    public void StartFileWatcher()
    {
      this.Output.AppendText("Watching folder " + this.TempPath + Environment.NewLine);
      Watcher.Created += (s, e) =>
        {
          System.Threading.Thread.Sleep(500);
          ProcessRequest(e.FullPath);
        };

      var existingFiles = Directory.EnumerateFiles(this.TempPath, "*.req");
      foreach (var item in existingFiles)
        ProcessRequest(item);
    }

    private void ProcessRequest(string inputFilePath)
    {
      var input = File.ReadAllLines(inputFilePath);
      var appxurl = input[0];

      var uriSegments = new System.Uri(appxurl).Segments;
      var fileName = uriSegments[uriSegments.Length - 1];
      var filePath = TempPath + fileName;

      this.Output.AppendText("Installing " + fileName + Environment.NewLine);
      this.Output.AppendText("  from " + appxurl + Environment.NewLine);
      this.Output.AppendText("  at " + DateTime.Now + Environment.NewLine);

      var result = new InstallResult();
      result.Error = DownloadAppx(appxurl, filePath);
      if (string.IsNullOrWhiteSpace(result.Error))
        result = InstallAppx(filePath);

      var logfilePath = inputFilePath.Replace(".req", ".log");
      if (string.IsNullOrWhiteSpace(result.Error))
      {
        File.WriteAllText(logfilePath, "SUCCESS");
        NotifyIcon.ShowBalloonTip(500, "OrgPortal", "App installed", System.Windows.Forms.ToolTipIcon.Info);
        this.Output.AppendText("  SUCCESS" + Environment.NewLine);
      }
      else
      {
        File.WriteAllText(logfilePath, result.ToString());
        NotifyIcon.ShowBalloonTip(500, "OrgPortal", "App not installed", System.Windows.Forms.ToolTipIcon.Warning);
        this.Output.AppendText("  FAILED" + Environment.NewLine);
        this.Output.AppendText(result.ToString() + Environment.NewLine);
      }
      this.Output.AppendText("===================" + Environment.NewLine);

      File.Delete(inputFilePath);

      GetInstalledPackages();
    }

    public InstallResult InstallAppx(string filepath)
    {
      var result = new InstallResult();

      try
      {
        var sb = new StringBuilder();
        sb.Append(@"add-appxpackage ");
        sb.Append(filepath);

        var process = new System.Diagnostics.Process();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.RedirectStandardOutput = true;

        process.StartInfo.FileName = "powershell.exe";
        process.StartInfo.Arguments = sb.ToString();

        process.StartInfo.CreateNoWindow = false;
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        process.Start();

        var stdout = process.StandardOutput;
        var stderr = process.StandardError;

        result.Output = stdout.ReadToEnd();
        result.Error = stderr.ReadToEnd();

        if (!process.HasExited)
          process.Kill();

        stdout.Close();
        stderr.Close();
      }
      catch (Exception ex)
      {
        if (string.IsNullOrWhiteSpace(result.Error))
          result.Error = ex.Message;
        else
          result.Error += Environment.NewLine + ex.Message;
      }
      finally
      {
        File.Delete(filepath);
      }

      return result;
    }

    public void GetInstalledPackages()
    {
      var sb = new StringBuilder();
      sb.Append(@"get-appxpackage > " + this.LocalPath + "InstalledPackages.txt");

      var process = new System.Diagnostics.Process();
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.RedirectStandardError = true;
      process.StartInfo.RedirectStandardOutput = true;

      process.StartInfo.FileName = "powershell.exe";
      process.StartInfo.Arguments = sb.ToString();

      process.StartInfo.CreateNoWindow = false;
      process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

      process.Start();

      if (!process.HasExited)
        process.Kill();
    }

    public string DownloadAppx(string fileUrl, string localFilePath)
    {
      try
      {
        using (var client = new WebClient())
        {
          client.DownloadFile(fileUrl, localFilePath);
        }
      }
      catch (Exception ex)
      {
        var sb = new StringBuilder();
        sb.AppendLine("Download of " + fileUrl + " failed.");
        sb.AppendLine(ex.Message);
        return sb.ToString();
      }
      return null;
    }
  }
}
