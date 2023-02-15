using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;

        public ImageService(DataContext context, IMapper mapper, IDbContextFactory<DataContext> dbContextFactory)
        {
            _context = context;
            _mapper = mapper;
            _dbContextFactory = dbContextFactory;
        }

        public async Task<ServiceResponse<bool>> AddUploadImage(UploadImageDto uploadImage)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var result = await _context.Images
                                .Where(i => i.ArticleId == uploadImage.ArticleId && i.NameImage == uploadImage.NameImage)
                                .FirstOrDefaultAsync();
                if (result != null)
                    throw new Exception("Image is exist and will be rewritten");

                Image newImage = _mapper.Map<Image>(uploadImage);

                _context.Images.Add(newImage);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Message = "Image was saved";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<UploadImageDto>>> GetListImageByArticleId(int articleId)
        {
            ServiceResponse<List<UploadImageDto>> response = new ServiceResponse<List<UploadImageDto>>();
            try
            {
                using DataContext dataContext = _dbContextFactory.CreateDbContext();
                var result = await dataContext.Images
                            .Where(i => i.ArticleId == articleId)
                            .ToListAsync();

                if (result == null || result.Count == 0)
                    throw new Exception("Article not have Image");

                response.Data = _mapper.Map<List<UploadImageDto>>(result);
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success= false;
                response.Message = ex.Message;
            }
            return response;
        }


        public async Task<ServiceResponse<bool>> DeleteImage(int articleId, string nameImage, string fullPathImage)

        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var result = await _context.Images
                                .Where(i => i.ArticleId == articleId && i.NameImage == nameImage)
                                .FirstOrDefaultAsync();
                if (result == null)
                    throw new Exception("Image is not exist in database");
                if(!File.Exists(fullPathImage))
                    throw new Exception("Image is not exist in folder");

                File.Delete(fullPathImage);

                _context.Images.Remove(result);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Message = "Image was deleted";
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
