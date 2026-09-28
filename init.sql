CREATE DATABASE IF NOT EXISTS appDB;
CREATE USER IF NOT EXISTS 'user'@'%' IDENTIFIED BY 'password';
GRANT SELECT, UPDATE, INSERT, DELETE ON appDB.* TO 'user'@'%';
FLUSH PRIVILEGES;

USE appDB;
CREATE TABLE IF NOT EXISTS users (
    ID INT(11) NOT NULL AUTO_INCREMENT,
    name VARCHAR(20) NOT NULL,
    surname VARCHAR(40) NOT NULL,
    PRIMARY KEY (ID)
);

CREATE TABLE IF NOT EXISTS orders (
    ID INT(11) NOT NULL AUTO_INCREMENT,
    user_id INT(11) NOT NULL,
    product_name VARCHAR(50) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (ID),
    FOREIGN KEY (user_id) REFERENCES users(ID) ON DELETE CASCADE
);

INSERT INTO users (name, surname)
SELECT * FROM (SELECT 'Alex', 'Rover') AS tmp
WHERE NOT EXISTS (
    SELECT name FROM users WHERE name = 'Alex' AND surname = 'Rover'
) LIMIT 1;

INSERT INTO users (name, surname)
SELECT * FROM (SELECT 'Bob', 'Marley') AS tmp
WHERE NOT EXISTS (
    SELECT name FROM users WHERE name = 'Bob' AND surname = 'Marley'
) LIMIT 1;

INSERT INTO users (name, surname)
SELECT * FROM (SELECT 'Alex', 'Rover') AS tmp
WHERE NOT EXISTS (
    SELECT name FROM users WHERE name = 'Alex' AND surname = 'Rover'
) LIMIT 1;

INSERT INTO users (name, surname)
SELECT * FROM (SELECT 'Kate', 'Yandson') AS tmp
WHERE NOT EXISTS (
    SELECT name FROM users WHERE name = 'Kate' AND surname = 'Yandson'
) LIMIT 1;

INSERT INTO users (name, surname)
SELECT * FROM (SELECT 'Lilo', 'Black') AS tmp
WHERE NOT EXISTS (
    SELECT name FROM users WHERE name = 'Lilo' AND surname = 'Black'
) LIMIT 1;

INSERT INTO orders (user_id, product_name, price)
SELECT * FROM (SELECT 1, 'Pizza', 900.00) AS tmp
WHERE NOT EXISTS (SELECT 1 FROM orders WHERE product_name = 'Pizza') LIMIT 1;

INSERT INTO orders (user_id, product_name, price)
SELECT * FROM (SELECT 2, 'Juice', 300.00) AS tmp
WHERE NOT EXISTS (SELECT 1 FROM orders WHERE product_name = 'Juice') LIMIT 1;