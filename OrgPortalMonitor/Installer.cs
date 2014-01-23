using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
      var logfilePath = inputFilePath.Replace(".req", ".log");
      var outputDoc = new XElement("request");
      outputDoc.Add(new XElement("requestFile", inputFilePath));
      this.Output.AppendText(">>Processing " + inputFilePath + Environment.NewLine);

      try
      {

        var input = File.ReadAllLines(inputFilePath);
        var command = input[0];
        outputDoc.Add(new XElement("command", command));

        if (command == "install")
        {
          ProcessInstallRequest(outputDoc, input[1]);
        }
        else if (command == "getDevLicense")
        {
          GetDevLicense(outputDoc);
        }
        else
        {
          outputDoc.Add(new XElement("success", "false"));
          outputDoc.Add(new XElement("error", "Invalid command"));
          this.Output.AppendText("Invalid command: " + command + Environment.NewLine);
          this.Output.AppendText("  Input file ignored" + Environment.NewLine);
          this.Output.AppendText("**FAILED" + Environment.NewLine);
        }
      }
      catch (Exception ex)
      {
        outputDoc.Add(new XElement("success", "false"));
        outputDoc.Add(new XElement("error", ex.Message));
        this.Output.AppendText("UNEXPECTED EXCEPTION" + Environment.NewLine);
        this.Output.AppendText(ex.ToString() + Environment.NewLine);
        this.Output.AppendText("**FAILED" + Environment.NewLine);
      }
      finally
      {
        File.Delete(inputFilePath);
        this.Output.AppendText("<<Processed " + inputFilePath + Environment.NewLine);
        this.Output.AppendText(Environment.NewLine);
        File.WriteAllText(logfilePath, outputDoc.ToString());
      }
    }

    private void ProcessInstallRequest(XElement outputDoc, string appxurl)
    {
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

      if (string.IsNullOrWhiteSpace(result.Error))
      {
        outputDoc.Add(new XElement("success", "true"));
        NotifyIcon.ShowBalloonTip(500, "OrgPortal", "App installed", System.Windows.Forms.ToolTipIcon.Info);
        this.Output.AppendText("**SUCCESS" + Environment.NewLine);
      }
      else
      {
        outputDoc.Add(new XElement("success", "false"));
        outputDoc.Add(new XElement("error", result.ToString()));
        NotifyIcon.ShowBalloonTip(500, "OrgPortal", "App not installed", System.Windows.Forms.ToolTipIcon.Warning);
        this.Output.AppendText("**FAILED" + Environment.NewLine);
        this.Output.AppendText(result.ToString() + Environment.NewLine);
      }

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

      while (!process.HasExited)
        System.Threading.Thread.Sleep(5);
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

    public void GetDevLicense()
    {
      GetDevLicense(new XElement("request"));
    }
    public void GetDevLicense(XElement outputDoc)
    {
      var sb = new StringBuilder();
      sb.Append(@"Show-WindowsDeveloperLicenseRegistration");

      var process = new System.Diagnostics.Process();
      process.StartInfo.UseShellExecute = true;

      process.StartInfo.FileName = "powershell.exe";
      process.StartInfo.Arguments = sb.ToString();

      process.StartInfo.Verb = "runas";

      process.Start();

      while (!process.HasExited) 
        System.Threading.Thread.Sleep(5);

      outputDoc.Add(new XElement("success", "true"));
    }

    public static readonly string _serviceURI = "http://localhost:48257/api/";

    public async Task AutoInstallUpdateApps()
    {
      this.Output.AppendText("Auto-install and auto-update apps");

      var serverAppList = await GetAppList();
      var installedAppList = GetInstalledApps();
      foreach (var serverApp in serverAppList)
      {
        var installedApp = installedAppList.Where(_ => _.PackageFamilyName == serverApp.PackageFamilyName).FirstOrDefault();
        if ((installedApp != null && serverApp.InstallMode.StartsWith("Auto") && UpdateAvailable(serverApp, installedApp)) || 
            (installedApp == null && serverApp.InstallMode == "AutoInstall"))
        {
          var requestFile = System.IO.File.CreateText(TempPath + System.Guid.NewGuid().ToString() + ".req");
          using (requestFile)
          {
            await requestFile.WriteLineAsync(serverApp.Name);
            await requestFile.WriteLineAsync(serverApp.AppxUrl);
            requestFile.Close();
          }
        }
      }
    }

    private bool UpdateAvailable(OrgPortalServer.Models.AppInfo serverApp, OrgPortalServer.Models.AppInfo installedApp)
    {
      var result = false;
      var serverVersion = serverApp.Version.Split('.');
      var installedVersion = installedApp.Version.Split('.');
      if (int.Parse(serverVersion[0]) > int.Parse(installedVersion[0]))
        result = true;
      else if (int.Parse(serverVersion[1]) > int.Parse(installedVersion[1]))
        result = true;
      else if (int.Parse(serverVersion[2]) > int.Parse(installedVersion[2]))
        result = true;
      else if (int.Parse(serverVersion[3]) > int.Parse(installedVersion[3]))
        result = true;
      return result;
    }

    private async Task<List<OrgPortalServer.Models.AppInfo>> GetAppList()
    {
      var appList = new List<OrgPortalServer.Models.AppInfo>();
      var client = new System.Net.Http.HttpClient();

      var response = await client.GetAsync(_serviceURI + "Apps");
      if (response.IsSuccessStatusCode)
      {
        var data = await response.Content.ReadAsStringAsync();
        var reader = new Newtonsoft.Json.JsonTextReader(new StringReader(data));
        var info = new List<Dictionary<string, string>>();
        var infoItem = new Dictionary<string, string>();
        try
        {
          while (reader.Read())
          {
            if (reader.TokenType == Newtonsoft.Json.JsonToken.EndObject)
            {
              info.Add(infoItem);
              infoItem = new Dictionary<string, string>();
            }
            else if (reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName && reader.Value != null)
            {
              var key = reader.Value.ToString();
              reader.Read();
              if (reader.Value != null)
                infoItem.Add(key, reader.Value.ToString());
              else
                infoItem.Add(key, string.Empty);
            }
          }
        }
        catch (Exception ex)
        {
          this.Output.AppendText(ex.ToString());
        }
        foreach (var obj in info)
        {
          var app = new OrgPortalServer.Models.AppInfo();
          app.Name = obj["Name"];
          app.PackageFamilyName = obj["PackageFamilyName"];
          app.AppxUrl = obj["AppxUrl"];
          app.Version = obj["Version"];
          app.Description = obj["Description"];
          app.ImageUrl = obj["ImageUrl"];
          if (string.IsNullOrEmpty(app.ImageUrl))
            app.ImageUrl = "Assets/DarkGray.png";
          app.InstallMode = obj["InstallMode"];
          appList.Add(app);
        }
      }
      return appList;
    }

    private List<OrgPortalServer.Models.AppInfo> GetInstalledApps()
    {
      GetInstalledPackages();
      var appList = new List<OrgPortalServer.Models.AppInfo>();
      var filePath = LocalPath + "InstalledPackages.txt";
      if (System.IO.File.Exists(filePath))
      {
        var installedPackages = System.IO.File.ReadAllLines(filePath);
        var info = new OrgPortalServer.Models.AppInfo();
        foreach (var line in installedPackages)
        {
          if (line.StartsWith("Name"))
            info.Name = line.Substring(line.IndexOf(":") + 2);
          else if (line.StartsWith("PackageFamilyName"))
            info.PackageFamilyName = line.Substring(line.IndexOf(":") + 2);
          else if (line.StartsWith("Version"))
            info.Version = line.Substring(line.IndexOf(":") + 2);
          else if (line.StartsWith("IsDevelopmentMode"))
          {
            appList.Add(info);
            info = new OrgPortalServer.Models.AppInfo();
          }
        }
      }
      return appList;
    }
  }
}
