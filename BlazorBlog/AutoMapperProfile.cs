using BlazorBlog.Models;

namespace BlazorBlog
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleDto, Article>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<UploadImageDto, Image>();
            CreateMap<Image, UploadImageDto>();

            CreateMap<Category, CategorySiteMenuDto>();
            CreateMap<Article, ArticleSiteMenuDto>();

        }
    }
}
