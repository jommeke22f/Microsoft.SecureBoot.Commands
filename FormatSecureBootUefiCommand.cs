// Decompiled with JetBrains decompiler
// Type: Microsoft.SecureBoot.Commands.FormatSecureBootUefiCommand
// Assembly: Microsoft.SecureBoot.Commands, Version=10.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0E959B1D-7C53-48FA-8AAC-DA98E68B750F
// Assembly location: C:\Windows\Microsoft.NET\assembly\GAC_32\Microsoft.SecureBoot.Commands\v4.0_10.0.0.0__31bf3856ad364e35\Microsoft.SecureBoot.Commands.dll

using System;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Text;

#nullable disable
namespace Microsoft.SecureBoot.Commands
{
  [Cmdlet("Format", "SecureBootUEFI")]
  public class FormatSecureBootUefiCommand : Cmdlet
  {
    private string name;
    private Guid signatureowner;
    private string[] certificatesfilepath;
    private SwitchParameter formatwithcert;
    private string[] hashes;
    private string algorithm;
    private SwitchParameter delete;
    private string signablefilepath;
    private string time;
    private SwitchParameter append;
    private string contentfilepath;

    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    [ValidateSet(new string[] {"PK", "KEK", "db", "dbx"}, IgnoreCase = true)]
    [Alias(new string[] {"n"})]
    public virtual string Name
    {
      get => this.name;
      set => this.name = value;
    }

    [Parameter(Mandatory = true, ParameterSetName = "FormatForCertificates")]
    [Parameter(Mandatory = true, ParameterSetName = "FormatForHashes")]
    [Alias(new string[] {"g"})]
    [ValidatePattern("[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}")]
    public Guid SignatureOwner
    {
      get => this.signatureowner;
      set => this.signatureowner = value;
    }

    [Parameter(Mandatory = true, ParameterSetName = "FormatForCertificates")]
    [ValidateNotNullOrEmpty]
    [Alias(new string[] {"c"})]
    public string[] CertificateFilePath
    {
      get => this.certificatesfilepath;
      set => this.certificatesfilepath = value;
    }

    [Parameter(ParameterSetName = "FormatForCertificates")]
    [Alias(new string[] {"cert"})]
    public SwitchParameter FormatWithCert
    {
      get => this.formatwithcert;
      set => this.formatwithcert = value;
    }

    [Parameter(Mandatory = true, ParameterSetName = "FormatForHashes")]
    [ValidateNotNullOrEmpty]
    [ValidatePattern("[a-fA-F0-9]*")]
    [Alias(new string[] {"h"})]
    public string[] Hash
    {
      get => this.hashes;
      set => this.hashes = value;
    }

    [Parameter(Mandatory = true, ParameterSetName = "FormatForHashes")]
    [ValidateNotNullOrEmpty]
    [ValidateSet(new string[] {"sha1", "sha256", "sha384", "sha512"})]
    [Alias(new string[] {"alg"})]
    public string Algorithm
    {
      get => this.algorithm;
      set => this.algorithm = value;
    }

    [Parameter(Mandatory = true, ParameterSetName = "FormatForDelete")]
    [Alias(new string[] {"del"})]
    public SwitchParameter Delete
    {
      get => this.delete;
      set => this.delete = value;
    }

    [Parameter]
    [Alias(new string[] {"s"})]
    [ValidateNotNullOrEmpty]
    public string SignableFilePath
    {
      get => this.signablefilepath;
      set => this.signablefilepath = value;
    }

    [Parameter]
    [Alias(new string[] {"t"})]
    public string Time
    {
      get => this.time;
      set => this.time = value;
    }

    [Parameter(ParameterSetName = "FormatForCertificates")]
    [Parameter(ParameterSetName = "FormatForHashes")]
    [Alias(new string[] {"append"})]
    public SwitchParameter AppendWrite
    {
      get => this.append;
      set => this.append = value;
    }

    [Parameter(ParameterSetName = "FormatForCertificates")]
    [Parameter(ParameterSetName = "FormatForHashes")]
    [ValidateNotNullOrEmpty]
    [Alias(new string[] {"f"})]
    public string ContentFilePath
    {
      get => this.contentfilepath;
      set => this.contentfilepath = value;
    }

    protected override void BeginProcessing()
    {
      if (!string.IsNullOrEmpty(this.contentfilepath))
        this.contentfilepath = Helper.GetFilePath(this.contentfilepath);
      if (!string.IsNullOrEmpty(this.signablefilepath))
        this.signablefilepath = Helper.GetFilePath(this.signablefilepath);
      if (string.IsNullOrEmpty(this.signablefilepath) || !string.IsNullOrEmpty(this.time))
        return;
      this.ThrowTerminatingError(new ErrorRecord((Exception) new ArgumentException(Helper.resources.GetString("SignableFileAndTimeRequired")), "SignableFileAndTimeRequired", ErrorCategory.InvalidArgument, (object) this));
    }

    protected override void ProcessRecord()
    {
      byte[] numArray = (byte[]) null;
      uint num = 39;
      if ((bool) this.append)
        num |= 64U;
      Helper.EfiVariables.TryGetValue(this.Name.ToLower(CultureInfo.InvariantCulture), out this.name);
      if (!(bool) this.delete)
      {
        numArray = this.certificatesfilepath == null ? Helper.GetContentFromHashes(this.hashes, this.algorithm, this.signatureowner, (Cmdlet) this) : Helper.GetContentFromCertificates(this.certificatesfilepath, this.signatureowner, (bool) this.formatwithcert, (Cmdlet) this);
        if (this.contentfilepath != null)
        {
          FileStream fileStream = File.Open(this.contentfilepath, FileMode.Create);
          fileStream.Write(numArray, 0, numArray.Length);
          fileStream.Close();
        }
      }
      if (!string.IsNullOrEmpty(this.signablefilepath))
      {
        FileStream output = File.Open(this.signablefilepath, FileMode.Create);
        UnicodeEncoding unicodeEncoding1 = new UnicodeEncoding(false, false);
        UnicodeEncoding unicodeEncoding2 = unicodeEncoding1;
        BinaryWriter binaryWriter = new BinaryWriter((Stream) output, (Encoding) unicodeEncoding2);
        DateTime date = DateTime.Parse(this.time, (IFormatProvider) CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal);
        binaryWriter.Write(unicodeEncoding1.GetBytes(this.name));
        binaryWriter.Write(Helper.GetNamespaceForVariable(this.name).ToByteArray());
        binaryWriter.Write(num);
        binaryWriter.Write(Helper.GetEfiTime(date));
        if (numArray != null)
          binaryWriter.Write(numArray);
        binaryWriter.Close();
      }
      this.WriteObject((object) new UEFIFormattedVariable(this.name, this.time, (bool) this.append, numArray));
    }
  }
}
