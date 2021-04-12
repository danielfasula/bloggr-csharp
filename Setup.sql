-- CREATE TABLE profiles
-- (
--     id VARCHAR(255) NOT NULL,
--     email VARCHAR(255) NOT NULL,
--     name VARCHAR(255),
--     picture VARCHAR(255),

--     PRIMARY KEY(id)
-- );

-- DROP TABLE blogs;

-- CREATE TABLE blogs
-- (
--     id int AUTO_INCREMENT,
--     title VARCHAR(255) NOT NULL,
--     body VARCHAR(255) NOT NULL,
--     imgUrl VARCHAR(255),
--     published TINYINT,
--     creatorId VARCHAR(255),

--     PRIMARY KEY (id),

--     FOREIGN KEY (creatorId)
--         REFERENCES profiles (id)
--         ON DELETE CASCADE
-- );

-- CREATE TABLE comments
-- (
--     id int AUTO_INCREMENT,
--     creatorId VARCHAR(255),
--     body VARCHAR(255) NOT NULL,
--     blog int,

--     PRIMARY KEY (id),

--         FOREIGN KEY (creatorId)
--             REFERENCES profiles (id)
--             ON DELETE CASCADE,

--         FOREIGN KEY (blog)
--             REFERENCES blogs (id)
--             ON DELETE CASCADE

-- )
