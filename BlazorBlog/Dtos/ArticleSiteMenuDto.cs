using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Dtos
{
    public class ArticleSiteMenuDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
       
    }
}
