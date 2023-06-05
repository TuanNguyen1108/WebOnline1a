using System;
using System.Collections.Generic;

namespace WebSite_Online1a.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? BrandId { get; set; }

    public int? CategoryId { get; set; }

    public string? NameProduct { get; set; }

    public string? Gb { get; set; }

    public int? SpecificationId { get; set; }

    public string? ProductImage { get; set; }

    public int? Price { get; set; }

    public int? PriceOld { get; set; }

    public string? Alias { get; set; }

    public bool SpNoiBat { get; set; }

    public bool SpGiamGia { get; set; }

    public string? PhanTramGiamGia { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<OderDetail> OderDetails { get; set; } = new List<OderDetail>();

    public virtual ICollection<Price> Prices { get; set; } = new List<Price>();

    public virtual Specification? Specification { get; set; }
}
