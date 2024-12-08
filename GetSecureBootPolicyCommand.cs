// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.GetSecureBootPolicyCommand
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

using System.Management.Automation;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  [Cmdlet("Get", "SecureBootPolicy")]
  public class GetSecureBootPolicyCommand : Cmdlet
  {
    protected override void ProcessRecord()
    {
      NativeMethods.SYSTEM_SECUREBOOT_POLICY_INFORMATION SystemInformation = new NativeMethods.SYSTEM_SECUREBOOT_POLICY_INFORMATION();
      uint status = NativeMethods.NtQuerySystemInformation(143U, out SystemInformation, (uint) Marshal.SizeOf((object) SystemInformation), out uint _);
      if (status != 0U)
        Helper.ThrowTerminatingErrorFromNTStatus(status, "GetSecureBootPolicy", (Cmdlet) this);
      this.WriteObject((object) new SecureBootPolicy(SystemInformation.PolicyPublisher, SystemInformation.PolicyVersion));
    }
  }
}
