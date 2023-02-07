using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ImageService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                var result = await _context.Images
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
    }
}
