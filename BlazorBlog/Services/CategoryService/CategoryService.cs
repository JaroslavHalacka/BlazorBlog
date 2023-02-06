using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoryService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<CategoryDto>>> GetAllCategories()
        {
            ServiceResponse<List<CategoryDto>> response = new ServiceResponse<List<CategoryDto>>();
            try
            {
                var result = _mapper.Map<List<CategoryDto>>
                    (
                        await _context.Categories
                        .Where(c => c.IsVisible && !c.IsDeleted)
                        .ToListAsync()
                    );
                if (result == null)
                    throw new Exception("System error");

                if (result.Count == 0)
                    throw new Exception("No Category found");

                response.Data = result;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

        }

        public async Task<ServiceResponse<List<CategoryDto>>> AdminGetAllCategories()
        {
            ServiceResponse<List<CategoryDto>> response = new ServiceResponse<List<CategoryDto>>();
            try
            {
                var result = _mapper.Map<List<CategoryDto>>
                    (
                        await _context.Categories.ToListAsync()
                    );
                if (result == null)
                    throw new Exception("System error");

                if (result.Count == 0)
                    throw new Exception("No Category found");

                response.Data = result;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<CategoryDto>> GetCategoryById(int id)
        {
            ServiceResponse<CategoryDto> response = new ServiceResponse<CategoryDto>();
            try
            {
                var result = _mapper.Map<CategoryDto>(await _context.Categories.FindAsync(id));

                if (result == null)
                    throw new Exception("System error");

                response.Data = result;
                response.Success = true;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<CategoryDto>> UpdateCategory(CategoryDto category)
        {
            ServiceResponse<CategoryDto> response = new ServiceResponse<CategoryDto>();
            try
            {
                var result = await _context.Categories.FirstOrDefaultAsync(a => a.Id == category.Id);
                if (result == null || category == null)
                    throw new Exception("Category in database or Category is null");

                result = _mapper.Map(category, result);

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<CategoryDto>(result);
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<CategoryDto>> CreateCategory(CategoryDto category)
        {
            ServiceResponse<CategoryDto> response = new ServiceResponse<CategoryDto>();
            try
            {
                var newCategory = _mapper.Map<Category>(category);
                if (newCategory == null)
                    throw new Exception("New Category is not exist");

                _context.Categories.Add(newCategory);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<CategoryDto>(newCategory);
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
