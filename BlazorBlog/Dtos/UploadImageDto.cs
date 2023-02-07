using Microsoft.AspNetCore.Components.Forms;

namespace BlazorBlog.Dtos
{
    public class UploadImageDto
    {
        public int ArticleId { get; set; }
        public string NameImage { get; set; } = string.Empty;
    }
}
