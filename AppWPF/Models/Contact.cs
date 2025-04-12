using System;
using System.Collections.Generic;

namespace AppWPF.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Notes { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Avatar { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ContactGroup> ContactGroups { get; set; } = new List<ContactGroup>();

    public virtual ICollection<FavoriteContact> FavoriteContacts { get; set; } = new List<FavoriteContact>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual User User { get; set; } = null!;
}
