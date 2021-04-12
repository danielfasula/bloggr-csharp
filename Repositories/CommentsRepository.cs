using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        internal Comment GetById(int id)
        {
            string sql = @"
            SELECT
            c.*,
            p.*
            FROM comments c
            JOIN profiles p ON c.creatorId = p.id
            WHERE c.id = @id;
            ";
            return _db.Query<Comment, Profile, Comment>(sql, (comment, profile) =>
            {
                comment.Creator = profile;
                return comment;
            }, new { id }, splitOn: "id").FirstOrDefault();
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

        internal Comment Create(Comment newComment)
        {
            string sql = @"
            INSERT INTO comments
            (id, creatorId, body, blog)
            VALUES
            (@Id, @CreatorId, @Body, @Blog);
            SELECT LAST_INSERT_ID(); 
            ";
            int id = _db.ExecuteScalar<int>(sql, newComment);
            newComment.Id = id;
            return newComment;
        }

        internal IEnumerable<Comment> GetByProfileId(string id)
        {
            string sql = @"
            SELECT
            c.*,
            p.*
            FROM comments c
            JOIN profiles p on c.creatorId = p.id
            WHERE creatorId = @id;
            ";
            return _db.Query<Comment, Profile, Comment>(sql, (comment, profile) =>
            {
                comment.CreatorId = profile.Id;
                return comment;
            }, new { id }, splitOn: "id");
        }
        internal Comment Edit(Comment data)
        {
            string sql = @"
            UPDATE comments
            SET
                body = @Body
            WHERE id = @Id;
            SELECT * FROM comments WHERE id = @Id;
            ";
            Comment returnComment = _db.QueryFirstOrDefault<Comment>(sql, data);
            return returnComment;
        }
        internal void Delete(int id, string userId)
        {
            string sql = @"
            DELETE FROM comments WHERE id = @id AND creatorId = @userId LIMIT 1;
            ";
            _db.Execute(sql, new { id, userId });
        }
    }
}