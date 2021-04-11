using System;
using System.Collections.Generic;
using System.Data;
using bloggr_csharp.Models;
using Dapper;

namespace bloggr_csharp.Repositories
{
    public class CommentsRepository
    {

        private readonly IDbConnection _db;

        public CommentsRepository(IDbConnection db)
        {
            _db = db;
        }

        internal IEnumerable<Comment> GetByBlogId(int id)
        {
            string sql = @"
            SELECT
            c.*,
            b.*
            FROM comments c
            JOIN blogs b on c.blog = b.id
            WHERE blog = @id;
            ";
            return _db.Query<Comment, Blog, Comment>(sql, (comment, blog) =>
            {
                comment.Blog = blog.Id;
                return comment;
            }, new { id }, splitOn: "id");
        }
    }
}