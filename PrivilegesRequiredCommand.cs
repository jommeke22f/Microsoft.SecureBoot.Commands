// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.PrivilegesRequiredCommand
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

using System;
using System.Management.Automation;

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  public abstract class PrivilegesRequiredCommand : Cmdlet
  {
    private IntPtr handle;

    protected override void BeginProcessing()
    {
      this.handle = new IntPtr(0);
      if (Helper.SetPrivilege(ref this.handle, 2U))
        return;
      this.ThrowTerminatingError(new ErrorRecord((Exception) new UnauthorizedAccessException(Helper.resources.GetString("SetPrivilegeFailed")), "SetPrivilegeFailed", ErrorCategory.PermissionDenied, (object) this));
    }

    protected abstract override void ProcessRecord();

    protected override void EndProcessing()
    {
      if (!Helper.SetPrivilege(ref this.handle, 2147483648U))
        this.ThrowTerminatingError(new ErrorRecord((Exception) new UnauthorizedAccessException(Helper.resources.GetString("SetPrivilegeFailed")), "SetPrivilegeDoneFailed", ErrorCategory.PermissionDenied, (object) this));
      NativeMethods.CloseHandle(this.handle);
    }
  }
}
