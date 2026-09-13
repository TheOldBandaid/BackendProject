USE appDB;

CREATE TABLE IF NOT EXISTS users (
    ID INT NOT NULL AUTO_INCREMENT,
    name VARCHAR(20) NOT NULL,
    surname VARCHAR(40) NOT NULL,
    PRIMARY KEY (ID)
);

INSERT INTO users (name, surname)
VALUES
    ('Alex', 'Rover'),
    ('Bob', 'Marley'),
    ('Kate', 'Yandson'),
    ('Lilo', 'Black');