using System;
using System.Collections.Generic;

namespace ErpSupport.Api.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<KnowledgeBase> KnowledgeBases { get; set; } = new List<KnowledgeBase>();

    public virtual ICollection<Ticket> TicketCreatedBies { get; set; } = new List<Ticket>();

    public virtual ICollection<Ticket> TicketResolvedBies { get; set; } = new List<Ticket>();
}
