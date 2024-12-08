// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.StatusException
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  [Serializable]
  public class StatusException : Exception
  {
    public StatusException()
    {
    }

    public StatusException(int error)
      : base(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("Win32Exception"), new object[1]
      {
        (object) error
      }))
    {
    }

    public StatusException(uint error)
      : base(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("NTStatusException"), new object[1]
      {
        (object) error.ToString("X8", (IFormatProvider) CultureInfo.CurrentCulture)
      }))
    {
    }

    public StatusException(string msg)
      : base(msg)
    {
    }

    public StatusException(string msg, Exception ex)
      : base(msg, ex)
    {
    }

    protected StatusException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
    }
  }
}
