using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.Services.ArticleService
{
    public class ArticleService : IArticleService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ArticleService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<ArticleDto>>> GetAllArticle()
        {

            ServiceResponse<List<ArticleDto>> response = new ServiceResponse<List<ArticleDto>>();

            try
            {
                var result = _mapper.Map<List<ArticleDto>>(await _context.Articles.ToListAsync());

                if (result == null)
                    throw new Exception("System error");

                if (result.Count == 0)
                    throw new Exception("No article found");

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

        public async Task<ServiceResponse<ArticleDto>> GetArticleByUrl(string url)
        {
            ServiceResponse<ArticleDto> response = new ServiceResponse<ArticleDto>();
            try
            {
                var result = _mapper.Map<ArticleDto>(await _context.Articles.FirstAsync(a=>a.Url == url));

                if(result == null)
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
    }
}
