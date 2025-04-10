using System;
using System.Collections.Generic;

namespace E_Document.Models;

public partial class Approval
{
    public int Id { get; set; }

    public int? DocumentId { get; set; }

    public int? ApproverId { get; set; }

    public int ApprovalOrder { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? ApprovedAt { get; set; }

    public string? LastApprover { get; set; }

    public virtual User? Approver { get; set; }

    public virtual Document? Document { get; set; }
}
