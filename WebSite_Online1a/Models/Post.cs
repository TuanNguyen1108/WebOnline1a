using System;
using System.Collections.Generic;

namespace WebSite_Online1a.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string? Title { get; set; }

    public string? Contents { get; set; }

    public string? Image { get; set; }

    public bool Published { get; set; }

    public bool IsHot { get; set; }

    public bool IsNewfeed { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? Author { get; set; }

    public string? MetaKey { get; set; }

    public string? MetaDesc { get; set; }

    public int? Views { get; set; }

    public string? Alias { get; set; }
}
