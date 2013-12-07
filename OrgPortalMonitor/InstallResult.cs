using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgPortalMonitor
{
  public class InstallResult
  {
    public string Output { get; set; }
    public string Error { get; set; }

    public override string ToString()
    {
      var result = new StringBuilder();
      result.AppendLine("ERROR");
      result.AppendLine(Error);
      result.AppendLine("OUTPUT");
      result.AppendLine(Output);
      return result.ToString();
    }
  }
}
