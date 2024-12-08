// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.SetSecureBootUefiCommand
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

using System;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  [Cmdlet("Set", "SecureBootUEFI")]
  public class SetSecureBootUefiCommand : PrivilegesRequiredCommand, IDisposable
  {
    private string name;
    private string contentfilepath;
    private byte[] content;
    private string signedfilepath;
    private string time;
    private SwitchParameter append;
    private string outputfilepath;
    private bool IsResealNeeded;
    private IntPtr fveHandle = IntPtr.Zero;
    private bool disposed;

    [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true)]
    [ValidateSet(new string[] {"PK", "KEK", "db", "dbx"}, IgnoreCase = true)]
    [Alias(new string[] {"n"})]
    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    [Parameter(ParameterSetName = "ContentsFromFile")]
    [Alias(new string[] {"f"})]
    [ValidateNotNullOrEmpty]
    public string ContentFilePath
    {
      get => this.contentfilepath;
      set => this.contentfilepath = value;
    }

    [Parameter(ParameterSetName = "ContentsFromByteArray", ValueFromPipelineByPropertyName = true)]
    public byte[] Content
    {
      get => this.content;
      set => this.content = value;
    }

    [Parameter]
    [Alias(new string[] {"s"})]
    [ValidateNotNullOrEmpty]
    public string SignedFilePath
    {
      get => this.signedfilepath;
      set => this.signedfilepath = value;
    }

    [Parameter(ValueFromPipelineByPropertyName = true, Mandatory = true)]
    [Alias(new string[] {"t"})]
    public string Time
    {
      get => this.time;
      set => this.time = value;
    }

    [Parameter(ValueFromPipelineByPropertyName = true)]
    [Alias(new string[] {"append"})]
    public SwitchParameter AppendWrite
    {
      get => this.append;
      set => this.append = value;
    }

    [Parameter]
    [Alias(new string[] {"of"})]
    [ValidateNotNullOrEmpty]
    public string OutputFilePath
    {
      get => this.outputfilepath;
      set => this.outputfilepath = value;
    }

    protected override void BeginProcessing()
    {
      base.BeginProcessing();
      if (!string.IsNullOrEmpty(this.contentfilepath))
        this.contentfilepath = Helper.GetFilePath(this.contentfilepath);
      if (!string.IsNullOrEmpty(this.signedfilepath))
        this.signedfilepath = Helper.GetFilePath(this.signedfilepath);
      if (!string.IsNullOrEmpty(this.outputfilepath))
        this.outputfilepath = Helper.GetFilePath(this.outputfilepath);
      if (string.IsNullOrEmpty(this.outputfilepath))
        this.IsResealNeeded = Helper.IsBitLockerResealNeeded();
      if (!this.IsResealNeeded)
        return;
      this.fveHandle = Helper.GetBitLockerHandle();
    }

    protected override void ProcessRecord()
    {
      Helper.EfiVariables.TryGetValue(this.name.ToLower(CultureInfo.InvariantCulture), out this.name);
      byte[] numArray1;
      if (string.IsNullOrEmpty(this.contentfilepath))
      {
        numArray1 = this.content;
      }
      else
      {
        FileStream fileStream = File.Open(this.contentfilepath, FileMode.Open, FileAccess.Read);
        numArray1 = new byte[fileStream.Length];
        fileStream.Read(numArray1, 0, numArray1.Length);
        fileStream.Close();
      }
      uint num = 39;
      if ((bool) this.append)
        num |= 64U;
      Guid namespaceForVariable = Helper.GetNamespaceForVariable(this.name);
      byte[] numArray2 = new byte[Marshal.SizeOf((object) new NativeMethods.EFI_TIME())];
      byte[] efiTime = Helper.GetEfiTime(DateTime.Parse(this.time, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal));
      byte[] authenticatedVariableData;
      if (!string.IsNullOrEmpty(this.signedfilepath))
      {
        FileStream fileStream = File.Open(this.signedfilepath, FileMode.Open, FileAccess.Read);
        byte[] numArray3 = new byte[fileStream.Length];
        fileStream.Read(numArray3, 0, numArray3.Length);
        authenticatedVariableData = Helper.CreateAuthenticatedVariableData(numArray1, numArray3, efiTime);
        fileStream.Close();
      }
      else
        authenticatedVariableData = Helper.CreateAuthenticatedVariableData(numArray1, Helper.EmptyPKCS7, efiTime);
      if (!string.IsNullOrEmpty(this.outputfilepath))
      {
        FileStream fileStream = File.Open(this.outputfilepath, FileMode.Create);
        fileStream.Write(authenticatedVariableData, 0, authenticatedVariableData.Length);
        fileStream.Close();
      }
      else
      {
        NativeMethods.UNICODE_STRING VariableName = new NativeMethods.UNICODE_STRING(this.name);
        uint status = NativeMethods.NtSetSystemEnvironmentValueEx(ref VariableName, namespaceForVariable.ToByteArray(), authenticatedVariableData, (uint) authenticatedVariableData.Length, num);
        if (status != 0U)
          Helper.ThrowTerminatingErrorFromNTStatus(status, "SetFWVarFailed", (Cmdlet) this);
      }
      this.WriteObject((object) new UEFIEnvironmentVariable(this.name, authenticatedVariableData, (ulong) num));
    }

    protected override void EndProcessing()
    {
      if (this.IsResealNeeded)
        Helper.ResealBitLocker(this.fveHandle);
      base.EndProcessing();
    }

    public void Dispose()
    {
      this.DisposeInternal();
      GC.SuppressFinalize((object) this);
    }

    protected virtual void DisposeInternal()
    {
      if (!this.disposed && this.IsResealNeeded)
      {
        int num = (int) NativeMethods.FveCloseVolume(this.fveHandle);
        this.fveHandle = IntPtr.Zero;
      }
      this.disposed = true;
    }

    ~SetSecureBootUefiCommand() => this.DisposeInternal();
  }
}
