using System;
using System.Collections.Generic;

namespace ErpSupport.Api.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int CreatedById { get; set; }

    public int? ResolvedById { get; set; }

    public string Title { get; set; } = null!;

    public string ProblemDescription { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? ResolutionText { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual User CreatedBy { get; set; } = null!;

    public virtual ICollection<KnowledgeBase> KnowledgeBases { get; set; } = new List<KnowledgeBase>();

    public virtual User? ResolvedBy { get; set; }
}
