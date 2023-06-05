using System;
using System.Collections.Generic;

namespace WebSite_Online1a.Models;

public partial class Specification
{
    public int SpecificationId { get; set; }

    public string? NameSpecification { get; set; }

    public string? Description { get; set; }

    public string? Cpu { get; set; }

    public string? SizeCreen { get; set; }

    public string? OperatingSystem { get; set; }

    public string? Ram { get; set; }

    public string? Camera { get; set; }

    public string? CameraSelfie { get; set; }

    public string? Battery { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
