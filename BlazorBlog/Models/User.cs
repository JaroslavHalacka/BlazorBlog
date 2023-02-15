using System;
using System.Collections.Generic;

namespace BlazorBlog.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime DateCreated { get; set; }
}
