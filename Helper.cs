// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.Helper
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  internal class Helper
  {
    internal const string ZEROED_GUID = "{00000000-0000-0000-0000-000000000000}";
    internal static ResourceManager resources = new ResourceManager("Microsoft.SecureBoot.Commands", Assembly.GetExecutingAssembly());
    internal static Dictionary<string, string> EfiVariables = new Dictionary<string, string>()
    {
      {
        "pk",
        "PK"
      },
      {
        "kek",
        "KEK"
      },
      {
        "setupmode",
        "SetupMode"
      },
      {
        "secureboot",
        "SecureBoot"
      },
      {
        "db",
        "db"
      },
      {
        "dbx",
        "dbx"
      },
      {
        "dbt",
        "dbt"
      },
      {
        "pkdefault",
        "PKDefault"
      },
      {
        "kekdefault",
        "KEKDefault"
      },
      {
        "dbdefault",
        "dbDefault"
      },
      {
        "dbxdefault",
        "dbxDefault"
      },
      {
        "dbtdefault",
        "dbtDefault"
      }
    };
    internal const uint SIZE_OF_RSA2048_SIGNATURE = 256;
    internal static byte[] SUPPORTED_RSA_EXPONENT = new byte[3]
    {
      (byte) 1,
      (byte) 0,
      (byte) 1
    };
    internal const uint SIZE_OF_EFI_SIGNATURE_LIST = 28;
    internal static byte[] EmptyPKCS7 = new byte[37]
    {
      (byte) 48,
      (byte) 35,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 49,
      (byte) 15,
      (byte) 48,
      (byte) 13,
      (byte) 6,
      (byte) 9,
      (byte) 96,
      (byte) 134,
      (byte) 72,
      (byte) 1,
      (byte) 101,
      (byte) 3,
      (byte) 4,
      (byte) 2,
      (byte) 1,
      (byte) 5,
      (byte) 0,
      (byte) 48,
      (byte) 11,
      (byte) 6,
      (byte) 9,
      (byte) 42,
      (byte) 134,
      (byte) 72,
      (byte) 134,
      (byte) 247,
      (byte) 13,
      (byte) 1,
      (byte) 7,
      (byte) 1,
      (byte) 49,
      (byte) 0
    };
    internal static Dictionary<string, Helper.AlgorithmInformation> HashSizes = new Dictionary<string, Helper.AlgorithmInformation>()
    {
      {
        "sha1",
        new Helper.AlgorithmInformation(NativeMethods.EFI_CERT_SHA1_GUID, 20U)
      },
      {
        "sha256",
        new Helper.AlgorithmInformation(NativeMethods.EFI_CERT_SHA256_GUID, 32U)
      },
      {
        "sha384",
        new Helper.AlgorithmInformation(NativeMethods.EFI_CERT_SHA384_GUID, 48U)
      },
      {
        "sha512",
        new Helper.AlgorithmInformation(NativeMethods.EFI_CERT_SHA512_GUID, 64U)
      }
    };

    private Helper()
    {
    }

    internal static bool SetPrivilege(ref IntPtr handle, uint privilege)
    {
      bool flag = false;
      if (handle.Equals((object) IntPtr.Zero))
      {
        if (!NativeMethods.OpenThreadToken(NativeMethods.GetCurrentThread(), 32U, true, ref handle) && !NativeMethods.OpenProcessToken(NativeMethods.GetCurrentProcess(), 32, ref handle))
          return false;
        flag = true;
      }
      NativeMethods.LUID lpLuid;
      if (!NativeMethods.LookupPrivilegeValue((string) null, "SeSystemEnvironmentPrivilege", out lpLuid))
      {
        if (flag)
        {
          NativeMethods.CloseHandle(handle);
          handle = IntPtr.Zero;
        }
        return false;
      }
      NativeMethods.TOKEN_PRIVILEGE NewState;
      NewState.PrivilegeCount = 1U;
      NewState.Privilege = new NativeMethods.LUID_AND_ATTRIBUTES();
      NewState.Privilege.Luid = lpLuid;
      NewState.Privilege.Attributes = privilege;
      if (!NativeMethods.AdjustTokenPrivileges(handle, false, ref NewState, (uint) Marshal.SizeOf((object) NewState), new IntPtr(0), new IntPtr(0)))
      {
        if (flag)
        {
          NativeMethods.CloseHandle(handle);
          handle = IntPtr.Zero;
        }
        return false;
      }
      if (Marshal.GetLastWin32Error() != 1300)
        return true;
      if (flag)
      {
        NativeMethods.CloseHandle(handle);
        handle = IntPtr.Zero;
      }
      return false;
    }

    internal static string GetFilePath(string filePathInput)
    {
      return Path.IsPathRooted(filePathInput) ? filePathInput : new SessionState().Path.CurrentFileSystemLocation.Path.ToString() + "\\" + filePathInput;
    }

    internal static Guid GetNamespaceForVariable(string name)
    {
      return name.Equals("db") || name.Equals("dbx") ? NativeMethods.EFI_IMAGE_SECURITY_DATABASE_GUID : NativeMethods.EFI_GLOBAL_VARIABLE;
    }

    internal static void ThrowTerminatingErrorFromNTStatus(
      uint status,
      string errorId,
      Cmdlet cmdlet)
    {
      switch (status)
      {
        case 2151874566:
          cmdlet.ThrowTerminatingError(new ErrorRecord((Exception) new StatusException(Helper.resources.GetString("SecureBootNotEnabled")), errorId, ErrorCategory.ResourceUnavailable, (object) cmdlet));
          break;
        case 3221225474:
        case 3221225475:
          cmdlet.ThrowTerminatingError(new ErrorRecord((Exception) new PlatformNotSupportedException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("NotImplementedException"), new object[1]
          {
            (object) status.ToString("X8", (IFormatProvider) CultureInfo.CurrentCulture)
          })), errorId, ErrorCategory.NotImplemented, (object) cmdlet));
          break;
        case 3221225506:
          cmdlet.ThrowTerminatingError(new ErrorRecord((Exception) new UnauthorizedAccessException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("AccessVioloationException"), new object[1]
          {
            (object) status.ToString("X8", (IFormatProvider) CultureInfo.CurrentCulture)
          })), errorId, ErrorCategory.PermissionDenied, (object) cmdlet));
          break;
        case 3221225728:
          cmdlet.ThrowTerminatingError(new ErrorRecord((Exception) new StatusException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("VarNotFoundException"), new object[1]
          {
            (object) status.ToString("X8", (IFormatProvider) CultureInfo.CurrentCulture)
          })), errorId, ErrorCategory.ResourceUnavailable, (object) cmdlet));
          break;
        default:
          cmdlet.ThrowTerminatingError(new ErrorRecord((Exception) new StatusException(status), errorId, ErrorCategory.NotSpecified, (object) cmdlet));
          break;
      }
    }

    internal static byte[] HexStringToByteArray(string hexString)
    {
      byte[] byteArray = new byte[hexString.Length >> 1];
      for (int index = 0; index < byteArray.Length; ++index)
        byteArray[index] = Convert.ToByte(hexString.Substring(index << 1, 2), 16);
      return byteArray;
    }

    internal static string BytesToFormattedString(byte[] bytes)
    {
      string formattedString = " ";
      for (int index = 0; index < bytes.Length; ++index)
      {
        if (index != 0 && index % 16 == 0)
          formattedString += "\n                         ";
        formattedString = formattedString + bytes[index].ToString("X2", (IFormatProvider) CultureInfo.CurrentCulture) + " ";
      }
      return formattedString;
    }

    internal static void WriteVerboseEfiSignatureList(
      Cmdlet cmdlet,
      Guid signatureType,
      uint signatureListSize,
      uint signatureSize)
    {
      cmdlet.WriteVerbose(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("FV-SignatureType"), new object[1]
      {
        (object) signatureType.ToString()
      }));
      cmdlet.WriteVerbose(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("FV-SignatureListSize"), new object[2]
      {
        (object) signatureListSize.ToString((IFormatProvider) CultureInfo.CurrentCulture),
        (object) signatureListSize.ToString("X4", (IFormatProvider) CultureInfo.CurrentCulture)
      }));
      cmdlet.WriteVerbose(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("FV-SignatureHeaderSize"), new object[1]
      {
        (object) 0
      }));
      cmdlet.WriteVerbose(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("FV-SignatureSize"), new object[2]
      {
        (object) signatureSize.ToString((IFormatProvider) CultureInfo.CurrentCulture),
        (object) signatureSize.ToString("X4", (IFormatProvider) CultureInfo.CurrentCulture)
      }));
    }

    internal static void WriteVerboseEfiSignatureData(
      Cmdlet cmdlet,
      Guid signatureOwner,
      byte[] signatureData)
    {
      cmdlet.WriteVerbose(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("FV-SignatureOwner"), new object[1]
      {
        (object) signatureOwner.ToString()
      }));
      cmdlet.WriteVerbose(Helper.resources.GetString("FV-SignatureData") + Helper.BytesToFormattedString(signatureData));
    }

    internal static bool AreArraysEqual<T>(T[] array1, T[] array2)
    {
      if (array1.Equals((object) array2))
        return true;
      if (array1.Length != array2.Length)
        return false;
      for (int index = 0; index < array1.Length; ++index)
      {
        ref T local1 = ref array1[index];
        T obj = default (T);
        if ((object) obj == null)
        {
          obj = local1;
          local1 = ref obj;
        }
        // ISSUE: variable of a boxed type
        __Boxed<T> local2 = (object) array2[index];
        if (!local1.Equals((object) local2))
          return false;
      }
      return true;
    }

    internal static void HrChk(uint hr)
    {
      if (hr == 0U)
        return;
      Marshal.ThrowExceptionForHR((int) hr);
    }

    internal static byte[] GetEfiTime(DateTime date)
    {
      byte[] destinationArray = new byte[Marshal.SizeOf((object) new NativeMethods.EFI_TIME())];
      int destinationIndex = 0;
      Array.Copy((Array) BitConverter.GetBytes((ushort) date.Year), 0, (Array) destinationArray, destinationIndex, 2);
      int num1 = destinationIndex + 2;
      byte[] numArray1 = destinationArray;
      int index1 = num1;
      int num2 = index1 + 1;
      int month = (int) (byte) date.Month;
      numArray1[index1] = (byte) month;
      byte[] numArray2 = destinationArray;
      int index2 = num2;
      int num3 = index2 + 1;
      int day = (int) (byte) date.Day;
      numArray2[index2] = (byte) day;
      byte[] numArray3 = destinationArray;
      int index3 = num3;
      int num4 = index3 + 1;
      int hour = (int) (byte) date.Hour;
      numArray3[index3] = (byte) hour;
      byte[] numArray4 = destinationArray;
      int index4 = num4;
      int num5 = index4 + 1;
      int minute = (int) (byte) date.Minute;
      numArray4[index4] = (byte) minute;
      byte[] numArray5 = destinationArray;
      int index5 = num5;
      int num6 = index5 + 1;
      int second = (int) (byte) date.Second;
      numArray5[index5] = (byte) second;
      return destinationArray;
    }

    internal static byte[] CreateAuthenticatedVariableData(
      byte[] data,
      byte[] signedData,
      byte[] efiTime)
    {
      byte[] signedData1 = Helper.GetSignedData(signedData);
      byte[] destinationArray = new byte[efiTime.Length + signedData1.Length + (data == null ? 0 : data.Length)];
      int destinationIndex1 = 0;
      Array.Copy((Array) efiTime, 0, (Array) destinationArray, destinationIndex1, efiTime.Length);
      int destinationIndex2 = destinationIndex1 + efiTime.Length;
      Array.Copy((Array) signedData1, 0, (Array) destinationArray, destinationIndex2, signedData1.Length);
      int destinationIndex3 = destinationIndex2 + signedData1.Length;
      if (data != null)
        Array.Copy((Array) data, 0, (Array) destinationArray, destinationIndex3, data.Length);
      return destinationArray;
    }

    internal static byte[] GetSignedData(byte[] signedData)
    {
      byte[] destinationArray = new byte[Marshal.SizeOf((object) new NativeMethods.WIN_CERTIFICATE()) + Marshal.SizeOf((object) new NativeMethods.EFI_GUID()) + (signedData == null ? 0 : signedData.Length)];
      int destinationIndex1 = 0;
      byte[] byteArray = NativeMethods.EFI_CERT_TYPE_PKCS7_GUID.ToByteArray();
      Array.Copy((Array) BitConverter.GetBytes((uint) destinationArray.Length), 0, (Array) destinationArray, destinationIndex1, 4);
      int destinationIndex2 = destinationIndex1 + 4;
      Array.Copy((Array) BitConverter.GetBytes((ushort) 512), 0, (Array) destinationArray, destinationIndex2, 2);
      int destinationIndex3 = destinationIndex2 + 2;
      Array.Copy((Array) BitConverter.GetBytes((ushort) 3825), 0, (Array) destinationArray, destinationIndex3, 2);
      int destinationIndex4 = destinationIndex3 + 2;
      Array.Copy((Array) byteArray, 0, (Array) destinationArray, destinationIndex4, byteArray.Length);
      int destinationIndex5 = destinationIndex4 + byteArray.Length;
      if (signedData != null)
        Array.Copy((Array) signedData, 0, (Array) destinationArray, destinationIndex5, signedData.Length);
      return destinationArray;
    }

    internal static byte[] GetContentFromCertificates(
      string[] certificates,
      Guid signatureowner,
      bool formatwithcert,
      Cmdlet cmdlet)
    {
      byte[] byteArray = signatureowner.ToByteArray();
      ArrayList arrayList = new ArrayList();
      Guid guid;
      if (formatwithcert)
      {
        cmdlet.WriteVerbose(Helper.resources.GetString("FV-FormattingWithCert"));
        guid = NativeMethods.EFI_CERT_X509_GUID;
      }
      else
      {
        cmdlet.WriteVerbose(Helper.resources.GetString("FV-FormattingWithKey"));
        Guid efiCertRsA2048Guid = NativeMethods.EFI_CERT_RSA2048_GUID;
        uint signatureSize = (uint) (256 + byteArray.Length);
        uint signatureListSize = (uint) (28UL + (ulong) signatureSize * (ulong) certificates.Length);
        arrayList.AddRange((ICollection) Helper.GetEfiSignatureList(efiCertRsA2048Guid, signatureListSize, signatureSize));
      }
      for (int index = 0; index < certificates.Length; ++index)
      {
        cmdlet.WriteVerbose(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("FV-CertIndex"), new object[1]
        {
          (object) index
        }));
        X509Certificate2 x509Certificate2 = new X509Certificate2(Helper.GetFilePath(certificates[index]));
        byte[] signatureData;
        if (formatwithcert)
        {
          Guid efiCertX509Guid = NativeMethods.EFI_CERT_X509_GUID;
          signatureData = x509Certificate2.GetRawCertData();
          uint signatureSize = (uint) (signatureData.Length + byteArray.Length);
          uint signatureListSize = 28U + signatureSize;
          Helper.WriteVerboseEfiSignatureList(cmdlet, efiCertX509Guid, signatureListSize, signatureSize);
          arrayList.AddRange((ICollection) Helper.GetEfiSignatureList(efiCertX509Guid, signatureListSize, signatureSize));
        }
        else
        {
          guid = NativeMethods.EFI_CERT_RSA2048_GUID;
          RSA key = (RSA) x509Certificate2.PublicKey.Key;
          if (!Helper.AreArraysEqual<byte>(key.ExportParameters(false).Exponent, Helper.SUPPORTED_RSA_EXPONENT))
            cmdlet.ThrowTerminatingError(new ErrorRecord((Exception) new InvalidCertificateException(Helper.resources.GetString("InvalidCertificateExponent")), "InvalidCertificateExponent", ErrorCategory.InvalidData, (object) cmdlet));
          signatureData = key.ExportParameters(false).Modulus;
          if (signatureData.Length != 256)
            cmdlet.ThrowTerminatingError(new ErrorRecord((Exception) new InvalidCertificateException(Helper.resources.GetString("InvalidCertificateModulus")), "InvalidCertificateSize", ErrorCategory.InvalidData, (object) cmdlet));
        }
        Helper.WriteVerboseEfiSignatureData(cmdlet, signatureowner, signatureData);
        arrayList.AddRange((ICollection) Helper.GetEfiSignatureData(signatureowner, signatureData));
      }
      return (byte[]) arrayList.ToArray(typeof (byte));
    }

    internal static byte[] GetContentFromHashes(
      string[] hashes,
      string algorithm,
      Guid signatureowner,
      Cmdlet cmdlet)
    {
      cmdlet.WriteVerbose(Helper.resources.GetString("FV-FormattingHashes"));
      Helper.AlgorithmInformation algorithmInformation;
      Helper.HashSizes.TryGetValue(algorithm.ToLower(CultureInfo.CurrentCulture), out algorithmInformation);
      Guid signatureType = algorithmInformation.SignatureType;
      byte[] byteArray1 = signatureowner.ToByteArray();
      uint signatureSize = algorithmInformation.HashSize + (uint) byteArray1.Length;
      uint signatureListSize = (uint) (28 + (int) signatureSize * hashes.Length);
      byte[] numArray = new byte[((long) algorithmInformation.HashSize + (long) byteArray1.Length) * (long) hashes.Length];
      Helper.WriteVerboseEfiSignatureList(cmdlet, signatureType, signatureListSize, signatureSize);
      for (int index = 0; index < hashes.Length; ++index)
      {
        cmdlet.WriteVerbose(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("FV-HashIndex"), new object[1]
        {
          (object) index
        }));
        byte[] byteArray2 = Helper.HexStringToByteArray(hashes[index]);
        if ((long) byteArray2.Length != (long) algorithmInformation.HashSize || hashes[index].Length % 2 != 0)
          cmdlet.ThrowTerminatingError(new ErrorRecord((Exception) new ArgumentException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, Helper.resources.GetString("InvalidHashSize"), new object[1]
          {
            (object) index
          })), "InvalidHashParameter", ErrorCategory.InvalidArgument, (object) cmdlet));
        Helper.WriteVerboseEfiSignatureData(cmdlet, signatureowner, byteArray2);
        Helper.CopyOwnerAndSignature(numArray, byteArray1, byteArray2, index);
      }
      return Helper.AddEfiSignatureList(signatureType, signatureListSize, signatureSize, numArray);
    }

    internal static byte[] AddEfiSignatureList(
      Guid signatureType,
      uint signatureListSize,
      uint signatureSize,
      byte[] content)
    {
      byte[] efiSignatureList = Helper.GetEfiSignatureList(signatureType, signatureListSize, signatureSize);
      byte[] destinationArray = new byte[efiSignatureList.Length + content.Length];
      Array.Copy((Array) efiSignatureList, (Array) destinationArray, efiSignatureList.Length);
      Array.Copy((Array) content, 0, (Array) destinationArray, efiSignatureList.Length, content.Length);
      return destinationArray;
    }

    internal static byte[] GetEfiSignatureList(
      Guid signatureType,
      uint signatureListSize,
      uint signatureSize)
    {
      byte[] destinationArray = new byte[28];
      int destinationIndex1 = 0;
      byte[] byteArray = signatureType.ToByteArray();
      Array.Copy((Array) byteArray, 0, (Array) destinationArray, destinationIndex1, byteArray.Length);
      int destinationIndex2 = destinationIndex1 + byteArray.Length;
      Array.Copy((Array) BitConverter.GetBytes(signatureListSize), 0, (Array) destinationArray, destinationIndex2, 4);
      int destinationIndex3 = destinationIndex2 + 4 + 4;
      Array.Copy((Array) BitConverter.GetBytes(signatureSize), 0, (Array) destinationArray, destinationIndex3, 4);
      int num = destinationIndex3 + 4;
      return destinationArray;
    }

    internal static byte[] GetEfiSignatureData(Guid signatureOwner, byte[] signatureData)
    {
      byte[] byteArray = signatureOwner.ToByteArray();
      byte[] destination = new byte[byteArray.Length + signatureData.Length];
      Helper.CopyOwnerAndSignature(destination, byteArray, signatureData, 0);
      return destination;
    }

    internal static void CopyOwnerAndSignature(
      byte[] destination,
      byte[] sigOwner,
      byte[] signature,
      int index)
    {
      Array.Copy((Array) sigOwner, 0, (Array) destination, index * (signature.Length + sigOwner.Length), sigOwner.Length);
      Array.Copy((Array) signature, 0, (Array) destination, index * (signature.Length + sigOwner.Length) + sigOwner.Length, signature.Length);
    }

    internal static bool IsWinPe()
    {
      return Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\MiniNT") != null;
    }

    internal static bool IsBitLockerResealNeeded()
    {
      NativeMethods.FVE_SECUREBOOT_BINDING_STATE SecureBootBindingState = NativeMethods.FVE_SECUREBOOT_BINDING_STATE.FVE_SECUREBOOT_BINDING_UNKNOWN;
      if (Helper.IsWinPe())
        return false;
      string osVolume = Helper.GetOsVolume();
      NativeMethods.FVE_STATUS_V4 Status = new NativeMethods.FVE_STATUS_V4();
      Status.StructureSize = (uint) Marshal.SizeOf((object) Status);
      Status.StructureVersion = 4U;
      try
      {
        Helper.HrChk(NativeMethods.FveGetStatusW(osVolume, ref Status));
        if (((int) Status.Flags & 1) == 0 || ((int) Status.Flags & 512) == 0 || ((int) Status.Flags & 1024) != 0)
          return false;
        Helper.HrChk(NativeMethods.FveGetSecureBootBindingState(out SecureBootBindingState));
        return SecureBootBindingState == NativeMethods.FVE_SECUREBOOT_BINDING_STATE.FVE_SECUREBOOT_BINDING_BOUND;
      }
      catch (DllNotFoundException ex)
      {
        return false;
      }
    }

    internal static IntPtr GetBitLockerHandle()
    {
      IntPtr zero1 = IntPtr.Zero;
      string osVolume = Helper.GetOsVolume();
      try
      {
        Helper.HrChk(NativeMethods.FveOpenVolumeW(osVolume, true, ref zero1));
      }
      catch
      {
        if (zero1 != IntPtr.Zero)
        {
          int num = (int) NativeMethods.FveCloseVolume(zero1);
          IntPtr zero2 = IntPtr.Zero;
        }
        throw;
      }
      return zero1;
    }

    internal static void ResealBitLocker(IntPtr fveHandle)
    {
      uint FlagsOut = 0;
      Helper.HrChk(NativeMethods.FveKeyManagement(fveHandle, 4U, out FlagsOut));
      Helper.HrChk(NativeMethods.FveCommitChanges(fveHandle));
    }

    internal static string GetOsVolume()
    {
      return "\\\\.\\" + Environment.GetEnvironmentVariable("SystemDrive");
    }

    internal class AlgorithmInformation
    {
      internal Guid SignatureType;
      internal uint HashSize;

      internal AlgorithmInformation(Guid sigType, uint hashSize)
      {
        this.SignatureType = sigType;
        this.HashSize = hashSize;
      }
    }
  }
}
