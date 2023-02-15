namespace BlazorBlog.Services.CategoryService
{
    public interface ICategoryService
    {
        event Action CategorySiteMenuChange;
        List<CategorySiteMenuDto> ListCategorySiteMenuDtos { get; set; }
        Task<ServiceResponse<List<CategoryDto>>> GetAllCategories();
        Task<ServiceResponse<List<CategoryDto>>> AdminGetAllCategories();
        Task<ServiceResponse<CategoryDto>> GetCategoryById(int id);
        Task<ServiceResponse<List<CategoryDto>>> GetCategoriesWitOutIdArticle(int articleId);
        

        Task<ServiceResponse<List<CategoryDto>>> UpdateCategory(CategoryDto category);
        Task<ServiceResponse<List<CategoryDto>>> AddCategory(CategoryDto category); 
        Task<ServiceResponse<List<CategoryDto>>> DeleteCategory(int id);
        Task<ServiceResponse<List<CategoryDto>>> NoDelete(int id);
        Task GetSiteMenuCategories();




    }
}
