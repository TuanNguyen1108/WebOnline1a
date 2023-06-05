﻿using System;
using System.Collections.Generic;

namespace WebSite_Online1a.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? AccountId { get; set; }

    public string? FullName { get; set; }
    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime? OderDate { get; set; }

    public int? TotalMoney { get; set; }

    public string? Note { get; set; }

    public int? OrderStatusId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<OderDetail> OderDetails { get; set; } = new List<OderDetail>();
}
