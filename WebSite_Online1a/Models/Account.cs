using System;
using System.Collections.Generic;

namespace WebSite_Online1a.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }

    public string? HoTen { get; set; }

    public string? Sdt { get; set; }

    public string? Address { get; set; }

    public string? Avatar { get; set; }

    public int? RoleId { get; set; }

    public string? NewPassword { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }
}
