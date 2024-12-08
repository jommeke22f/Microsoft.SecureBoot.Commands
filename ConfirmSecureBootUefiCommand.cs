// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.ConfirmSecureBootUefiCommand
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

using System;
using System.Management.Automation;

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  [Cmdlet("Confirm", "SecureBootUEFI")]
  public class ConfirmSecureBootUefiCommand : PrivilegesRequiredCommand
  {
    protected override void ProcessRecord()
    {
      byte[] numArray1 = new byte[1];
      byte[] numArray2 = new byte[1]{ (byte) 1 };
      NativeMethods.UNICODE_STRING VariableName = new NativeMethods.UNICODE_STRING("SecureBoot");
      uint ValueLength = 1;
      uint Attributes = 7;
      uint status1 = NativeMethods.NtQuerySystemEnvironmentValueEx(ref VariableName, NativeMethods.EFI_GLOBAL_VARIABLE.ToByteArray(), numArray1, ref ValueLength, out Attributes);
      if (status1 != 0U)
        Helper.ThrowTerminatingErrorFromNTStatus(status1, "GetFWVarFailed", (Cmdlet) this);
      VariableName = new NativeMethods.UNICODE_STRING("SetupMode");
      uint status2 = NativeMethods.NtQuerySystemEnvironmentValueEx(ref VariableName, NativeMethods.EFI_GLOBAL_VARIABLE.ToByteArray(), numArray2, ref ValueLength, out Attributes);
      if (status2 != 0U)
        Helper.ThrowTerminatingErrorFromNTStatus(status2, "GetFWVarFailed", (Cmdlet) this);
      if (numArray1[0] == (byte) 1 && numArray2[0] == (byte) 1)
        this.ThrowTerminatingError(new ErrorRecord((Exception) new InvalidStateException(Helper.resources.GetString("InvalidSecureBootState")), "InvalidSecureBootState", ErrorCategory.InvalidResult, (object) this));
      else
        this.WriteObject((object) (bool) (numArray1[0] != (byte) 1 ? 0 : (numArray2[0] == (byte) 0 ? 1 : 0)));
    }
  }
}
