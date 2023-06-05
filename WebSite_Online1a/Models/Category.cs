using System;
using System.Collections.Generic;

namespace WebSite_Online1a.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public int? BrandId { get; set; }

    public string? NameCategory { get; set; }

    public string? CategoryImage { get; set; }

    public string? Alias { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
