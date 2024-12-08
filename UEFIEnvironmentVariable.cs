// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.UEFIEnvironmentVariable
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  internal class UEFIEnvironmentVariable
  {
    private string name;
    private byte[] bytes;
    private UEFIEnvironmentVariableAttributes attributes;

    public UEFIEnvironmentVariable(string name, byte[] bytes, ulong attributes)
    {
      this.name = name;
      this.bytes = bytes;
      this.attributes = new UEFIEnvironmentVariableAttributes(attributes);
    }

    public string Name => this.name;

    public byte[] Bytes => this.bytes;

    public UEFIEnvironmentVariableAttributes Attributes => this.attributes;
  }
}
