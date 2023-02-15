namespace BlazorBlog.Dtos
{
    public class CategorySiteMenuDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public virtual List<ArticleSiteMenuDto> Articles { get; } = new List<ArticleSiteMenuDto>();
    }
}
