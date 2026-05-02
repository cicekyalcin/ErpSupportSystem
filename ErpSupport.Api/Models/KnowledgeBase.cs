using System;
using System.Collections.Generic;

namespace ErpSupport.Api.Models;

public partial class KnowledgeBase
{
    public int Id { get; set; }

    public int? TicketId { get; set; }

    public int AuthorId { get; set; }

    public string ProblemTitle { get; set; } = null!;

    public string SolutionText { get; set; } = null!;

    public string? SearchKeywords { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ViewCount { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual Ticket? Ticket { get; set; }
}
