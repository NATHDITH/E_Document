using System;
using System.Collections.Generic;

namespace E_Document.Models;

public partial class Document
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public int? UploadedBy { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? UploadedAt { get; set; }

    public virtual ICollection<Approval> Approvals { get; set; } = new List<Approval>();

    public virtual ICollection<Signature> Signatures { get; set; } = new List<Signature>();

    public virtual User? UploadedByNavigation { get; set; }
}
