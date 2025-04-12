using System;
using System.Collections.Generic;

namespace AppWPF.Models;

public partial class Log
{
    public int LogId { get; set; }

    public int UserId { get; set; }

    public int? ContactId { get; set; }

    public string Action { get; set; } = null!;

    public string? Details { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Contact? Contact { get; set; }

    public virtual User User { get; set; } = null!;
}
