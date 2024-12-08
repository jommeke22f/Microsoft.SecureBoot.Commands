// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.InvalidCertificateException
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  [Serializable]
  public class InvalidCertificateException : Exception
  {
    public InvalidCertificateException()
    {
    }

    public InvalidCertificateException(string msg)
      : base(msg)
    {
    }

    public InvalidCertificateException(string msg, Exception ex)
      : base(msg, ex)
    {
    }

    protected InvalidCertificateException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
