using System;
using System.Collections.Generic;

namespace AppWPF.Models;

public partial class Group
{
    public int GroupId { get; set; }

    public int UserId { get; set; }

    public string GroupName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ContactGroup> ContactGroups { get; set; } = new List<ContactGroup>();

    public virtual User User { get; set; } = null!;
}
