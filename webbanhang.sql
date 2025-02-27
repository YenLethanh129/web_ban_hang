-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: localhost
-- Thời gian đã tạo: Th2 26, 2025 lúc 04:54 PM
-- Phiên bản máy phục vụ: 8.0.31
-- Phiên bản PHP: 7.4.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `webbanhang`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `categories`
--

CREATE TABLE `categories` (
  `id` int NOT NULL,
  `name` varchar(100) COLLATE utf8mb4_general_ci NOT NULL DEFAULT '' COMMENT 'Category name'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `categories`
--

INSERT INTO `categories` (`id`, `name`) VALUES
(1, 'Xoài'),
(2, 'Khoai mì'),
(3, 'Khoai Lang');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `orders`
--

CREATE TABLE `orders` (
  `id` int NOT NULL,
  `user_id` int NOT NULL,
  `fullname` varchar(100) COLLATE utf8mb4_general_ci DEFAULT '',
  `email` varchar(100) COLLATE utf8mb4_general_ci DEFAULT '',
  `phone_number` varchar(20) COLLATE utf8mb4_general_ci NOT NULL,
  `address` varchar(200) COLLATE utf8mb4_general_ci NOT NULL,
  `note` varchar(200) COLLATE utf8mb4_general_ci DEFAULT '',
  `order_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `status` enum('pending','processing','shipping','delivered','cancelled') COLLATE utf8mb4_general_ci DEFAULT NULL,
  `total_money` float DEFAULT NULL,
  `shipping_method` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `shipping_address` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `shipping_date` date DEFAULT NULL,
  `tracking_number` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `payment_method` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `payment_status` enum('pending','processing','shipping','delivered','cancelled') CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `is_active` tinyint(1) DEFAULT NULL,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL
) ;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `order_details`
--

CREATE TABLE `order_details` (
  `id` int NOT NULL,
  `order_id` int DEFAULT NULL,
  `product_id` int DEFAULT NULL,
  `number_of_products` int DEFAULT NULL,
  `price` float DEFAULT NULL,
  `total_money` float DEFAULT NULL,
  `color` varchar(20) COLLATE utf8mb4_general_ci DEFAULT ''
) ;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `products`
--

CREATE TABLE `products` (
  `id` int NOT NULL,
  `name` varchar(255) COLLATE utf8mb4_general_ci NOT NULL DEFAULT '' COMMENT 'Product name',
  `price` float NOT NULL COMMENT 'Product price',
  `thumbnail` varchar(255) COLLATE utf8mb4_general_ci DEFAULT '',
  `description` text COLLATE utf8mb4_general_ci,
  `created_at` datetime DEFAULT NULL,
  `updated_at` datetime DEFAULT NULL,
  `category_id` int DEFAULT NULL
) ;

--
-- Đang đổ dữ liệu cho bảng `products`
--

INSERT INTO `products` (`id`, `name`, `price`, `thumbnail`, `description`, `created_at`, `updated_at`, `category_id`) VALUES
(1, 'Khoang lang 18cm', 5000, 'https://picsum.photos/seed/picsum/200/300', 'Khoang lang giong Gia Lai', '2024-12-21 13:24:12', '2024-12-21 14:07:49', 3),
(2, 'Intelligent Wooden Bottle', 633535000, 'https://picsum.photos/seed/picsum/200/300', 'Qui iusto nesciunt minus nemo nihil provident.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(3, 'Khoang lang 18cm', 5000, 'https://picsum.photos/seed/picsum/200/300', 'Khoang lang giong Gia Lai', '2024-12-21 13:24:12', '2024-12-21 14:07:57', 3),
(5, 'Enormous Linen Clock', 558617000, 'https://picsum.photos/seed/picsum/200/300', 'Quo suscipit voluptatibus qui in aut vero.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(6, 'Ergonomic Linen Chair', 639063000, 'https://picsum.photos/seed/picsum/200/300', 'Distinctio iusto rem doloribus a ipsam iste natus.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(7, 'Mediocre Leather Bench', 775281000, 'https://picsum.photos/seed/picsum/200/300', 'Recusandae eveniet ut enim delectus.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(8, 'Heavy Duty Steel Bench', 750875000, 'https://picsum.photos/seed/picsum/200/300', 'Veritatis nesciunt et autem dolore necessitatibus sed.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(9, 'Enormous Iron Computer', 17856700, 'https://picsum.photos/seed/picsum/200/300', 'A neque ea quisquam.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(10, 'Aerodynamic Bronze Lamp', 27996100, 'https://picsum.photos/seed/picsum/200/300', 'Delectus dolor vero debitis magnam non cum.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(11, 'Awesome Bronze Pants', 885505000, 'https://picsum.photos/seed/picsum/200/300', 'Itaque molestiae quo rerum fugiat quas.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(12, 'Awesome Marble Chair', 525025000, 'https://picsum.photos/seed/picsum/200/300', 'Inventore voluptatem sit qui suscipit odio.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(13, 'Incredible Plastic Lamp', 296793000, 'https://picsum.photos/seed/picsum/200/300', 'Ducimus iusto sint sit ratione provident repellat mollitia.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(14, 'Small Silk Clock', 602546000, 'https://picsum.photos/seed/picsum/200/300', 'Autem est ad quae excepturi rerum.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(15, 'Synergistic Silk Bench', 21490900, 'https://picsum.photos/seed/picsum/200/300', 'Doloribus molestias cupiditate et minus.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(16, 'Awesome Wool Bag', 558971000, 'https://picsum.photos/seed/picsum/200/300', 'Corrupti vitae corporis omnis vel qui est et.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(17, 'Enormous Aluminum Knife', 903875000, 'https://picsum.photos/seed/picsum/200/300', 'Deserunt consequuntur ut porro numquam harum qui.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(18, 'Aerodynamic Wool Wallet', 631172000, 'https://picsum.photos/seed/picsum/200/300', 'Ut aliquid soluta quis nobis architecto.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(19, 'Fantastic Granite Shoes', 440236000, 'https://picsum.photos/seed/picsum/200/300', 'Eligendi dolorem itaque.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(20, 'Enormous Cotton Shoes', 881967000, 'https://picsum.photos/seed/picsum/200/300', 'Aut unde qui placeat.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(21, 'Synergistic Aluminum Bench', 146026000, 'https://picsum.photos/seed/picsum/200/300', 'Et amet iure et.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(22, 'Aerodynamic Marble Clock', 418461000, 'https://picsum.photos/seed/picsum/200/300', 'Aut placeat corrupti unde cum libero maiores aut.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(23, 'Mediocre Rubber Coat', 699375000, 'https://picsum.photos/seed/picsum/200/300', 'Ducimus rerum eius officia omnis deserunt dignissimos.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(24, 'Practical Paper Shoes', 183539000, 'https://picsum.photos/seed/picsum/200/300', 'Dolor sit enim.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(25, 'Durable Aluminum Chair', 816978000, 'https://picsum.photos/seed/picsum/200/300', 'Id voluptatum voluptate molestiae totam quisquam nihil.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(26, 'Mediocre Iron Bench', 129726000, 'https://picsum.photos/seed/picsum/200/300', 'Quasi in et iure delectus.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(27, 'Small Cotton Keyboard', 596681000, 'https://picsum.photos/seed/picsum/200/300', 'Possimus nobis unde inventore quo.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(28, 'Synergistic Cotton Hat', 586973000, 'https://picsum.photos/seed/picsum/200/300', 'Rerum provident possimus iste velit tenetur.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(29, 'Gorgeous Linen Computer', 199815000, 'https://picsum.photos/seed/picsum/200/300', 'Accusantium dolor accusamus consequatur aliquid consequatur unde temporibus.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(30, 'Practical Cotton Shirt', 6993610, 'https://picsum.photos/seed/picsum/200/300', 'Veritatis distinctio doloremque dicta nihil a omnis.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(31, 'Sleek Granite Shirt', 885833000, 'https://picsum.photos/seed/picsum/200/300', 'Aperiam totam nihil dolorem expedita ut maxime.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(32, 'Enormous Steel Bench', 628476000, 'https://picsum.photos/seed/picsum/200/300', 'Aliquam similique illo ipsum.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(33, 'Fantastic Plastic Keyboard', 876779000, 'https://picsum.photos/seed/picsum/200/300', 'Aliquam facere ut dignissimos.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(34, 'Enormous Paper Watch', 491828000, 'https://picsum.photos/seed/picsum/200/300', 'Consequuntur pariatur ullam dolorem velit reiciendis.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(35, 'Sleek Aluminum Keyboard', 585113000, 'https://picsum.photos/seed/picsum/200/300', 'Eveniet iusto laudantium quasi adipisci.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(36, 'Aerodynamic Paper Gloves', 29566400, 'https://picsum.photos/seed/picsum/200/300', 'Animi dolores placeat.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(37, 'Ergonomic Concrete Table', 423444000, 'https://picsum.photos/seed/picsum/200/300', 'Quam fugit autem accusamus veniam non.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(38, 'Ergonomic Paper Chair', 684431000, 'https://picsum.photos/seed/picsum/200/300', 'Veniam delectus tempora aut ea expedita atque et.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(39, 'Gorgeous Cotton Watch', 931948000, 'https://picsum.photos/seed/picsum/200/300', 'Nisi et architecto minima sed.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(40, 'Heavy Duty Rubber Gloves', 891724000, 'https://picsum.photos/seed/picsum/200/300', 'Nisi itaque ea.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(41, 'Synergistic Linen Wallet', 183376000, 'https://picsum.photos/seed/picsum/200/300', 'Ut quae explicabo consequatur non recusandae quod.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(42, 'Practical Granite Chair', 821621000, 'https://picsum.photos/seed/picsum/200/300', 'Consequatur sed error blanditiis.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(43, 'Awesome Concrete Bag', 800558000, 'https://picsum.photos/seed/picsum/200/300', 'Non similique facere.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(44, 'Intelligent Marble Table', 491029000, 'https://picsum.photos/seed/picsum/200/300', 'Aut hic optio sapiente aut.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(45, 'Heavy Duty Aluminum Table', 713592000, 'https://picsum.photos/seed/picsum/200/300', 'Dolor quia dolor delectus.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(46, 'Small Silk Gloves', 211534000, 'https://picsum.photos/seed/picsum/200/300', 'Numquam ut sint molestias optio.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(47, 'Incredible Concrete Lamp', 557980000, 'https://picsum.photos/seed/picsum/200/300', 'Recusandae et necessitatibus iusto veniam explicabo.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(48, 'Enormous Linen Watch', 471417000, 'https://picsum.photos/seed/picsum/200/300', 'Occaecati porro molestias et.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(49, 'Incredible Plastic Pants', 28557200, 'https://picsum.photos/seed/picsum/200/300', 'Sed voluptates vel quis.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(50, 'Heavy Duty Concrete Car', 365157000, 'https://picsum.photos/seed/picsum/200/300', 'Consequatur beatae recusandae.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(51, 'Fantastic Wool Shoes', 691907000, 'https://picsum.photos/seed/picsum/200/300', 'Nesciunt sequi aspernatur qui sit.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(52, 'Intelligent Steel Car', 402345000, 'https://picsum.photos/seed/picsum/200/300', 'Velit quia molestias quis quis iure.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(53, 'Gorgeous Paper Chair', 291156000, 'https://picsum.photos/seed/picsum/200/300', 'Nostrum voluptatem quos.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(54, 'Incredible Steel Bottle', 577587000, 'https://picsum.photos/seed/picsum/200/300', 'Beatae cupiditate rem alias iure placeat eum dolore.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(55, 'Fantastic Linen Lamp', 248835000, 'https://picsum.photos/seed/picsum/200/300', 'Laudantium et cupiditate laudantium iure adipisci voluptas.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(56, 'Durable Plastic Bottle', 994307000, 'https://picsum.photos/seed/picsum/200/300', 'Laudantium rerum occaecati magnam molestiae autem quia quia.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(57, 'Lightweight Marble Hat', 43460100, 'https://picsum.photos/seed/picsum/200/300', 'Ex ea ut aut quo.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(58, 'Heavy Duty Granite Wallet', 642628000, 'https://picsum.photos/seed/picsum/200/300', 'Mollitia similique voluptatem reprehenderit eos molestiae temporibus enim.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(59, 'Incredible Leather Lamp', 602593000, 'https://picsum.photos/seed/picsum/200/300', 'Impedit aliquam architecto exercitationem nemo.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(60, 'Sleek Wooden Hat', 426211000, 'https://picsum.photos/seed/picsum/200/300', 'Doloribus distinctio sint et adipisci est voluptates.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(61, 'Awesome Paper Watch', 849970000, 'https://picsum.photos/seed/picsum/200/300', 'Aliquam quas officiis voluptas tempora autem.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(62, 'Enormous Paper Wallet', 941076000, 'https://picsum.photos/seed/picsum/200/300', 'Saepe occaecati accusantium cupiditate nemo qui in cumque.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(63, 'Gorgeous Wool Table', 925237000, 'https://picsum.photos/seed/picsum/200/300', 'Amet dolor in illo sit doloremque sit inventore.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(64, 'Heavy Duty Wool Wallet', 244819000, 'https://picsum.photos/seed/picsum/200/300', 'Tempore est doloribus.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(65, 'Intelligent Copper Watch', 575518000, 'https://picsum.photos/seed/picsum/200/300', 'Est cupiditate voluptatum qui qui tempore consequatur quasi.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(66, 'Aerodynamic Iron Wallet', 772496000, 'https://picsum.photos/seed/picsum/200/300', 'Commodi nisi quis voluptas cumque asperiores.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(67, 'Aerodynamic Marble Chair', 147508000, 'https://picsum.photos/seed/picsum/200/300', 'Pariatur aut inventore.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(68, 'Durable Marble Car', 906730000, 'https://picsum.photos/seed/picsum/200/300', 'Distinctio aut sed recusandae quisquam vero doloremque eum.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(69, 'Awesome Cotton Lamp', 452136000, 'https://picsum.photos/seed/picsum/200/300', 'Praesentium et debitis et error.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(70, 'Awesome Concrete Bench', 196445000, 'https://picsum.photos/seed/picsum/200/300', 'Veritatis consequatur rerum sed sequi.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(71, 'Rustic Concrete Bench', 401555000, 'https://picsum.photos/seed/picsum/200/300', 'Rerum hic consectetur accusantium.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(72, 'Rustic Wooden Plate', 449468000, 'https://picsum.photos/seed/picsum/200/300', 'Deserunt non optio voluptas.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(73, 'Sleek Leather Gloves', 626275000, 'https://picsum.photos/seed/picsum/200/300', 'Quia provident necessitatibus.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(74, 'Synergistic Cotton Gloves', 643361000, 'https://picsum.photos/seed/picsum/200/300', 'Doloribus quibusdam quia in.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(75, 'Gorgeous Aluminum Bottle', 465021000, 'https://picsum.photos/seed/picsum/200/300', 'Fugiat ut itaque consequuntur architecto qui.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(76, 'Small Marble Computer', 294774000, 'https://picsum.photos/seed/picsum/200/300', 'Voluptatum veniam cumque illum.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(77, 'Practical Copper Bottle', 112838000, 'https://picsum.photos/seed/picsum/200/300', 'Est pariatur maiores distinctio harum.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(78, 'Synergistic Silk Knife', 118678000, 'https://picsum.photos/seed/picsum/200/300', 'Beatae aperiam sunt id iure aut.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(79, 'Small Plastic Knife', 616834000, 'https://picsum.photos/seed/picsum/200/300', 'Sint quia ab.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(80, 'Lightweight Marble Chair', 939981000, 'https://picsum.photos/seed/picsum/200/300', 'Sit quas quasi.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(81, 'Rustic Wooden Table', 535602000, 'https://picsum.photos/seed/picsum/200/300', 'Quaerat consectetur excepturi esse voluptatem veniam quis commodi.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(82, 'Durable Silk Chair', 592439000, 'https://picsum.photos/seed/picsum/200/300', 'Esse voluptate beatae et aperiam.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(83, 'Sleek Granite Car', 877507000, 'https://picsum.photos/seed/picsum/200/300', 'Sed sit tempore.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(84, 'Aerodynamic Granite Shirt', 832630000, 'https://picsum.photos/seed/picsum/200/300', 'Dolorum sit corporis possimus quidem provident.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(85, 'Small Leather Computer', 752123000, 'https://picsum.photos/seed/picsum/200/300', 'Itaque exercitationem facilis excepturi consequatur consectetur ipsa voluptatem.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(86, 'Sleek Granite Bench', 708679000, 'https://picsum.photos/seed/picsum/200/300', 'Ut labore aliquam dolores.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(87, 'Rustic Bronze Bench', 283931000, 'https://picsum.photos/seed/picsum/200/300', 'Voluptatem omnis ea voluptate et vero eligendi quo.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(88, 'Intelligent Steel Shirt', 517081000, 'https://picsum.photos/seed/picsum/200/300', 'Deleniti enim impedit est molestiae ut.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(89, 'Incredible Aluminum Bottle', 299680000, 'https://picsum.photos/seed/picsum/200/300', 'Odit dolorem rerum consequuntur doloribus ut praesentium.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(90, 'Enormous Silk Knife', 974260000, 'https://picsum.photos/seed/picsum/200/300', 'Corrupti est totam iste aut.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(91, 'Small Concrete Pants', 307345000, 'https://picsum.photos/seed/picsum/200/300', 'Nostrum officiis quo aspernatur vel.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(92, 'Awesome Steel Hat', 521936000, 'https://picsum.photos/seed/picsum/200/300', 'Veritatis omnis iure molestiae occaecati alias ut eaque.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(93, 'Small Paper Hat', 108917000, 'https://picsum.photos/seed/picsum/200/300', 'Et animi sed culpa.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(94, 'Rustic Paper Wallet', 151274000, 'https://picsum.photos/seed/picsum/200/300', 'Dolor aut voluptas vel deleniti.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(95, 'Lightweight Concrete Knife', 773485000, 'https://picsum.photos/seed/picsum/200/300', 'Non ut est eveniet eum aut ratione.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(96, 'Aerodynamic Linen Car', 547593000, 'https://picsum.photos/seed/picsum/200/300', 'Et voluptatem ut aut.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(97, 'Sleek Wool Shoes', 856658000, 'https://picsum.photos/seed/picsum/200/300', 'Porro sapiente esse.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(98, 'Small Iron Shirt', 17803600, 'https://picsum.photos/seed/picsum/200/300', 'Sit nam voluptas.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(99, 'Durable Aluminum Clock', 319154000, 'https://picsum.photos/seed/picsum/200/300', 'Est dolores eos nobis quia dolore inventore.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(100, 'Intelligent Copper Shirt', 244741000, 'https://picsum.photos/seed/picsum/200/300', 'Ut voluptatem alias nulla accusantium expedita repellat.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(101, 'Intelligent Bronze Shirt', 251786000, 'https://picsum.photos/seed/picsum/200/300', 'Exercitationem quae enim ullam saepe ut laboriosam.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(102, 'Small Linen Gloves', 929744000, 'https://picsum.photos/seed/picsum/200/300', 'Nobis officia non rerum tempora cumque asperiores vel.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(103, 'Awesome Paper Keyboard', 784418000, 'https://picsum.photos/seed/picsum/200/300', 'Labore delectus nihil magnam voluptas blanditiis fuga.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(104, 'Ergonomic Linen Shirt', 296718000, 'https://picsum.photos/seed/picsum/200/300', 'Quis ut esse architecto explicabo dolorem velit consequatur.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(105, 'Gorgeous Paper Keyboard', 196763000, 'https://picsum.photos/seed/picsum/200/300', 'Harum eum id pariatur quisquam.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(106, 'Synergistic Rubber Chair', 516289000, 'https://picsum.photos/seed/picsum/200/300', 'Ut ab sapiente.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(107, 'Rustic Plastic Plate', 538941000, 'https://picsum.photos/seed/picsum/200/300', 'Vitae ut ullam est autem non.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(108, 'Aerodynamic Granite Coat', 331207000, 'https://picsum.photos/seed/picsum/200/300', 'Beatae sit ullam eius explicabo iusto.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(109, 'Synergistic Paper Wallet', 545890000, 'https://picsum.photos/seed/picsum/200/300', 'Inventore voluptates laboriosam corporis corrupti ut repudiandae.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(110, 'Awesome Steel Coat', 32968900, 'https://picsum.photos/seed/picsum/200/300', 'Alias ex reprehenderit accusantium et adipisci aspernatur quas.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(111, 'Durable Granite Shirt', 792090000, 'https://picsum.photos/seed/picsum/200/300', 'Accusantium reprehenderit a velit a nihil harum.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(112, 'Enormous Iron Keyboard', 977543000, 'https://picsum.photos/seed/picsum/200/300', 'Accusantium eum sunt excepturi eum vitae enim.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(113, 'Intelligent Rubber Gloves', 868791000, 'https://picsum.photos/seed/picsum/200/300', 'Neque expedita tempora cum quibusdam eveniet accusamus dolores.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(114, 'Practical Concrete Pants', 408410000, 'https://picsum.photos/seed/picsum/200/300', 'Et rerum magnam.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(115, 'Mediocre Aluminum Clock', 57495400, 'https://picsum.photos/seed/picsum/200/300', 'Cupiditate fugiat et esse hic.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(116, 'Lightweight Wool Plate', 784058000, 'https://picsum.photos/seed/picsum/200/300', 'Enim ex beatae adipisci.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(117, 'Rustic Wooden Computer', 906681000, 'https://picsum.photos/seed/picsum/200/300', 'Hic neque quia rerum accusantium.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(118, 'Incredible Linen Bag', 887415000, 'https://picsum.photos/seed/picsum/200/300', 'Incidunt officia et deserunt facere sit ratione.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(119, 'Sleek Iron Wallet', 901184000, 'https://picsum.photos/seed/picsum/200/300', 'Mollitia qui voluptas ex autem.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(120, 'Awesome Iron Keyboard', 959853000, 'https://picsum.photos/seed/picsum/200/300', 'Nesciunt nam perspiciatis.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(121, 'Incredible Concrete Knife', 687470000, 'https://picsum.photos/seed/picsum/200/300', 'Laboriosam ullam culpa nisi.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(122, 'Small Steel Plate', 657111000, 'https://picsum.photos/seed/picsum/200/300', 'Doloremque voluptatum dignissimos.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(123, 'Durable Cotton Plate', 347281000, 'https://picsum.photos/seed/picsum/200/300', 'Optio officiis aperiam quia vel est doloribus natus.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(124, 'Aerodynamic Wooden Hat', 909403000, 'https://picsum.photos/seed/picsum/200/300', 'Ut non praesentium eum.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(125, 'Practical Cotton Plate', 18820600, 'https://picsum.photos/seed/picsum/200/300', 'Sed enim doloremque.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(126, 'Lightweight Steel Hat', 279552000, 'https://picsum.photos/seed/picsum/200/300', 'Ab eos nisi.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 1),
(127, 'Aerodynamic Plastic Hat', 308582000, 'https://picsum.photos/seed/picsum/200/300', 'Quibusdam aut ut voluptas ipsum nesciunt.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(128, 'Enormous Silk Bench', 354261000, 'https://picsum.photos/seed/picsum/200/300', 'Earum aut fugiat aut vel quod omnis.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(129, 'Rustic Concrete Plate', 560505000, 'https://picsum.photos/seed/picsum/200/300', 'Et veritatis tempore ullam.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(130, 'Lightweight Rubber Bag', 802426000, 'https://picsum.photos/seed/picsum/200/300', 'Iure quod dicta maxime fugit nisi explicabo.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(131, 'Intelligent Aluminum Bottle', 985388000, 'https://picsum.photos/seed/picsum/200/300', 'Aliquam tempora veritatis.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(132, 'Ergonomic Plastic Wallet', 741114000, 'https://picsum.photos/seed/picsum/200/300', 'Eos qui dolorem iure.', '2024-12-21 13:24:12', '2024-12-21 13:24:12', 2),
(133, 'Fantastic Concrete Bag', 11039200, 'https://picsum.photos/seed/picsum/200/300', 'Aperiam vel vel.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(134, 'Durable Cotton Shirt', 776605000, 'https://picsum.photos/seed/picsum/200/300', 'Exercitationem esse voluptatibus nobis.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(135, 'Fantastic Plastic Bottle', 401825000, 'https://picsum.photos/seed/picsum/200/300', 'Ut itaque ipsam sit ut labore quasi soluta.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(136, 'Incredible Concrete Hat', 757959000, 'https://picsum.photos/seed/picsum/200/300', 'Sit omnis facilis aut neque officia maiores quas.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(137, 'Synergistic Linen Bench', 316878000, 'https://picsum.photos/seed/picsum/200/300', 'Ipsa quibusdam cumque dignissimos.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(138, 'Fantastic Aluminum Lamp', 915045000, 'https://picsum.photos/seed/picsum/200/300', 'Possimus placeat necessitatibus in ratione aliquam veritatis.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(139, 'Rustic Wooden Wallet', 304518000, 'https://picsum.photos/seed/picsum/200/300', 'Qui eos sit numquam cumque provident.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(140, 'Fantastic Rubber Lamp', 112723000, 'https://picsum.photos/seed/picsum/200/300', 'Quia dolores tempore nam doloremque sint aut sint.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(141, 'Sleek Wool Car', 521888000, 'https://picsum.photos/seed/picsum/200/300', 'Eos ducimus omnis accusantium ea et ipsum eius.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(142, 'Heavy Duty Concrete Watch', 533397000, 'https://picsum.photos/seed/picsum/200/300', 'Molestiae nisi qui eaque.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(143, 'Ergonomic Silk Shoes', 442508000, 'https://picsum.photos/seed/picsum/200/300', 'Quo qui in accusamus inventore.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(144, 'Practical Silk Car', 646665000, 'https://picsum.photos/seed/picsum/200/300', 'Libero et voluptatem eum consectetur quas eligendi assumenda.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(145, 'Synergistic Silk Chair', 484364000, 'https://picsum.photos/seed/picsum/200/300', 'Velit voluptatem corporis commodi maxime expedita voluptatem minima.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(146, 'Mediocre Plastic Plate', 709275000, 'https://picsum.photos/seed/picsum/200/300', 'A ipsam error officia error.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(147, 'Aerodynamic Bronze Clock', 43796100, 'https://picsum.photos/seed/picsum/200/300', 'Fugit nihil esse et eius veritatis modi et.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(148, 'Synergistic Aluminum Shirt', 171635000, 'https://picsum.photos/seed/picsum/200/300', 'Aliquam sunt magnam illo aliquid.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(149, 'Lightweight Granite Shoes', 903429000, 'https://picsum.photos/seed/picsum/200/300', 'Est accusantium maiores consequatur eum omnis et aut.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(150, 'Awesome Granite Wallet', 892287000, 'https://picsum.photos/seed/picsum/200/300', 'Et harum distinctio sunt hic sit.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(151, 'Small Paper Bench', 483602000, 'https://picsum.photos/seed/picsum/200/300', 'Et adipisci sint et sint.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(152, 'Synergistic Concrete Knife', 748286000, 'https://picsum.photos/seed/picsum/200/300', 'Voluptatem quam dolore dolorem enim provident assumenda totam.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(153, 'Lightweight Leather Bag', 256957000, 'https://picsum.photos/seed/picsum/200/300', 'Minus dignissimos ut sapiente enim.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(154, 'Aerodynamic Wooden Bench', 319869000, 'https://picsum.photos/seed/picsum/200/300', 'Aut sunt magnam deleniti qui eum voluptas nesciunt.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(155, 'Aerodynamic Steel Watch', 923877000, 'https://picsum.photos/seed/picsum/200/300', 'Mollitia animi repellat laudantium itaque.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(156, 'Durable Aluminum Shoes', 47622000, 'https://picsum.photos/seed/picsum/200/300', 'Et neque ipsum voluptas incidunt quaerat.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(157, 'Rustic Paper Shirt', 992708000, 'https://picsum.photos/seed/picsum/200/300', 'Pariatur aliquam dolores mollitia.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(158, 'Synergistic Iron Car', 533505000, 'https://picsum.photos/seed/picsum/200/300', 'Et repudiandae et.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(159, 'Heavy Duty Steel Clock', 23389400, 'https://picsum.photos/seed/picsum/200/300', 'Inventore accusamus ut possimus nihil illo unde esse.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(160, 'Rustic Concrete Shoes', 4421750, 'https://picsum.photos/seed/picsum/200/300', 'Quae sint aut quia repudiandae est dolorum.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(161, 'Aerodynamic Plastic Bottle', 748152000, 'https://picsum.photos/seed/picsum/200/300', 'Officia voluptatibus ipsum qui at atque.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(162, 'Ergonomic Granite Bench', 224858000, 'https://picsum.photos/seed/picsum/200/300', 'Fuga ad et sed facilis nihil quis impedit.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(163, 'Mediocre Cotton Pants', 896357000, 'https://picsum.photos/seed/picsum/200/300', 'Illo doloribus delectus omnis nesciunt quibusdam quidem.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(164, 'Heavy Duty Steel Keyboard', 736488000, 'https://picsum.photos/seed/picsum/200/300', 'Illo eos sequi amet dolorem et optio.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(165, 'Mediocre Rubber Bag', 843417000, 'https://picsum.photos/seed/picsum/200/300', 'Dicta nihil impedit tempora.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(166, 'Lightweight Bronze Hat', 59387900, 'https://picsum.photos/seed/picsum/200/300', 'Culpa sed sed autem fugit.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(167, 'Fantastic Iron Watch', 876789000, 'https://picsum.photos/seed/picsum/200/300', 'Esse illum debitis excepturi rerum et.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(168, 'Practical Linen Shirt', 738722000, 'https://picsum.photos/seed/picsum/200/300', 'Soluta perferendis ipsam eaque temporibus.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(169, 'Ergonomic Wool Clock', 898279000, 'https://picsum.photos/seed/picsum/200/300', 'Necessitatibus ipsa veritatis mollitia et consectetur dolore.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(170, 'Incredible Bronze Hat', 74442600, 'https://picsum.photos/seed/picsum/200/300', 'Animi ut qui nihil sapiente omnis veritatis.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(171, 'Heavy Duty Marble Gloves', 616055000, 'https://picsum.photos/seed/picsum/200/300', 'Praesentium dolores inventore et accusamus sit.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(172, 'Small Steel Chair', 880396000, 'https://picsum.photos/seed/picsum/200/300', 'Inventore est facere quas tempore exercitationem.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(173, 'Incredible Bronze Computer', 694738000, 'https://picsum.photos/seed/picsum/200/300', 'Et eum velit sed temporibus occaecati.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(174, 'Incredible Copper Hat', 166198000, 'https://picsum.photos/seed/picsum/200/300', 'Ut modi ex commodi.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(175, 'Sleek Aluminum Hat', 881279000, 'https://picsum.photos/seed/picsum/200/300', 'Rerum in assumenda.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(176, 'Small Copper Lamp', 522498000, 'https://picsum.photos/seed/picsum/200/300', 'Suscipit quisquam perspiciatis et et possimus.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(177, 'Durable Iron Gloves', 365430000, 'https://picsum.photos/seed/picsum/200/300', 'Culpa molestiae officia ipsum.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(178, 'Aerodynamic Rubber Wallet', 385445000, 'https://picsum.photos/seed/picsum/200/300', 'Pariatur sed optio esse.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(179, 'Aerodynamic Rubber Bag', 755729000, 'https://picsum.photos/seed/picsum/200/300', 'Est maxime facere voluptatum veniam.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(180, 'Synergistic Steel Wallet', 64808100, 'https://picsum.photos/seed/picsum/200/300', 'Alias facilis odit consequatur.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(181, 'Awesome Copper Shoes', 109982000, 'https://picsum.photos/seed/picsum/200/300', 'Sequi voluptas deleniti vitae quibusdam hic.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(182, 'Small Copper Clock', 964703000, 'https://picsum.photos/seed/picsum/200/300', 'Praesentium est alias saepe recusandae reprehenderit aut.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(183, 'Lightweight Silk Watch', 230334000, 'https://picsum.photos/seed/picsum/200/300', 'Totam sit ipsa possimus ut dolores reprehenderit quia.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(184, 'Lightweight Wooden Pants', 545149000, 'https://picsum.photos/seed/picsum/200/300', 'Qui voluptatem porro.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(185, 'Mediocre Granite Pants', 219261000, 'https://picsum.photos/seed/picsum/200/300', 'Saepe sit et a aspernatur laudantium.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(186, 'Sleek Leather Knife', 493858000, 'https://picsum.photos/seed/picsum/200/300', 'Molestiae suscipit est sint.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(187, 'Synergistic Marble Bottle', 293952000, 'https://picsum.photos/seed/picsum/200/300', 'Qui modi delectus non et commodi.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(188, 'Synergistic Wooden Pants', 895120000, 'https://picsum.photos/seed/picsum/200/300', 'Architecto sit aut deleniti id.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(189, 'Fantastic Linen Pants', 993898000, 'https://picsum.photos/seed/picsum/200/300', 'Ipsum non magni.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(190, 'Small Concrete Coat', 288453000, 'https://picsum.photos/seed/picsum/200/300', 'Vero voluptatem possimus.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(191, 'Fantastic Aluminum Knife', 570911000, 'https://picsum.photos/seed/picsum/200/300', 'Rerum commodi suscipit.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(192, 'Mediocre Bronze Hat', 812610000, 'https://picsum.photos/seed/picsum/200/300', 'Autem perspiciatis nobis.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(193, 'Gorgeous Linen Car', 288081000, 'https://picsum.photos/seed/picsum/200/300', 'Architecto numquam omnis dolor.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(194, 'Sleek Wool Coat', 355842000, 'https://picsum.photos/seed/picsum/200/300', 'Dolore vel culpa quia sit.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(195, 'Practical Bronze Bottle', 114602, 'https://picsum.photos/seed/picsum/200/300', 'Dignissimos labore incidunt.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(196, 'Intelligent Plastic Shirt', 617699000, 'https://picsum.photos/seed/picsum/200/300', 'Cum accusantium est ipsum.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(197, 'Enormous Paper Computer', 275384000, 'https://picsum.photos/seed/picsum/200/300', 'Sed enim veniam.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(198, 'Mediocre Wool Bottle', 989103000, 'https://picsum.photos/seed/picsum/200/300', 'Minus odio in illum ut at.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(199, 'Durable Rubber Keyboard', 609841000, 'https://picsum.photos/seed/picsum/200/300', 'Ex debitis reiciendis vel ullam magnam consequatur quam.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(200, 'Mediocre Cotton Bench', 762214000, 'https://picsum.photos/seed/picsum/200/300', 'Non et amet labore.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(201, 'Sleek Plastic Shirt', 298375000, 'https://picsum.photos/seed/picsum/200/300', 'Labore neque minima sed unde assumenda cumque dolore.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(202, 'Durable Linen Car', 992144000, 'https://picsum.photos/seed/picsum/200/300', 'Atque asperiores iure voluptatem aut ut.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(203, 'Incredible Plastic Bench', 395684000, 'https://picsum.photos/seed/picsum/200/300', 'Vitae aut accusantium perferendis quis accusamus illo.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(204, 'Aerodynamic Copper Shirt', 767731000, 'https://picsum.photos/seed/picsum/200/300', 'Accusamus eos quia asperiores.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(205, 'Gorgeous Wool Computer', 806425000, 'https://picsum.photos/seed/picsum/200/300', 'Beatae pariatur et.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(206, 'Awesome Wool Lamp', 158912000, 'https://picsum.photos/seed/picsum/200/300', 'Deleniti quo ut.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(207, 'Fantastic Iron Keyboard', 955936000, 'https://picsum.photos/seed/picsum/200/300', 'Ipsum corrupti ea provident qui quis.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(208, 'Awesome Plastic Hat', 123666000, 'https://picsum.photos/seed/picsum/200/300', 'Cum est eius est voluptates est corrupti autem.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(209, 'Synergistic Iron Clock', 263716000, 'https://picsum.photos/seed/picsum/200/300', 'Exercitationem ut rerum nihil aut.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(210, 'Ergonomic Rubber Computer', 565119000, 'https://picsum.photos/seed/picsum/200/300', 'Et optio voluptate aut quidem architecto quisquam maiores.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(211, 'Aerodynamic Bronze Hat', 546386000, 'https://picsum.photos/seed/picsum/200/300', 'Nulla illo aut ex.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(212, 'Awesome Rubber Hat', 391822000, 'https://picsum.photos/seed/picsum/200/300', 'Ratione eum architecto.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(213, 'Awesome Steel Watch', 47588700, 'https://picsum.photos/seed/picsum/200/300', 'Adipisci et incidunt.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(214, 'Lightweight Iron Shoes', 836335000, 'https://picsum.photos/seed/picsum/200/300', 'Iusto et suscipit est similique qui aut.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(215, 'Heavy Duty Aluminum Chair', 386916000, 'https://picsum.photos/seed/picsum/200/300', 'Officiis corrupti accusamus temporibus qui velit occaecati rerum.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(216, 'Durable Marble Shoes', 862720000, 'https://picsum.photos/seed/picsum/200/300', 'Reiciendis cum aut ut facilis magni cupiditate.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(217, 'Lightweight Rubber Bottle', 230174000, 'https://picsum.photos/seed/picsum/200/300', 'Molestiae a excepturi est nisi in accusamus impedit.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(218, 'Intelligent Iron Watch', 564460000, 'https://picsum.photos/seed/picsum/200/300', 'Perferendis perspiciatis nulla.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(219, 'Gorgeous Linen Bag', 131157000, 'https://picsum.photos/seed/picsum/200/300', 'Accusamus ullam assumenda nemo possimus quaerat qui dolores.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(220, 'Mediocre Leather Computer', 94236200, 'https://picsum.photos/seed/picsum/200/300', 'Voluptates voluptatum aut laboriosam optio sequi.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(221, 'Mediocre Cotton Shirt', 157422000, 'https://picsum.photos/seed/picsum/200/300', 'Dolores ut at dignissimos molestiae earum praesentium.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(222, 'Incredible Cotton Clock', 335290000, 'https://picsum.photos/seed/picsum/200/300', 'Et consequuntur et sequi vitae corrupti.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(223, 'Incredible Concrete Bottle', 14266300, 'https://picsum.photos/seed/picsum/200/300', 'Sint non eos id.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(224, 'Heavy Duty Leather Bottle', 752654000, 'https://picsum.photos/seed/picsum/200/300', 'Facilis accusantium pariatur.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(225, 'Rustic Rubber Keyboard', 150820000, 'https://picsum.photos/seed/picsum/200/300', 'Dicta ullam voluptatem consequatur blanditiis quia nam iste.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(226, 'Intelligent Leather Gloves', 238758000, 'https://picsum.photos/seed/picsum/200/300', 'Molestias asperiores sapiente officiis temporibus aut dolores aspernatur.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(227, 'Fantastic Steel Watch', 965067000, 'https://picsum.photos/seed/picsum/200/300', 'Repellendus vero repellendus reprehenderit sunt.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(228, 'Small Steel Car', 643478000, 'https://picsum.photos/seed/picsum/200/300', 'Laboriosam sit molestiae recusandae consequatur id eveniet.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(229, 'Lightweight Aluminum Bottle', 587297000, 'https://picsum.photos/seed/picsum/200/300', 'Ea quod quaerat.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(230, 'Mediocre Copper Coat', 193258000, 'https://picsum.photos/seed/picsum/200/300', 'Totam iure dolores.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(231, 'Heavy Duty Cotton Chair', 667913000, 'https://picsum.photos/seed/picsum/200/300', 'Rerum ad aut occaecati impedit minima nulla ullam.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(232, 'Ergonomic Wooden Car', 522584000, 'https://picsum.photos/seed/picsum/200/300', 'Consectetur rerum voluptatibus odio officia veritatis.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(233, 'Incredible Cotton Shirt', 123794000, 'https://picsum.photos/seed/picsum/200/300', 'Sequi eum sed quas harum.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(234, 'Aerodynamic Copper Gloves', 376722000, 'https://picsum.photos/seed/picsum/200/300', 'Sint consequatur deleniti et atque cumque.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(235, 'Small Cotton Hat', 940733000, 'https://picsum.photos/seed/picsum/200/300', 'Illum modi aspernatur temporibus eum fugit non voluptas.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(236, 'Fantastic Granite Hat', 664818000, 'https://picsum.photos/seed/picsum/200/300', 'Sunt est voluptatem.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(237, 'Awesome Silk Pants', 331780000, 'https://picsum.photos/seed/picsum/200/300', 'Ut enim ducimus.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(238, 'Sleek Leather Bag', 632566000, 'https://picsum.photos/seed/picsum/200/300', 'Id vitae rerum quod sunt laudantium laborum asperiores.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(239, 'Durable Cotton Lamp', 207365000, 'https://picsum.photos/seed/picsum/200/300', 'Voluptatum ut ad non voluptatem.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(240, 'Awesome Copper Pants', 905625000, 'https://picsum.photos/seed/picsum/200/300', 'Et similique omnis architecto.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(241, 'Incredible Iron Gloves', 210083000, 'https://picsum.photos/seed/picsum/200/300', 'Qui nesciunt ullam excepturi illo qui veniam.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(242, 'Fantastic Wooden Watch', 734388000, 'https://picsum.photos/seed/picsum/200/300', 'Aut in quae alias.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(243, 'Lightweight Marble Car', 265044000, 'https://picsum.photos/seed/picsum/200/300', 'Dolores voluptas natus sint.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(244, 'Fantastic Concrete Hat', 14514600, 'https://picsum.photos/seed/picsum/200/300', 'Ducimus omnis enim alias voluptas hic unde et.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(245, 'Synergistic Iron Knife', 815870000, 'https://picsum.photos/seed/picsum/200/300', 'Et tempore debitis dolorum voluptatem.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(246, 'Awesome Rubber Keyboard', 837000000, 'https://picsum.photos/seed/picsum/200/300', 'Quasi qui quidem distinctio iure.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(247, 'Rustic Cotton Bottle', 26992700, 'https://picsum.photos/seed/picsum/200/300', 'Quo est ut repellat.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(248, 'Durable Wool Chair', 542606000, 'https://picsum.photos/seed/picsum/200/300', 'Aut maiores nihil.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(249, 'Intelligent Steel Gloves', 329498000, 'https://picsum.photos/seed/picsum/200/300', 'Iusto fugiat delectus commodi ullam earum quia.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(250, 'Gorgeous Paper Plate', 142937000, 'https://picsum.photos/seed/picsum/200/300', 'Nisi maiores aut ea exercitationem fugit nesciunt.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(251, 'Small Rubber Chair', 312980000, 'https://picsum.photos/seed/picsum/200/300', 'Nisi enim adipisci voluptatibus qui qui.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(252, 'Enormous Concrete Plate', 403861000, 'https://picsum.photos/seed/picsum/200/300', 'Repellendus est qui quia et et consequatur amet.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(253, 'Practical Steel Gloves', 676543000, 'https://picsum.photos/seed/picsum/200/300', 'Magni voluptas suscipit dolorem.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(254, 'Practical Leather Computer', 104897000, 'https://picsum.photos/seed/picsum/200/300', 'Fuga dolore illum debitis.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(255, 'Synergistic Copper Chair', 613142000, 'https://picsum.photos/seed/picsum/200/300', 'Labore ipsum est optio.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(256, 'Awesome Granite Plate', 870526000, 'https://picsum.photos/seed/picsum/200/300', 'Nobis dolor dignissimos ratione ratione veritatis.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(257, 'Synergistic Aluminum Lamp', 582539000, 'https://picsum.photos/seed/picsum/200/300', 'Numquam rerum odit non sit consectetur.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(258, 'Intelligent Leather Lamp', 25227500, 'https://picsum.photos/seed/picsum/200/300', 'Ipsam qui error qui nam.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(259, 'Heavy Duty Steel Shirt', 667454000, 'https://picsum.photos/seed/picsum/200/300', 'In id quae fugiat sed veniam unde.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(260, 'Aerodynamic Leather Shirt', 421243000, 'https://picsum.photos/seed/picsum/200/300', 'Laudantium velit qui.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(261, 'Sleek Silk Pants', 683934000, 'https://picsum.photos/seed/picsum/200/300', 'Et ex non.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(262, 'Ergonomic Concrete Bag', 229542000, 'https://picsum.photos/seed/picsum/200/300', 'Iusto facilis quas voluptatem sequi.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(263, 'Gorgeous Granite Bench', 835417000, 'https://picsum.photos/seed/picsum/200/300', 'Consequatur dolor id mollitia provident et illo omnis.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(264, 'Practical Aluminum Bag', 95308300, 'https://picsum.photos/seed/picsum/200/300', 'Nihil cum voluptates consectetur sed non.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(265, 'Durable Cotton Knife', 920678000, 'https://picsum.photos/seed/picsum/200/300', 'Repellendus aut iure quibusdam.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(266, 'Practical Leather Table', 139218000, 'https://picsum.photos/seed/picsum/200/300', 'Voluptatibus modi eligendi blanditiis repudiandae.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(267, 'Gorgeous Cotton Lamp', 182528000, 'https://picsum.photos/seed/picsum/200/300', 'Nihil deserunt dicta consequatur ratione et.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(268, 'Practical Rubber Car', 504300000, 'https://picsum.photos/seed/picsum/200/300', 'Esse assumenda aliquam qui delectus neque.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(269, 'Heavy Duty Copper Knife', 117175000, 'https://picsum.photos/seed/picsum/200/300', 'Corporis quaerat et eum.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(270, 'Heavy Duty Wool Hat', 126171000, 'https://picsum.photos/seed/picsum/200/300', 'Ea quae ipsum iure.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(271, 'Durable Wooden Bottle', 582500000, 'https://picsum.photos/seed/picsum/200/300', 'Est ea occaecati.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(272, 'Sleek Bronze Hat', 930719000, 'https://picsum.photos/seed/picsum/200/300', 'Laudantium blanditiis inventore iste reprehenderit animi.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(273, 'Awesome Plastic Watch', 644974000, 'https://picsum.photos/seed/picsum/200/300', 'Sint sapiente quo delectus aut qui veniam cumque.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(274, 'Intelligent Concrete Keyboard', 584260000, 'https://picsum.photos/seed/picsum/200/300', 'Odit quidem facilis.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(275, 'Small Concrete Computer', 3987340, 'https://picsum.photos/seed/picsum/200/300', 'Sapiente ab quis esse similique voluptatum aut.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(276, 'Awesome Aluminum Bottle', 530961000, 'https://picsum.photos/seed/picsum/200/300', 'Non blanditiis ipsum.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(277, 'Heavy Duty Linen Bench', 796054000, 'https://picsum.photos/seed/picsum/200/300', 'Amet et id.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(278, 'Aerodynamic Linen Coat', 214139000, 'https://picsum.photos/seed/picsum/200/300', 'Sit modi est.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(279, 'Ergonomic Concrete Knife', 236018000, 'https://picsum.photos/seed/picsum/200/300', 'Minus ut ullam ratione.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(280, 'Enormous Paper Car', 413556000, 'https://picsum.photos/seed/picsum/200/300', 'Corrupti minus autem doloremque veniam est ipsum voluptas.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(281, 'Rustic Silk Wallet', 539058000, 'https://picsum.photos/seed/picsum/200/300', 'Laborum et eos eveniet eos et sint illum.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(282, 'Incredible Wool Shirt', 8748020, 'https://picsum.photos/seed/picsum/200/300', 'Voluptatem est autem ut autem qui.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(283, 'Ergonomic Copper Clock', 234618000, 'https://picsum.photos/seed/picsum/200/300', 'Quia rem vitae sit et.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2);
INSERT INTO `products` (`id`, `name`, `price`, `thumbnail`, `description`, `created_at`, `updated_at`, `category_id`) VALUES
(284, 'Ergonomic Leather Computer', 612567000, 'https://picsum.photos/seed/picsum/200/300', 'Ut voluptatem vel.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(285, 'Enormous Silk Watch', 115940000, 'https://picsum.photos/seed/picsum/200/300', 'Non veritatis voluptas.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(286, 'Awesome Concrete Chair', 355018000, 'https://picsum.photos/seed/picsum/200/300', 'Repellat ducimus vitae hic expedita esse.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(287, 'Fantastic Leather Knife', 207189000, 'https://picsum.photos/seed/picsum/200/300', 'Quia occaecati alias.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(288, 'Gorgeous Linen Keyboard', 65442200, 'https://picsum.photos/seed/picsum/200/300', 'Animi atque delectus esse ad dolorum aut.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(289, 'Awesome Wool Keyboard', 606759000, 'https://picsum.photos/seed/picsum/200/300', 'Culpa voluptatibus doloribus et.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(290, 'Practical Iron Coat', 686488000, 'https://picsum.photos/seed/picsum/200/300', 'Omnis voluptas voluptatem minima.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 2),
(291, 'Heavy Duty Copper Plate', 190968000, 'https://picsum.photos/seed/picsum/200/300', 'Porro et dolores doloribus autem eligendi qui.', '2024-12-21 13:24:13', '2024-12-21 13:24:13', 1),
(292, 'Khoang lang 18cm', 5000, 'https://picsum.photos/seed/picsum/200/300', 'Khoang lang giong Gia Lai', '2025-01-10 18:05:11', '2025-01-10 18:05:11', 3),
(293, 'Khoang lang 18cm', 5000, 'https://picsum.photos/seed/picsum/200/300', 'Khoang lang giong Gia Lai', '2025-01-10 19:48:06', '2025-01-10 19:48:06', 3),
(294, 'Khoang lang 18cm', 5000, 'https://picsum.photos/seed/picsum/200/300', 'Khoang lang giong Gia Lai', '2025-01-11 18:00:51', '2025-01-11 18:00:51', 3),
(295, 'Khoang lang 18cm', 5000, 'https://picsum.photos/seed/picsum/200/300', 'Khoang lang giong Gia Lai', '2025-01-11 18:02:45', '2025-01-11 18:02:45', 3);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `product_images`
--

CREATE TABLE `product_images` (
  `id` int NOT NULL,
  `product_id` int DEFAULT NULL,
  `image_url` varchar(300) COLLATE utf8mb4_general_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `product_images`
--

INSERT INTO `product_images` (`id`, `product_id`, `image_url`) VALUES
(11, 23, 'www.derick-paucek.io'),
(12, 10, 'www.kendall-kub.com'),
(13, 79, 'www.domitila-lindgren.info'),
(14, 3, 'www.otto-collins.info'),
(15, 51, 'www.rafaela-hauck.org'),
(16, 99, 'www.gerard-robel.name'),
(17, 27, 'www.ellis-donnelly.info'),
(18, 72, 'www.leopoldo-reichel.biz'),
(19, 56, 'www.maud-runolfsdottir.org'),
(20, 68, 'www.lorenzo-reinger.co'),
(22, 48, 'www.hai-leuschke.com'),
(23, 78, 'www.tory-hilll.com'),
(24, 82, 'www.leroy-keeling.io'),
(25, 16, 'www.antione-schuster.info'),
(26, 77, 'www.elmira-schaden.co'),
(27, 95, 'www.mao-ernser.io'),
(28, 85, 'www.geri-funk.org'),
(29, 25, 'www.allan-zieme.com'),
(30, 8, 'www.darrell-gibson.biz'),
(31, 23, 'www.delores-senger.io'),
(32, 41, 'www.glen-howe.net'),
(33, 2, 'www.marya-grady.com'),
(34, 88, 'www.amiee-mohr.biz'),
(35, 13, 'www.guadalupe-kessler.name'),
(36, 37, 'www.tory-lynch.co'),
(37, 88, 'www.sammie-trantow.net'),
(38, 77, 'www.lemuel-quigley.name'),
(40, 14, 'https://picsum.photos/seed/picsum/200/300'),
(41, 3, 'https://picsum.photos/seed/picsum/200/300'),
(42, 9, 'https://picsum.photos/seed/picsum/200/300'),
(43, 7, 'https://picsum.photos/seed/picsum/200/300'),
(44, 7, 'https://picsum.photos/seed/picsum/200/300'),
(45, 15, 'https://picsum.photos/seed/picsum/200/300'),
(46, 15, 'https://picsum.photos/seed/picsum/200/300'),
(47, 14, 'https://picsum.photos/seed/picsum/200/300'),
(48, 8, 'https://picsum.photos/seed/picsum/200/300'),
(49, 17, 'https://picsum.photos/seed/picsum/200/300'),
(50, 8, 'https://picsum.photos/seed/picsum/200/300'),
(51, 8, 'https://picsum.photos/seed/picsum/200/300'),
(52, 13, 'https://picsum.photos/seed/picsum/200/300'),
(53, 15, 'https://picsum.photos/seed/picsum/200/300'),
(54, 16, 'https://picsum.photos/seed/picsum/200/300'),
(55, 15, 'https://picsum.photos/seed/picsum/200/300'),
(56, 8, 'https://picsum.photos/seed/picsum/200/300'),
(57, 7, 'https://picsum.photos/seed/picsum/200/300'),
(58, 11, 'https://picsum.photos/seed/picsum/200/300'),
(59, 15, 'https://picsum.photos/seed/picsum/200/300'),
(60, 6, 'https://picsum.photos/seed/picsum/200/300'),
(61, 11, 'https://picsum.photos/seed/picsum/200/300'),
(62, 16, 'https://picsum.photos/seed/picsum/200/300'),
(63, 16, 'https://picsum.photos/seed/picsum/200/300'),
(64, 9, 'https://picsum.photos/seed/picsum/200/300'),
(65, 17, 'https://picsum.photos/seed/picsum/200/300'),
(67, 12, 'https://picsum.photos/seed/picsum/200/300'),
(68, 14, 'https://picsum.photos/seed/picsum/200/300'),
(69, 11, 'https://picsum.photos/seed/picsum/200/300'),
(71, 7, 'https://picsum.photos/seed/picsum/200/300'),
(72, 2, 'https://picsum.photos/seed/picsum/200/300'),
(73, 12, 'https://picsum.photos/seed/picsum/200/300'),
(74, 11, 'https://picsum.photos/seed/picsum/200/300'),
(75, 18, 'https://picsum.photos/seed/picsum/200/300'),
(76, 5, 'https://picsum.photos/seed/picsum/200/300'),
(77, 10, 'https://picsum.photos/seed/picsum/200/300'),
(78, 13, 'https://picsum.photos/seed/picsum/200/300'),
(80, 8, 'https://picsum.photos/seed/picsum/200/300'),
(81, 1, 'https://picsum.photos/seed/picsum/200/300'),
(82, 6, 'https://picsum.photos/seed/picsum/200/300'),
(83, 17, 'https://picsum.photos/seed/picsum/200/300'),
(84, 5, 'https://picsum.photos/seed/picsum/200/300'),
(85, 2, 'https://picsum.photos/seed/picsum/200/300'),
(86, 6, 'https://picsum.photos/seed/picsum/200/300'),
(87, 6, 'https://picsum.photos/seed/picsum/200/300'),
(88, 9, 'https://picsum.photos/seed/picsum/200/300'),
(89, 6, 'https://picsum.photos/seed/picsum/200/300'),
(90, 8, 'https://picsum.photos/seed/picsum/200/300'),
(91, 5, 'https://picsum.photos/seed/picsum/200/300'),
(92, 10, 'https://picsum.photos/seed/picsum/200/300'),
(93, 13, 'https://picsum.photos/seed/picsum/200/300'),
(95, 3, 'https://picsum.photos/seed/picsum/200/300'),
(96, 2, 'https://picsum.photos/seed/picsum/200/300'),
(97, 18, 'https://picsum.photos/seed/picsum/200/300'),
(98, 13, 'https://picsum.photos/seed/picsum/200/300'),
(99, 16, 'https://picsum.photos/seed/picsum/200/300'),
(100, 2, 'https://picsum.photos/seed/picsum/200/300'),
(101, 6, 'https://picsum.photos/seed/picsum/200/300'),
(102, 9, 'https://picsum.photos/seed/picsum/200/300'),
(103, 9, 'https://picsum.photos/seed/picsum/200/300'),
(104, 13, 'https://picsum.photos/seed/picsum/200/300'),
(105, 8, 'https://picsum.photos/seed/picsum/200/300'),
(106, 2, 'https://picsum.photos/seed/picsum/200/300'),
(107, 17, 'https://picsum.photos/seed/picsum/200/300'),
(108, 2, 'https://picsum.photos/seed/picsum/200/300'),
(109, 17, 'https://picsum.photos/seed/picsum/200/300'),
(110, 10, 'https://picsum.photos/seed/picsum/200/300'),
(111, 17, 'https://picsum.photos/seed/picsum/200/300'),
(112, 11, 'https://picsum.photos/seed/picsum/200/300'),
(113, 11, 'https://picsum.photos/seed/picsum/200/300'),
(114, 18, 'https://picsum.photos/seed/picsum/200/300'),
(115, 10, 'https://picsum.photos/seed/picsum/200/300'),
(116, 16, 'https://picsum.photos/seed/picsum/200/300'),
(117, 6, 'https://picsum.photos/seed/picsum/200/300'),
(118, 5, 'https://picsum.photos/seed/picsum/200/300'),
(119, 2, 'https://picsum.photos/seed/picsum/200/300'),
(120, 10, 'https://picsum.photos/seed/picsum/200/300'),
(121, 16, 'https://picsum.photos/seed/picsum/200/300'),
(122, 12, 'https://picsum.photos/seed/picsum/200/300'),
(123, 13, 'https://picsum.photos/seed/picsum/200/300'),
(124, 6, 'https://picsum.photos/seed/picsum/200/300'),
(125, 15, 'https://picsum.photos/seed/picsum/200/300'),
(126, 12, 'https://picsum.photos/seed/picsum/200/300'),
(127, 12, 'https://picsum.photos/seed/picsum/200/300'),
(128, 5, 'https://picsum.photos/seed/picsum/200/300'),
(129, 2, 'https://picsum.photos/seed/picsum/200/300'),
(130, 11, 'https://picsum.photos/seed/picsum/200/300'),
(131, 5, 'https://picsum.photos/seed/picsum/200/300'),
(132, 8, 'https://picsum.photos/seed/picsum/200/300'),
(133, 8, 'https://picsum.photos/seed/picsum/200/300'),
(134, 13, 'https://picsum.photos/seed/picsum/200/300'),
(135, 13, 'https://picsum.photos/seed/picsum/200/300'),
(136, 10, 'https://picsum.photos/seed/picsum/200/300'),
(137, 5, 'https://picsum.photos/seed/picsum/200/300'),
(138, 16, 'https://picsum.photos/seed/picsum/200/300'),
(139, 2, 'https://picsum.photos/seed/picsum/200/300'),
(140, 6, 'https://picsum.photos/seed/picsum/200/300'),
(141, 17, 'https://picsum.photos/seed/picsum/200/300'),
(142, 10, 'https://picsum.photos/seed/picsum/200/300'),
(143, 18, 'https://picsum.photos/seed/picsum/200/300'),
(144, 6, 'https://picsum.photos/seed/picsum/200/300'),
(145, 6, 'https://picsum.photos/seed/picsum/200/300'),
(146, 7, 'https://picsum.photos/seed/picsum/200/300'),
(147, 12, 'https://picsum.photos/seed/picsum/200/300'),
(148, 18, 'https://picsum.photos/seed/picsum/200/300'),
(149, 1, 'https://picsum.photos/seed/picsum/200/300'),
(150, 2, 'https://picsum.photos/seed/picsum/200/300'),
(151, 16, 'https://picsum.photos/seed/picsum/200/300'),
(152, 3, 'https://picsum.photos/seed/picsum/200/300'),
(153, 9, 'https://picsum.photos/seed/picsum/200/300'),
(154, 17, 'https://picsum.photos/seed/picsum/200/300'),
(155, 8, 'https://picsum.photos/seed/picsum/200/300'),
(156, 3, 'https://picsum.photos/seed/picsum/200/300'),
(157, 8, 'https://picsum.photos/seed/picsum/200/300'),
(158, 10, 'https://picsum.photos/seed/picsum/200/300');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `roles`
--

CREATE TABLE `roles` (
  `id` int NOT NULL,
  `name` varchar(20) COLLATE utf8mb4_general_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `roles`
--

INSERT INTO `roles` (`id`, `name`) VALUES
(1, 'USER'),
(2, 'ADMIN');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `social_accounts`
--

CREATE TABLE `social_accounts` (
  `id` int NOT NULL,
  `provider` varchar(20) COLLATE utf8mb4_general_ci NOT NULL COMMENT 'facebook, google, twitter, etc',
  `provider_id` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  `email` varchar(150) COLLATE utf8mb4_general_ci NOT NULL,
  `name` varchar(100) COLLATE utf8mb4_general_ci NOT NULL,
  `user_id` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `tokens`
--

CREATE TABLE `tokens` (
  `id` int NOT NULL,
  `token` varchar(255) COLLATE utf8mb4_general_ci NOT NULL,
  `token_type` varchar(50) COLLATE utf8mb4_general_ci NOT NULL,
  `experation_date` datetime DEFAULT NULL,
  `revoked` tinyint(1) NOT NULL,
  `expired` tinyint(1) NOT NULL,
  `user_id` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `users`
--

CREATE TABLE `users` (
  `id` int NOT NULL,
  `fullname` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
  `phone_number` varchar(10) COLLATE utf8mb4_general_ci NOT NULL,
  `address` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
  `password` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '',
  `created_at` datetime NOT NULL,
  `updated_at` datetime NOT NULL,
  `is_active` tinyint(1) NOT NULL DEFAULT '1',
  `date_of_birth` date NOT NULL,
  `facebook_account_id` int NOT NULL DEFAULT '0',
  `google_account_id` int NOT NULL DEFAULT '0',
  `role_id` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `users`
--

INSERT INTO `users` (`id`, `fullname`, `phone_number`, `address`, `password`, `created_at`, `updated_at`, `is_active`, `date_of_birth`, `facebook_account_id`, `google_account_id`, `role_id`) VALUES
(1, 'Lê Thanh Yên', '0375440589', 'Tân Bình, HCM', '$2a$10$9a577rhCtAOgHpgPe4wZqef9I.A5cBDGtVkDk8MnwX3IYsPLxCBMi', '2024-12-21 15:16:26', '2024-12-21 15:16:26', 1, '2004-09-16', 0, 0, NULL),
(2, 'John Doe', '1234567890', '123 Elm Street, Springfield', '$2a$10$PPTBkBmjM677w5QsBSWl8u2KSbsmqem3K7zZf4cRMz.90GtFMLbpq', '2025-01-10 13:22:45', '2025-01-10 13:22:45', 0, '1985-10-25', 0, 0, 1),
(3, 'John Doe', '0375440580', '123 Elm Street, Springfield', '$2a$10$Xg6j0w/AGb3Aa9HBJBk0x.tkyFJ4irVqpRZh.TCFaJ2txeuTPYdp2', '2025-01-10 13:38:55', '2025-01-10 13:38:55', 0, '1985-10-25', 0, 0, 2),
(4, 'John Doe', '0375440541', '123 Elm Street, Springfield', '$2a$10$9a577rhCtAOgHpgPe4wZqef9I.A5cBDGtVkDk8MnwX3IYsPLxCBMi', '2025-01-10 18:03:06', '2025-01-10 18:03:06', 0, '1985-10-25', 0, 0, 1),
(17, 'Lê Thanh Yên', '0375440500', '688/91 Quang Trung, Go Vap', '$2a$10$yMYhApQw9bpdTqmv3JHqCu99wWKu8vYxNlOo.NI9X0hGCbxZ5rhdO', '2025-01-15 16:55:46', '2025-01-15 16:55:46', 0, '2004-09-16', 0, 0, 1),
(18, 'Lê Thanh Yên', '0375440501', '688/91 Quang Trung, Go Vap', '$2a$10$GQ1yA3MePZ4nBJ/mcUyTfeizp0VANK53W5pwqMMwq8wcKTZ1goWzW', '2025-01-15 16:59:21', '2025-01-15 16:59:21', 0, '2004-09-16', 0, 0, 1),
(19, 'Lê Thanh Yên', '0375440502', '688/91 Quang Trung, Go Vap', '$2a$10$Q6ugzxI5iz0DXZQ2qCMs4ebHIXzFVcO0DvL6vwjYuB9yjycAP21Oa', '2025-01-15 17:05:56', '2025-01-15 17:05:56', 0, '2004-09-16', 0, 0, 1),
(20, 'Lê Thanh Yên', '0375440503', '688/91 Quang Trung, Go Vap', '$2a$10$cItSRFcas6yBhFKKClfBCOu/0SWTWT/bXsIKMWsG9xjmm17l0l2I6', '2025-01-15 17:22:03', '2025-01-15 17:22:03', 0, '2004-09-16', 0, 0, 1),
(21, 'Lê Thanh Yên', '0375440504', '688/91 Quang Trung, Go Vap', '$2a$10$WlcAjuVc7Abt3TTigB/vjupg0DQdiM7iTeut1bDvVzOVCa8AP7vdG', '2025-01-15 17:23:57', '2025-01-15 17:23:57', 0, '2004-09-16', 0, 0, 1),
(22, 'Lê Thanh Yên', '0375440505', '17A Cong Hoa', '$2a$10$ZQmCbOexaV/V0sHOKkql0.jwoQiesyrBXxlPnBDFD6VDnZ.LXYaGq', '2025-01-15 17:38:02', '2025-01-15 17:38:02', 0, '2004-09-16', 0, 0, 1),
(23, 'Lê Thanh Yên', '0375440506', '688/91 Quang Trung, Go Vap', '$2a$10$IOwDWak9cuKtYCxrtA0Ll.xeZ42bty4I/daIWw20MP83by8LqVdV.', '2025-01-15 17:48:17', '2025-01-15 17:48:17', 0, '2004-09-16', 0, 0, 1);

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `categories`
--
ALTER TABLE `categories`
  ADD PRIMARY KEY (`id`);

--
-- Chỉ mục cho bảng `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`);

--
-- Chỉ mục cho bảng `order_details`
--
ALTER TABLE `order_details`
  ADD PRIMARY KEY (`id`),
  ADD KEY `order_id` (`order_id`),
  ADD KEY `product_id` (`product_id`);

--
-- Chỉ mục cho bảng `products`
--
ALTER TABLE `products`
  ADD PRIMARY KEY (`id`),
  ADD KEY `category_id` (`category_id`);

--
-- Chỉ mục cho bảng `product_images`
--
ALTER TABLE `product_images`
  ADD PRIMARY KEY (`id`),
  ADD KEY `product_images_product_id_foreign` (`product_id`);

--
-- Chỉ mục cho bảng `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`id`);

--
-- Chỉ mục cho bảng `social_accounts`
--
ALTER TABLE `social_accounts`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`);

--
-- Chỉ mục cho bảng `tokens`
--
ALTER TABLE `tokens`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `token` (`token`),
  ADD KEY `user_id` (`user_id`);

--
-- Chỉ mục cho bảng `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD KEY `role_id` (`role_id`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `categories`
--
ALTER TABLE `categories`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT cho bảng `orders`
--
ALTER TABLE `orders`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `order_details`
--
ALTER TABLE `order_details`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `products`
--
ALTER TABLE `products`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `product_images`
--
ALTER TABLE `product_images`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=160;

--
-- AUTO_INCREMENT cho bảng `social_accounts`
--
ALTER TABLE `social_accounts`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `tokens`
--
ALTER TABLE `tokens`
  MODIFY `id` int NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `users`
--
ALTER TABLE `users`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `orders`
--
ALTER TABLE `orders`
  ADD CONSTRAINT `orders_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

--
-- Các ràng buộc cho bảng `order_details`
--
ALTER TABLE `order_details`
  ADD CONSTRAINT `order_details_ibfk_1` FOREIGN KEY (`order_id`) REFERENCES `orders` (`id`),
  ADD CONSTRAINT `order_details_ibfk_2` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`);

--
-- Các ràng buộc cho bảng `products`
--
ALTER TABLE `products`
  ADD CONSTRAINT `products_ibfk_1` FOREIGN KEY (`category_id`) REFERENCES `categories` (`id`);

--
-- Các ràng buộc cho bảng `product_images`
--
ALTER TABLE `product_images`
  ADD CONSTRAINT `product_images_product_id_foreign` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `social_accounts`
--
ALTER TABLE `social_accounts`
  ADD CONSTRAINT `social_accounts_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

--
-- Các ràng buộc cho bảng `tokens`
--
ALTER TABLE `tokens`
  ADD CONSTRAINT `tokens_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`);

--
-- Các ràng buộc cho bảng `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`role_id`) REFERENCES `roles` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
