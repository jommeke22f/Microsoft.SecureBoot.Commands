// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.GetSecureBootUefiCommand
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

using System;
using System.Globalization;
using System.IO;
using System.Management.Automation;

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  [Cmdlet("Get", "SecureBootUEFI")]
  public class GetSecureBootUefiCommand : PrivilegesRequiredCommand
  {
    private string myName;
    private string outputfilepath;

    [Parameter(Mandatory = true, Position = 1, ParameterSetName = "__AllParameterSets", ValueFromPipeline = true)]
    [ValidateSet(new string[] {"PK", "KEK", "db", "dbx", "SetupMode", "SecureBoot", "PKDefault", "KEKDefault", "dbDefault", "dbxDefault", "dbt", "dbtDefault"}, IgnoreCase = true)]
    [Alias(new string[] {"n"})]
    public string Name
    {
      get => this.myName;
      set => this.myName = value;
    }

    [Parameter]
    [Alias(new string[] {"f"})]
    public string OutputFilePath
    {
      get => this.outputfilepath;
      set => this.outputfilepath = value;
    }

    protected override void BeginProcessing()
    {
      base.BeginProcessing();
      if (string.IsNullOrEmpty(this.outputfilepath))
        return;
      this.outputfilepath = Helper.GetFilePath(this.outputfilepath);
    }

    protected override void ProcessRecord()
    {
      uint ValueLength = 0;
      uint Attributes = 0;
      Helper.EfiVariables.TryGetValue(this.Name.ToLower(CultureInfo.InvariantCulture), out this.myName);
      Guid namespaceForVariable = Helper.GetNamespaceForVariable(this.Name);
      NativeMethods.UNICODE_STRING VariableName = new NativeMethods.UNICODE_STRING(this.Name);
      uint status1 = NativeMethods.NtQuerySystemEnvironmentValueEx(ref VariableName, namespaceForVariable.ToByteArray(), (byte[]) null, ref ValueLength, out Attributes);
      if (status1 != 3221225507U)
        Helper.ThrowTerminatingErrorFromNTStatus(status1, "GetFWVarFailed", (Cmdlet) this);
      byte[] numArray = new byte[(int) ValueLength];
      uint status2 = NativeMethods.NtQuerySystemEnvironmentValueEx(ref VariableName, namespaceForVariable.ToByteArray(), numArray, ref ValueLength, out Attributes);
      if (status2 != 0U)
        Helper.ThrowTerminatingErrorFromNTStatus(status2, "GetFWVarFailed", (Cmdlet) this);
      if (!string.IsNullOrEmpty(this.outputfilepath))
      {
        FileStream fileStream = File.Open(this.outputfilepath, FileMode.Create);
        fileStream.Write(numArray, 0, numArray.Length);
        fileStream.Close();
      }
      this.WriteObject((object) new UEFIEnvironmentVariable(this.Name, numArray, (ulong) Attributes));
    }
  }
}
