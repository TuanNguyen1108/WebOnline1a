using System;
using System.Collections.Generic;

namespace WebSite_Online1a.Models;

public partial class Custormer
{
    public int CustomerId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Note { get; set; }

    public DateTime? DateOder { get; set; }

    public bool Status { get; set; }
}
