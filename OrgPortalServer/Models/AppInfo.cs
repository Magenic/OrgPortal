﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace OrgPortalServer.Models
{
  public class AppInfo
  {
    public string Name { get; set; }
    public string PackageFamilyName { get; set; }
    public string Description { get; set; }
    public string AppxUrl { get; set; }
    public string ImageUrl { get; set; }
    public string Version { get; set; }
    public string InstallMode { get; set; }
  }
}