namespace BlazorBlog.Services.ArticleService
{
    public interface IArticleService
    {
        Task<ServiceResponse<List<ArticleDto>>> GetAllArticle();
        Task<ServiceResponse<ArticleDto>> GetArticleByUrl(string url);
    }
}
