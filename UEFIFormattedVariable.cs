// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.UEFIFormattedVariable
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  internal class UEFIFormattedVariable
  {
    private string name;
    private string time;
    private bool appendWrite;
    private byte[] bytes;

    public UEFIFormattedVariable(string name, string time, bool appendWrite, byte[] bytes)
    {
      this.name = name;
      this.time = time;
      this.appendWrite = appendWrite;
      this.bytes = bytes;
    }

    public string Name => this.name;

    public string Time => this.time;

    public bool AppendWrite => this.appendWrite;

    public byte[] Content => this.bytes;
  }
}
