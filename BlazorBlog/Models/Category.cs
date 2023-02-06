using System;
using System.Collections.Generic;

namespace BlazorBlog.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Url { get; set; } = null!;

    public bool IsVisible { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Article> Articles { get; } = new List<Article>();
}
