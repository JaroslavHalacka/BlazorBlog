using System;
using System.Collections.Generic;

namespace BlazorBlog.Models;

public partial class Article
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public string NameForMenu { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Content { get; set; } = null!;

    public int Author { get; set; }

    public DateTime DateCreated { get; set; }

    public bool IsPublished { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Image> Images { get; } = new List<Image>();

    public virtual ICollection<Category> Categories { get; } = new List<Category>();
}
