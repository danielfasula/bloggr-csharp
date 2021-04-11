using System;
using System.Collections.Generic;
using bloggr_csharp.Models;
using bloggr_csharp.Repositories;

namespace bloggr_csharp.Services
{
    public class CommentsService
    {
        private readonly CommentsRepository _repo;

        public CommentsService(CommentsRepository repo)
        {
            _repo = repo;
        }

        internal IEnumerable<Comment> GetCommentsByBlogId(int id)
        {
            return _repo.GetByBlogId(id);
        }
    }
}