using System;
using System.Collections.Generic;

namespace E_Document.Models;

public partial class Signature
{
    public int Id { get; set; }

    public int? ApproverId { get; set; }

    public int? DocumentId { get; set; }

    public string SignaturePath { get; set; } = null!;

    public DateTime? SignedAt { get; set; }

    public virtual User? Approver { get; set; }

    public virtual Document? Document { get; set; }
}
