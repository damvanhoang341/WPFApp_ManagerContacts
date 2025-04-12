using System;
using System.Collections.Generic;

namespace AppWPF.Models;

public partial class ContactGroup
{
    public int ContactId { get; set; }

    public int GroupId { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Contact Contact { get; set; } = null!;

    public virtual Group Group { get; set; } = null!;
}
