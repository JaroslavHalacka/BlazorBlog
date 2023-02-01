namespace BlazorBlog.Services.ArticleService
{
    public interface IArticleService
    {
        Task<ServiceResponse<List<ArticleDto>>> GetAllArticle();
        Task<ServiceResponse<List<ArticleDto>>> AdminGetAllArticle();
        Task<ServiceResponse<ArticleDto>> GetArticleByUrl(string url);
        Task<ServiceResponse<ArticleDto>> GetArticleById(int id);
        
    }
}
