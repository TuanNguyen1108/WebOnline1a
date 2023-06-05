using System;
using System.Collections.Generic;

namespace WebSite_Online1a.Models;

public partial class OderDetail
{
    public int? OrderDetailId { get; set; }

    public int? ProductId { get; set; }

    public int? PriceId { get; set; }

    public int? Price { get; set; }

    public int? Num { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Price? PriceNavigation { get; set; }

    public virtual Product? Product { get; set; }
}
