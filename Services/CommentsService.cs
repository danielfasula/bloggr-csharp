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

        internal Comment GetById(int id)
        {
            Comment data = _repo.GetById(id);
            if (data == null)
            {
                throw new Exception("INVALID ID");
            }
            return data;
        }

        internal IEnumerable<Comment> GetCommentsByBlogId(int id)
        {
            return _repo.GetByBlogId(id);
        }

        internal IEnumerable<Comment> GetCommentsByProfileId(string id)
        {
            return _repo.GetByProfileId(id);
        }

        internal Comment Create(Comment newComment)
        {
            return _repo.Create(newComment);
        }

        internal Comment Edit(Comment updated)
        {
            Comment data = GetById(updated.Id);
            data.Body = updated.Body != null ? updated.Body : data.Body;

            return _repo.Edit(data);
        }

        internal string Delete(int id, string userId)
        {
            GetById(id);
            _repo.Delete(id, userId);
            return "Comment Deleted";
        }
    }
}