using System;
using System.Collections.Generic;

namespace WebSite_Online1a.Models;

public partial class Brand
{
    public int BrandId { get; set; }

    public string? NameBrand { get; set; }

    public string? Alias { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
