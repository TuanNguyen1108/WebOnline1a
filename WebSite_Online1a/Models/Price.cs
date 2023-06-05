using System;
using System.Collections.Generic;

namespace WebSite_Online1a.Models;

public partial class Price
{
    public int PriceId { get; set; }

    public string? NamePrice { get; set; }

    public int? ProductId { get; set; }

    public int? PriceOld { get; set; }

    public int? PriceNew { get; set; }

    public string? Gb { get; set; }

    public string? Color { get; set; }

    public string? ColorImage { get; set; }

    public virtual ICollection<OderDetail> OderDetails { get; set; } = new List<OderDetail>();

    public virtual Product? Product { get; set; }
}
