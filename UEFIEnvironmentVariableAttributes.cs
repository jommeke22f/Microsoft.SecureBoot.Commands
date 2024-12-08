// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.UEFIEnvironmentVariableAttributes
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  internal class UEFIEnvironmentVariableAttributes
  {
    private ulong attributes;

    public UEFIEnvironmentVariableAttributes(ulong attributes) => this.attributes = attributes;

    public ulong Attributes => this.attributes;

    public override string ToString()
    {
      string str = "";
      if (((long) this.Attributes & 1L) != 0L)
        str += "NON VOLATILE\n";
      if (((long) this.Attributes & 2L) != 0L)
        str += "BOOTSERVICE ACCESS\n";
      if (((long) this.Attributes & 4L) != 0L)
        str += "RUNTIME ACCESS\n";
      if (((long) this.Attributes & 8L) != 0L)
        str += "HARDWARE ERROR RECORD\n";
      if (((long) this.Attributes & 16L) != 0L)
        str += "AUTHENTICATED WRITE ACCESS\n";
      if (((long) this.Attributes & 32L) != 0L)
        str += "TIME BASED AUTHENTICATED WRITE ACCESS\n";
      if (((long) this.Attributes & 64L) != 0L)
        str += "APPEND WRITE\n";
      return str;
    }
  }
}
