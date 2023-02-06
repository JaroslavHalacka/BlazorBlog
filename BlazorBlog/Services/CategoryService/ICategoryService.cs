namespace BlazorBlog.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<CategoryDto>>> GetAllCategories();
        Task<ServiceResponse<List<CategoryDto>>> AdminGetAllCategories();
        Task<ServiceResponse<CategoryDto>> GetCategoryById(int id);
        Task<ServiceResponse<CategoryDto>> UpdateCategory(CategoryDto category);
        Task<ServiceResponse<CategoryDto>> CreateCategory(CategoryDto category);

    }
}
