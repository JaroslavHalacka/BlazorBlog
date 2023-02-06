namespace BlazorBlog.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public bool IsVisible { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
        public bool IsNew { get; set; } = false;
        public bool Editing { get; set; } = false;
    }
}
