﻿namespace BlazorBlog
{
    public class AutoMapperProfile :Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Article, ArticleDto>();
        }
    }
}