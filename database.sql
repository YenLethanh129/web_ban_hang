CREATE TABLE tokens
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    token VARCHAR(255) UNIQUE NOT NULL,
    token_type VARCHAR(50) NOT NULL,
    experation_date DATETIME,
    revoked TINYINT(1) NOT NULL,
    expired TINYINT(1) NOT NULL,
    user_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE social_accounts
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    provider VARCHAR(20) NOT NULL COMMENT 'facebook, google, twitter, etc',
    provider_id VARCHAR(50) NOT NULL,
    email VARCHAR(150) NOT NULL,
    name VARCHAR(100) NOT NULL,
    user_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE categories
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL DEFAULT '' COMMENT 'Category name',
);

CREATE TABLE products(
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255) NOT NULL DEFAULT '' COMMENT 'Product name',
    price FLOAT NOT NULL CHECK (price >= 0) COMMENT 'Product price',
    thumbnail VARCHAR(255) DEFAULT '',
    description TEXT DEFAULT '' COMMENT 'Product description',
    created_at DATETIME,
    updated_at DATETIME,
    category_id INT,
    FOREIGN KEY (category_id) REFERENCES categories(id) 
);

ALTER TABLE users ADD COLUMN role_id INT;

CREATE TABLE roles
(
    id INT PRIMARY KEY,
    name VARCHAR(20) NOT NULL
);

ALTER TABLE users ADD FOREIGN KEY (role_id) REFERENCES roles(id);


CREATE TABLE orders
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT NOT NULL,
    fullname VARCHAR(100) DEFAULT '',
    email VARCHAR(100 DEFAULT '',
    phone_number VARCHAR(20) NOT NULL,
    address VARCHAR(200) NOT NULL,
    note VARCHAR(200) DEFAULT '',
    order_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(20),
    total_money FLOAT CHECK(total_money >= 0),
    FOREIGN KEY (user_id) REFERENCES users(id)
);

ALTER TABLE orders ADD COLUMN shiping_method VARCHAR(100);
ALTER TABLE orders ADD COLUMN shiping_address VARCHAR(200);
ALTER TABLE orders ADD COLUMN shiping_date DATE;
ALTER TABLE orders ADD COLUMN tracking_number VARCHAR(100);
ALTER TABLE orders ADD COLUMN payment_method VARCHAR(100);

ALTER TABLE orders ADD COLUMN active TINYINT(1);
ALTER TABLE orders
MODIFY COLUMN status ENUM('pending', 'processing', 'shipping', 'delivered', 'cancelled');

CREATE TABLE order_details
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    order_id INT,
    product_id INT,
    number_of_products INT CHECK(number_of_products > 0),
    price FLOAT CHECK(price >= 0),
    total_money FLOAT CHECK(total_money >= 0),
    color VARCHAR(20) DEFAULT '',
    FOREIGN KEY (order_id) REFERENCES orders(id),
    FOREIGN KEY (product_id) REFERENCES products(id)
);

CREATE TABLE product_images
(
    id INT PRIMARY KEY AUTO_INCREMENT,
    product_id INT,
    CONSTRAINT product_images_product_id_foreign
        FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE
);
