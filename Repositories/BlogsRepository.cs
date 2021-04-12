using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using bloggr_csharp.Models;
using Dapper;

namespace bloggr_csharp.Repositories
{
    public class BlogsRepository
    {
        private readonly IDbConnection _db;

        public BlogsRepository(IDbConnection db)
        {
            _db = db;
        }
        internal IEnumerable<Blog> GetByProfileId(string id)
        {
            string sql = @"
            SELECT
            b.*,
            p.*
            FROM blogs b
            JOIN profiles p ON b.creatorId = p.id
            WHERE creatorId = @id;
            ";
            return _db.Query<Blog, Profile, Blog>(sql, (blog, profile) =>
            {
                blog.CreatorId = profile.Id;
                return blog;
            }, new { id }, splitOn: "id");
        }

        internal Blog GetById(int id)
        {
            string sql = @"
            SELECT
            b.*,
            p.*
            FROM blogs b
            JOIN profiles p ON b.creatorId = p.id
            WHERE b.id = @id;
            ";
            return _db.Query<Blog, Profile, Blog>(sql, (blog, profile) =>
            {
                blog.Creator = profile;
                return blog;
            }, new { id }, splitOn: "id").FirstOrDefault();
        }

        internal IEnumerable<Blog> GetAll()
        {
            string sql = @"
            SELECT
            b.*,
            p.*
            FROM blogs b
            JOIN profiles p ON b.creatorId = p.id
            ";
            return _db.Query<Blog, Profile, Blog>(sql, (blog, profile) =>
            {
                blog.Creator = profile;
                return blog;
            }, splitOn: "id");
        }

        // internal Blog GetById(int id)
        // {
        //     string sql = @"
        //     SELECT * FROM blogs WHERE id = @id;
        //     ";
        //     return _db.QueryFirstOrDefault<Blog>(sql, new { id });
        // }
        internal Blog Create(Blog newBlog)
        {
            string sql = @"
            INSERT INTO blogs
            (id, title, body, imgUrl, published, creatorId)
            VALUES
            (@Id, @Title, @Body, @ImgUrl, @Published, @CreatorId);
            SELECT LAST_INSERT_ID();
            ";
            int id = _db.ExecuteScalar<int>(sql, newBlog);
            newBlog.Id = id;
            return newBlog;
        }

        internal Blog Edit(Blog data)
        {
            string sql = @"
            UPDATE blogs
            SET
                title = @Title,
                body = @Body,
                imgUrl = @imgUrl,
                published = @published
            WHERE id = @Id;
            SELECT * FROM blogs WHERE id = @Id;
            ";
            Blog returnBlog = _db.QueryFirstOrDefault<Blog>(sql, data);
            return returnBlog;
        }

        internal void Delete(int id, string userId)
        {
            string sql = @"
            DELETE FROM blogs WHERE id = @id AND creatorId = @userId LIMIT 1;
            ";
            _db.Execute(sql, new { id, userId });
        }
    }
}