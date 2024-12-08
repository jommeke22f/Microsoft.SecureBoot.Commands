// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.NativeMethods
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  internal class NativeMethods
  {
    internal const int TOKEN_ADJUST_PRIVILEGES = 32;
    internal const string SE_SYSTEM_ENVIRONMENT_NAME = "SeSystemEnvironmentPrivilege";
    internal const long ERROR_NOT_ALL_ASSIGNED = 1300;
    internal static Guid EFI_GLOBAL_VARIABLE = new Guid(2347032417U, (ushort) 37834, (ushort) 4562, (byte) 170, (byte) 13, (byte) 0, (byte) 224, (byte) 152, (byte) 3, (byte) 43, (byte) 140);
    internal const string EFI_VARIABLE_SETUPMODE = "SetupMode";
    internal const string EFI_VARIABLE_SECUREBOOT = "SecureBoot";
    internal const string EFI_VARIABLE_PK = "PK";
    internal const string EFI_VARIABLE_KEK = "KEK";
    internal const string EFI_VARIABLE_PKDEFAULT = "PKDefault";
    internal const string EFI_VARIABLE_KEKDEFAULT = "KEKDefault";
    internal const string EFI_VARIABLE_DBDEFAULT = "dbDefault";
    internal const string EFI_VARIABLE_DBXDEFAULT = "dbxDefault";
    internal const string EFI_VARIABLE_DBTDEFAULT = "dbtDefault";
    internal static Guid EFI_IMAGE_SECURITY_DATABASE_GUID = new Guid(3608785611U, (ushort) 15674, (ushort) 17814, (byte) 163, (byte) 188, (byte) 218, (byte) 208, (byte) 14, (byte) 103, (byte) 101, (byte) 111);
    internal const string EFI_IMAGE_SECURITY_DATABASE = "db";
    internal const string EFI_IMAGE_SECURITY_DATABASE1 = "dbx";
    internal const string EFI_IMAGE_SECURITY_DATABASE2 = "dbt";
    internal const string EFI_SECUREBOOT_STAGED_POLICY = "StagedPolicy";
    internal static Guid EFI_CERT_TYPE_PKCS7_GUID = new Guid(1253036701, (short) 26847, (short) 18926, (byte) 138, (byte) 169, (byte) 52, (byte) 125, (byte) 55, (byte) 86, (byte) 101, (byte) 167);
    internal const ushort WIN_CERT_TYPE_EFI_GUID = 3825;
    internal const ushort EFI_WIN_CERTIFICATE_REVISION = 512;
    internal static Guid EFI_MICROSOFT_GUID = new Guid(2012912317, (short) 857, (short) 19762, (byte) 189, (byte) 96, (byte) 40, (byte) 244, (byte) 231, (byte) 143, (byte) 120, (byte) 75);
    internal static Guid EFI_CERT_SHA256_GUID = new Guid(3250853414U, (ushort) 20556, (ushort) 16530, (byte) 172, (byte) 169, (byte) 65, (byte) 249, (byte) 54, (byte) 147, (byte) 67, (byte) 40);
    internal static Guid EFI_CERT_RSA2048_GUID = new Guid(1012360936, (short) 9884, (short) 20020, (byte) 170, (byte) 20, (byte) 237, (byte) 119, (byte) 110, (byte) 133, (byte) 179, (byte) 182);
    internal static Guid EFI_CERT_RSA2048_SHA256_GUID = new Guid(3803406736U, (ushort) 34715, (ushort) 19005, (byte) 173, (byte) 141, (byte) 242, (byte) 231, (byte) 187, (byte) 163, (byte) 39, (byte) 132);
    internal static Guid EFI_CERT_SHA1_GUID = new Guid(2188158226U, (ushort) 53008, (ushort) 19145, (byte) 177, (byte) 135, (byte) 190, (byte) 1, (byte) 73, (byte) 102, (byte) 49, (byte) 189);
    internal static Guid EFI_CERT_RSA2048_SHA1_GUID = new Guid(1744323663U, (ushort) 34627, (ushort) 18673, (byte) 163, (byte) 40, (byte) 30, (byte) 170, (byte) 184, (byte) 115, (byte) 96, (byte) 128);
    internal static Guid EFI_CERT_X509_GUID = new Guid(2780846497U, (ushort) 38116, (ushort) 19111, (byte) 135, (byte) 181, (byte) 171, (byte) 21, (byte) 92, (byte) 43, (byte) 240, (byte) 114);
    internal static Guid EFI_CERT_SHA224_GUID = new Guid(191779379U, (ushort) 42588, (ushort) 17609, (byte) 148, (byte) 7, (byte) 217, (byte) 171, (byte) 131, (byte) 191, (byte) 200, (byte) 189);
    internal static Guid EFI_CERT_SHA384_GUID = new Guid(4282274567U, (ushort) 40912, (ushort) 18633, (byte) 133, (byte) 241, (byte) 138, (byte) 213, (byte) 108, (byte) 112, (byte) 30, (byte) 1);
    internal static Guid EFI_CERT_SHA512_GUID = new Guid(155062190U, (ushort) 42692, (ushort) 20304, (byte) 159, (byte) 27, (byte) 212, (byte) 30, (byte) 43, (byte) 137, (byte) 193, (byte) 154);
    internal const uint RegistryRootHKLM = 2147483650;
    internal const uint RegistryRootSecureBootPolicies = 2164260864;
    internal const uint STATUS_SUCCESS = 0;
    internal const uint STATUS_NOT_IMPLEMENTED = 3221225474;
    internal const uint STATUS_INVALID_INFO_CLASS = 3221225475;
    internal const uint STATUS_INVALID_PARAMETER = 3221225485;
    internal const uint STATUS_ACCESS_VIOLATION = 3221225506;
    internal const uint STATUS_BUFFER_TOO_SMALL = 3221225507;
    internal const uint STATUS_VARIABLE_NOT_FOUND = 3221225728;
    internal const uint STATUS_INSUFFICIENT_NVRAM_RESOURCES = 3221226580;
    internal const uint STATUS_SECUREBOOT_NOT_ENABLED = 2151874566;
    internal const uint FVE_KEY_MGMT_IN_REFRESH_TPM_BINDING = 4;
    internal const uint FVE_KEY_MGMT_OUT_TPM_BINDING_REFRESHED = 8;
    internal const uint FVE_E_NOT_ACTIVATED = 2150694920;
    internal const uint FVE_E_NOT_ON_STACK = 2150694986;
    internal const uint FVE_STATUS_VERSION_4 = 4;
    internal const uint S_FALSE = 1;
    internal const uint FVE_FLAG_ON = 1;
    internal const uint FVE_FLAG_HAS_TPM_DATA = 512;
    internal const uint FVE_FLAG_DISABLED = 1024;
    internal const uint SystemSecureBootPolicyInformation = 143;
    internal const uint VARIABLE_INFORMATION_NAMES = 1;
    internal const uint VARIABLE_INFORMATION_VALUES = 2;
    internal const uint VER_NT_DOMAIN_CONTROLLER = 2;
    internal const uint VER_NT_SERVER = 3;

    private NativeMethods()
    {
    }

    [DllImport("Advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool AdjustTokenPrivileges(
      IntPtr TokenHandle,
      [MarshalAs(UnmanagedType.Bool)] bool DisableAllPrivileges,
      ref NativeMethods.TOKEN_PRIVILEGE NewState,
      uint BufferLength,
      IntPtr PreviousState,
      IntPtr ReturnLength);

    [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool LookupPrivilegeValue(
      string lpSystemName,
      string lpName,
      out NativeMethods.LUID lpLuid);

    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern IntPtr GetCurrentProcess();

    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern IntPtr GetCurrentThread();

    [DllImport("Advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool OpenProcessToken(
      IntPtr ProcessToken,
      int DesiredAccess,
      ref IntPtr TokenHandle);

    [DllImport("Advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool OpenThreadToken(
      IntPtr ThreadHandle,
      uint DesiredAccess,
      bool OpenAsSelf,
      ref IntPtr TokenHandle);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetFirmwareType(out NativeMethods.FirmwareType firmwareType);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool CopyFile(
      string lpExistingFileName,
      string lpNewFileName,
      [MarshalAs(UnmanagedType.Bool)] bool bFailIfExists);

    [DllImport("ntdll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint NtQuerySystemEnvironmentValueEx(
      ref NativeMethods.UNICODE_STRING VariableName,
      byte[] VendorGuid,
      byte[] Value,
      ref uint ValueLength,
      out uint Attributes);

    [DllImport("ntdll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint NtSetSystemEnvironmentValueEx(
      ref NativeMethods.UNICODE_STRING VariableName,
      byte[] VendorGuid,
      byte[] Value,
      uint ValueLength,
      uint Attributes);

    [DllImport("ntdll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint NtQuerySystemInformation(
      uint SystemInformationClass,
      out NativeMethods.SYSTEM_SECUREBOOT_POLICY_INFORMATION SystemInformation,
      uint SystemInformationLength,
      out uint ReturnLength);

    [DllImport("ntdll.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint NtEnumerateSystemEnvironmentValuesEx(
      uint InformationClass,
      byte[] Buffer,
      ref uint BufferLength);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool QueryPerformanceCounter(ref long count);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool QueryPerformanceFrequency(ref long frequency);

    [DllImport("fveapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint FveOpenVolumeW(
      string VolumeName,
      [MarshalAs(UnmanagedType.Bool)] bool bNeedWriteAccess,
      ref IntPtr phVolume);

    [DllImport("fveapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint FveKeyManagement(
      IntPtr hFveVolume,
      uint FlagsIn,
      out uint FlagsOut);

    [DllImport("fveapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint FveCommitChanges(IntPtr hFveVolume);

    [DllImport("fveapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint FveCloseVolume(IntPtr hFveVolume);

    [DllImport("fveapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint FveGetStatusW(
      string VolumeName,
      ref NativeMethods.FVE_STATUS_V4 Status);

    [DllImport("fveapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern uint FveGetSecureBootBindingState(
      out NativeMethods.FVE_SECUREBOOT_BINDING_STATE SecureBootBindingState);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool GetVersionEx(ref NativeMethods.OSVERSIONINFOEX osVersion);

    [Flags]
    internal enum SePrivileges : uint
    {
      PrivilegeEnabled = 2,
      PrivilegeUsedForAccess = 2147483648, // 0x80000000
    }

    [Flags]
    internal enum UefiVariableAttributes
    {
      NonVolatile = 1,
      BootServiceAccess = 2,
      RunTimeAccess = 4,
      HardwareErrorRecord = 8,
      AuthenticatedWriteAccess = 16, // 0x00000010
      TimeBasedAuthenticatedWriteAccess = 32, // 0x00000020
      AppendWrite = 64, // 0x00000040
    }

    internal enum FirmwareType
    {
      Unknown,
      BIOS,
      EFI,
      Max,
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    internal struct SYSTEM_SECUREBOOT_POLICY_INFORMATION
    {
      internal Guid PolicyPublisher;
      internal uint PolicyVersion;
      internal uint PolicyOptions;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    internal struct VARIABLE_NAME
    {
      private long NextEntryOffset;
      private NativeMethods.EFI_GUID VendorGuid;
      private StringBuilder Name;
    }

    internal enum FVE_SECUREBOOT_BINDING_STATE
    {
      FVE_SECUREBOOT_BINDING_UNKNOWN = -1, // 0xFFFFFFFF
      FVE_SECUREBOOT_BINDING_NOT_POSSIBLE = 0,
      FVE_SECUREBOOT_BINDING_DISABLED_BY_POLICY = 1,
      FVE_SECUREBOOT_BINDING_POSSIBLE = 2,
      FVE_SECUREBOOT_BINDING_BOUND = 3,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct FVE_STATUS_V4
    {
      internal uint StructureSize;
      internal uint StructureVersion;
      internal ushort FveVersion;
      internal uint Flags;
      internal double ConvertedPercent;
      internal uint LastConvertStatus;
      internal long VolArriveTime;
      internal double WipedPercent;
      internal uint WipeState;
      internal uint WipeCount;
      internal ulong ExtendedFlags;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct OSVERSIONINFOEX
    {
      internal uint dwOSVersionInfoSize;
      internal uint dwMajorVersion;
      internal uint dwMinorVersion;
      internal uint dwBuildNumber;
      internal uint dwPlatformId;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
      internal string szCSDVersion;
      internal ushort wServicePackMajor;
      internal ushort wServicePackMinor;
      internal ushort wSuiteMask;
      internal byte wProductType;
      internal byte wReserved;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct LUID
    {
      internal uint LowPart;
      internal uint HighPart;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct LUID_AND_ATTRIBUTES
    {
      internal NativeMethods.LUID Luid;
      internal uint Attributes;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct TOKEN_PRIVILEGE
    {
      internal uint PrivilegeCount;
      internal NativeMethods.LUID_AND_ATTRIBUTES Privilege;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct UNICODE_STRING
    {
      private const int SIZEOF_WCHAR = 2;
      private ushort Length;
      private ushort MaximumLength;
      [MarshalAs(UnmanagedType.LPWStr)]
      private string Buffer;

      public UNICODE_STRING(string sourceString)
      {
        this.Buffer = sourceString;
        this.Length = (ushort) (this.Buffer.Length * 2);
        this.MaximumLength = (ushort) ((uint) this.Length + 2U);
      }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct EFI_TIME
    {
      internal ushort Year;
      internal byte Month;
      internal byte Day;
      internal byte Hour;
      internal byte Minute;
      internal byte Second;
      internal byte Pad1;
      internal uint Nanosecond;
      internal ushort TimeZone;
      internal byte Daylight;
      internal byte Pad2;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WIN_CERTIFICATE
    {
      internal uint dwLength;
      internal ushort wRevision;
      internal ushort wCertificateType;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    internal struct EFI_GUID
    {
      internal uint Data1;
      internal ushort Data2;
      internal ushort Data3;
      internal ulong Data4;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    internal struct EFI_SIGNATURE_DATA
    {
      private NativeMethods.EFI_GUID SignatureOwner;
      private byte[] SignatureData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    internal struct EFI_SIGNATURE_LIST
    {
      private NativeMethods.EFI_GUID SignatureType;
      private uint SignatureListSize;
      private uint SignatureHeaderSize;
      private uint SignatureSize;
    }
  }
}
