using BlazorBlog.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;

        public CategoryService(DataContext context, IMapper mapper, IDbContextFactory<DataContext> dbContextFactory)
        {
            _context = context;
            _mapper = mapper;
            _dbContextFactory = dbContextFactory;
        }

        public event Action CategorySiteMenuChange;
        public List<CategorySiteMenuDto> ListCategorySiteMenuDtos { get; set; } = new List<CategorySiteMenuDto>();

        public async Task<ServiceResponse<List<CategoryDto>>> GetAllCategories()
        {
            ServiceResponse<List<CategoryDto>> response = new ServiceResponse<List<CategoryDto>>();
            try
            {
                using DataContext dataContext = _dbContextFactory.CreateDbContext();
                var result = _mapper.Map<List<CategoryDto>>
                    (
                        await dataContext.Categories
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
                using DataContext dataContext = _dbContextFactory.CreateDbContext();
                var result = _mapper.Map<List<CategoryDto>>
                    (
                        await dataContext.Categories.ToListAsync()
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
                using DataContext dataContext = _dbContextFactory.CreateDbContext();
                var result = _mapper.Map<CategoryDto>(await dataContext.Categories.FindAsync(id));

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

        public async Task<ServiceResponse<List<CategoryDto>>> UpdateCategory(CategoryDto category)
        {
            ServiceResponse<List<CategoryDto>> response = new ServiceResponse<List<CategoryDto>>();
            try
            {
                using DataContext dataContext = _dbContextFactory.CreateDbContext();
                var result = await dataContext.Categories.FirstOrDefaultAsync(a => a.Id == category.Id);
                if (result == null || category == null)
                    throw new Exception("Category in database or Category is null");

                result = _mapper.Map(category, result);

                await dataContext.SaveChangesAsync();

                response = await AdminGetAllCategories();
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }



        public async Task<ServiceResponse<List<CategoryDto>>> AddCategory(CategoryDto category)
        {
            using DataContext dataContext = _dbContextFactory.CreateDbContext();
            category.Editing = category.IsNew = false;
            var newCategory = _mapper.Map<Category>(category);
            dataContext.Categories.Add(newCategory);
            await dataContext.SaveChangesAsync();
            return await AdminGetAllCategories();
        }

        public async Task<ServiceResponse<List<CategoryDto>>> DeleteCategory(int id)
        {
            using DataContext dataContext = _dbContextFactory.CreateDbContext();
            var category = await dataContext.Categories.FindAsync(id);

            if (category == null)
            {
                return new ServiceResponse<List<CategoryDto>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            category.IsDeleted = true;
            await dataContext.SaveChangesAsync();
            return await AdminGetAllCategories();
        }

        public async Task<ServiceResponse<List<CategoryDto>>> NoDelete(int id)
        {
            using DataContext dataContext = _dbContextFactory.CreateDbContext();
            var category = await dataContext.Categories.FindAsync(id);

            if (category == null)
            {
                return new ServiceResponse<List<CategoryDto>>
                {
                    Success = false,
                    Message = "Category not found."
                };
            }

            category.IsDeleted = false;
            await dataContext.SaveChangesAsync();
            return await AdminGetAllCategories();
        }

        public async Task<ServiceResponse<List<CategoryDto>>> GetCategoriesWitOutIdArticle(int articleId)
        {
            ServiceResponse<List<CategoryDto>> response = new ServiceResponse<List<CategoryDto>>();
            using DataContext dataContext = _dbContextFactory.CreateDbContext();

            var aaa = await dataContext.Categories
                .Where(c => !c.Articles.Any(a => a.Id == articleId))
                .ToListAsync();

            response.Data = _mapper.Map<List<CategoryDto>>(aaa);
            return response;
        }

        public async Task GetSiteMenuCategories()
        {
            using DataContext dataContext = _dbContextFactory.CreateDbContext();
            var result = _mapper.Map<List<CategorySiteMenuDto>>
            (
                await dataContext.Categories
                   .Include(c => c.Articles)
                .Where(c => c.IsVisible && !c.IsDeleted)
                .ToListAsync()
            );
            ListCategorySiteMenuDtos = result;

            CategorySiteMenuChange.Invoke();
        }

    }


}

