using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.Dtos
{
    public class ArticleDto
    {
        public int Id { get; set; }
        [Required, StringLength(20, ErrorMessage = "Please use only 20 characters.")]
        public string Url { get; set; } = string.Empty;
        [Required, StringLength(25, ErrorMessage = "Please use only 20 characters.")]
        public string NameForMenu { get; set; } = null!;
        [Required, StringLength(50, ErrorMessage = "Please use only 50 characters.")]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Author { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsPublished { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool IsNew { get; set; } = false;
        public bool Editing { get; set; } = false;
        public virtual List<CategoryDto> Categories { get; } = new List<CategoryDto>();
    }
}
