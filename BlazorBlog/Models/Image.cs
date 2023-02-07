using System;
using System.Collections.Generic;

namespace BlazorBlog.Models;

public partial class Image
{
    public int ArticleId { get; set; }

    public string NameImage { get; set; } = null!;

    public virtual Article Article { get; set; } = null!;
}
