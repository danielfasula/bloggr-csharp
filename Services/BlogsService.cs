using System;
using System.Collections;
using System.Collections.Generic;
using bloggr_csharp.Models;
using bloggr_csharp.Repositories;

namespace bloggr_csharp.Services
{
    public class BlogsService
    {
        private readonly BlogsRepository _repo;

        public BlogsService(BlogsRepository repo)
        {
            _repo = repo;
        }

        internal IEnumerable<Blog> GetBlogsByProfileId(string id)
        {
            return _repo.GetByProfileId(id);
        }

        internal IEnumerable<Blog> GetAll()
        {
            return _repo.GetAll();
        }

        internal Blog GetById(int id)
        {
            Blog data = _repo.GetById(id);
            if (data == null)
            {
                throw new Exception("Invalid ID");
            }
            return data;
        }

        internal Blog Create(Blog newBlog)
        {
            return _repo.Create(newBlog);
        }

        internal Blog Edit(Blog updated)
        {
            Blog data = GetById(updated.Id);
            data.Title = updated.Title != null ? updated.Title : data.Title;
            data.Body = updated.Body != null ? updated.Body : data.Body;
            data.imgUrl = updated.imgUrl != null ? updated.imgUrl : data.imgUrl;
            data.published = updated.published != null ? updated.published : data.published;

            return _repo.Edit(data);
        }

        internal string Delete(int id, string userId)
        {
            GetById(id);
            _repo.Delete(id, userId);
            return "Blog Deleted";
        }
    }
}