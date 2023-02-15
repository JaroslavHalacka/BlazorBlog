using Microsoft.EntityFrameworkCore;
using System;

namespace BlazorBlog.Services.ArticleService
{
    public class ArticleService : IArticleService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<DataContext> _dbContextFactory;

        public List<ArticleDto> AllArticleDto { get; set; } = new List<ArticleDto>();

        public ArticleService(DataContext context, IMapper mapper, IDbContextFactory<DataContext> dbContextFactory)
        {
            _context = context;
            _mapper = mapper;
            _dbContextFactory = dbContextFactory;
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

                using DataContext dataContext = _dbContextFactory.CreateDbContext();


                var result = _mapper.Map<ArticleDto>(await dataContext.Articles
                                                                    .Include(a => a.Categories)
                                                                    .FirstOrDefaultAsync(a => a.Id == id));

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
                if (result == null || article == null)
                    throw new Exception("Article in database or ArticleDto is null");

                result.Url= article.Url;
                result.Title= article.Title;
                result.NameForMenu = article.NameForMenu;
                result.Description= article.Description;
                result.Content= article.Content;
                result.IsPublished= article.IsPublished;
                result.IsDeleted= article.IsDeleted;

                await _context.SaveChangesAsync();

                response = await GetArticleById(article.Id);
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
        public async Task<ServiceResponse<ArticleDto>> InsertPossibleCategory(int categoryId, int idArticle)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            var article = await _context.Articles
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(a => a.Id == idArticle);

            article.Categories.Add(category);
            await _context.SaveChangesAsync();

            var response = await _context.Articles
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(a => a.Id == idArticle);


            return new ServiceResponse<ArticleDto> { Data = _mapper.Map<ArticleDto>(response) };
        }
        public async Task<ServiceResponse<ArticleDto>> RemovePossibleCategory(int categoryId, int idArticle)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            var article = await _context.Articles
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(a => a.Id == idArticle);

            article.Categories.Remove(category);
            await _context.SaveChangesAsync();

            var response = await _context.Articles
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(a => a.Id == idArticle);


            return new ServiceResponse<ArticleDto> { Data = _mapper.Map<ArticleDto>(response) };
        }

        public async Task<ServiceResponse<List<ArticleDto>>> SearchArticles(string searchText)
        {
            ServiceResponse<List<ArticleDto>> response = new ServiceResponse<List<ArticleDto>>();

            try
            {
                var result = _mapper.Map<List<ArticleDto>>
                    (
                        await GetArticlesBySearchText(searchText)
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

        private async Task<List<Article>> GetArticlesBySearchText(string searchText)
        {
            return await _context.Articles
                                    .Where(a => a.Title.ToLower().Contains(searchText.ToLower()) ||
                                            a.Description.ToLower().Contains(searchText.ToLower()) ||
                                            a.Content.ToLower().Contains(searchText.ToLower()) &&
                                            a.IsPublished && !a.IsDeleted)
                                    .ToListAsync();
        }

        public async Task<ServiceResponse<List<string>>> GetArticleSearchSuggestions(string searchText)
        {
            var articles = await GetArticlesBySearchText(searchText);
            List<string> response = new List<string>();

            if(articles != null)
            {
                foreach (var article in articles)
                {
                    if (article.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        response.Add(article.Title);
                    }

                    if (article.Description != null)
                    {
                        var punctuation = article.Description.Where(char.IsPunctuation)
                            .Distinct().ToArray();
                        var words = article.Description.Split()
                            .Select(s => s.Trim(punctuation));
                        foreach (var word in words)
                        {
                            if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !response.Contains(word))
                            {
                                response.Add(word);
                            }
                        }
                    }
                    //if (article.Content != null)
                    //{
                    //    var punctuation = article.Content.Where(char.IsPunctuation)
                    //        .Distinct().ToArray();
                    //    var words = article.Content.Split()
                    //        .Select(s => s.Trim(punctuation));
                    //    foreach (var word in words)
                    //    {
                    //        if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !response.Contains(word))
                    //        {
                    //            response.Add(word);
                    //        }
                    //    }
                    //}
                }
            }
            return new ServiceResponse<List<string>> { Data = response};
        }


    }
}
