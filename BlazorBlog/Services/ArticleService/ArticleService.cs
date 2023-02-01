using Microsoft.EntityFrameworkCore;
using System;

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
                var result = _mapper.Map<List<ArticleDto>>
                    (
                        await _context.Articles
                        .Where(a => a.IsPublished && !a.IsDeleted)
                        .ToListAsync()
                    );

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
        public async Task<ServiceResponse<List<ArticleDto>>> AdminGetAllArticle()
        {
            ServiceResponse<List<ArticleDto>> response = new ServiceResponse<List<ArticleDto>>();

            try
            {
                var result = _mapper.Map<List<ArticleDto>>
                    (
                        await _context.Articles.ToListAsync()
                    );

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
                var result = _mapper.Map<ArticleDto>(await _context.Articles.FirstAsync(a => a.Url == url));

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

        public async Task<ServiceResponse<ArticleDto>> GetArticleById(int id)
        {
            ServiceResponse<ArticleDto> response = new ServiceResponse<ArticleDto>();
            try
            {
                var result = _mapper.Map<ArticleDto>(await _context.Articles.FindAsync(id));

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

        public async Task<ServiceResponse<ArticleDto>> UpdateArticle(ArticleDto article)
        {
            ServiceResponse<ArticleDto> response = new ServiceResponse<ArticleDto>();
            try
            {
                var result = await _context.Articles.FirstOrDefaultAsync(a => a.Id == article.Id);
                if(result == null || article == null)
                    throw new Exception("Article in database or ArticleDto is null");

                result = _mapper.Map(article, result);

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<ArticleDto>(result);
            }
            catch (Exception ex)
            {

                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<ArticleDto>> CreatedArticle(ArticleDto article)
        {
            ServiceResponse<ArticleDto> response = new ServiceResponse<ArticleDto>();
            try
            {
                var newArticle = _mapper.Map<Article>(article);
                if (newArticle == null)
                    throw new Exception("New Article is not exist");

                _context.Articles.Add(newArticle);
                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<ArticleDto>(newArticle);
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
