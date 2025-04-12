using System;
using System.Collections.Generic;

namespace AppWPF.Models;

public partial class FavoriteContact
{
    public int UserId { get; set; }

    public int ContactId { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Contact Contact { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
