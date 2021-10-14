USE db;
CREATE TABLE `Category` (
  `groupid` int NOT NULL AUTO_INCREMENT,
  `parentid` int DEFAULT NULL,
  `groupname` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`groupid`)
) ENGINE=InnoDB;

CREATE TABLE `Product` (
  `productid` int NOT NULL AUTO_INCREMENT,
  `productname` varchar(45) NOT NULL,
  `groupid` int NOT NULL,
  `createddate` datetime NOT NULL,
  `price` decimal(65,2) NOT NULL,
  `vatprice` decimal(65,2) NOT NULL,
  `vatpercentage` int NOT NULL,
  PRIMARY KEY (`productid`),
  KEY `groupid` (`groupid`),
  CONSTRAINT `groupid` FOREIGN KEY (`groupid`) REFERENCES `Category` (`groupid`)
);

CREATE TABLE `Shop` (
  `shopid` int NOT NULL AUTO_INCREMENT,
  `shopname` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`shopid`)
);

CREATE TABLE `ProductShop` (
  `productid` int NOT NULL,
  `shopid` int NOT NULL,
  KEY `productid` (`productid`),
  KEY `shopid` (`shopid`),
  CONSTRAINT `ProductShop_ibfk_1` FOREIGN KEY (`productid`) REFERENCES `Product` (`productid`),
  CONSTRAINT `ProductShop_ibfk_2` FOREIGN KEY (`shopid`) REFERENCES `Shop` (`shopid`)
);
INSERT INTO Shop (shopname)
VALUES ('Super Store');
INSERT INTO Shop (shopname)
VALUES ('Mega Store');
INSERT INTO Shop (shopname)
VALUES ('Hyper Store');
INSERT INTO Shop (shopname)
VALUES ('None');

INSERT INTO Category (parentid, groupname)
VALUES (0, "Foods");
INSERT INTO Category (parentid, groupname)
VALUES (0, "Kitchenware");
INSERT INTO Category (parentid, groupname)
VALUES (1, "Vegetables");
INSERT INTO Category (parentid, groupname)
VALUES (1, "Fruits");
INSERT INTO Category (parentid, groupname)
VALUES (2, "Home Aplliances");
INSERT INTO Category (parentid, groupname)
VALUES (2, "Untensils");

INSERT INTO Product (productname, groupid, createddate, price, vatprice, vatpercentage)
VALUES ("Grapes", 4, CURRENT_TIMESTAMP, 1, 1.25, 20);
INSERT INTO Product (productname, groupid, createddate, price, vatprice, vatpercentage)
VALUES ("Potatoes", 3, CURRENT_TIMESTAMP, 0.48, 0.6, 20);
INSERT INTO Product (productname, groupid, createddate, price, vatprice, vatpercentage)
VALUES ("Spatula", 6, CURRENT_TIMESTAMP, 5.6, 7, 20);
INSERT INTO Product (productname, groupid, createddate, price, vatprice, vatpercentage)
VALUES ("Kettle", 5, CURRENT_TIMESTAMP, 10, 12.5, 20);
INSERT INTO Product (productname, groupid, createddate, price, vatprice, vatpercentage)
VALUES ("Cherrys", 4, CURRENT_TIMESTAMP, 0.96, 1.2, 20);
INSERT INTO Product (productname, groupid, createddate, price, vatprice, vatpercentage)
VALUES ("Carrots", 3, CURRENT_TIMESTAMP, 0.38, 0.48, 20);

INSERT INTO  ProductShop (productid, shopid)
VALUES (1,1);
INSERT INTO  ProductShop (productid, shopid)
VALUES (2,2);
INSERT INTO  ProductShop (productid, shopid)
VALUES (3,3);
INSERT INTO  ProductShop (productid, shopid)
VALUES (4,1);
INSERT INTO  ProductShop (productid, shopid)
VALUES (5,2);
INSERT INTO  ProductShop (productid, shopid)
VALUES (6,3);

