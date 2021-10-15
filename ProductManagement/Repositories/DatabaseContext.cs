using MySql.Data.MySqlClient;
using ProductManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductManagement.Repositories
{
    public class DatabaseContext : IProductRepository
    {
        public string ConnectionString { get; set; }

        public DatabaseContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        //Get all the Products and info about them that exist in the database.
        public IEnumerable<Product> GetProducts()
        {
            List<Product> products = new();

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT p.*, Shop.shopname, c.groupname FROM ProductShop as ps JOIN Product as p ON p.productid = ps.productid " +
                    "JOIN Shop ON Shop.shopid = ps.shopid JOIN Category as c ON c.groupid = p.groupid; ";

                MySqlCommand cmd = new(query, connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (products.Any(product => product.ProductId == reader.GetInt64("productid")))
                        {
                            var product = products.FirstOrDefault(product => product.ProductId == reader.GetInt64("productid"));
                            product.Shops.Add(reader.GetString("shopname"));
                        }
                        else
                        {
                            products.Add(new Product()
                            {
                                ProductId = reader.GetInt64("productid"),
                                Name = reader.GetString("productname"),
                                GroupId = reader.GetInt64("groupid"),
                                GroupName = reader.GetString("groupname"),
                                CreatedDate = reader.GetDateTime("createddate"),
                                Price = reader.GetDecimal("price"),
                                VatPrice = reader.GetDecimal("vatprice"),
                                VatPercentage = reader.GetInt32("vatpercentage"),
                                Shops = new List<string>() { reader.GetString("shopname") }
                            });
                        }
                    }
                }

            }
            return products;
        }

        //Get a product and its info by the ID of the product from the database.
        public Product GetProduct(long id)
        {
            Product product = null;
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT p.*, Shop.shopname, c.groupname FROM ProductShop as ps JOIN Product as p ON p.productid = ps.productid " +
                    "JOIN Shop ON Shop.shopid = ps.shopid JOIN Category as c ON c.groupid = p.groupid WHERE p.productid = " + id;

                MySqlCommand cmd = new(query, connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (product != null)
                        {
                            product.Shops.Add(reader.GetString("shopname"));
                        }
                        else
                        {
                            product = new()
                            {
                                ProductId = reader.GetInt64("productid"),
                                Name = reader.GetString("productname"),
                                GroupId = reader.GetInt64("groupid"),
                                GroupName = reader.GetString("groupname"),
                                CreatedDate = reader.GetDateTime("createddate"),
                                Price = reader.GetDecimal("price"),
                                VatPrice = reader.GetDecimal("vatprice"),
                                VatPercentage = reader.GetInt32("vatpercentage"),
                                Shops = new List<string>() { reader.GetString("shopname") }
                            };
                        }
                    }
                }

            }
            return product;
        }

        //Create new product in the database.
        public void CreateProduct(Product product)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO Product (productname, groupid, createddate, price, vatprice, vatpercentage) VALUES (@productname, @groupid, @createddate, @price, @vatprice, @vatpercentage)";

                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@productname", product.Name);
                    cmd.Parameters.AddWithValue("@groupid", product.GroupId);
                    cmd.Parameters.AddWithValue("@createddate", product.CreatedDate);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@vatprice", product.VatPrice);
                    cmd.Parameters.AddWithValue("@vatpercentage", product.VatPercentage);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //Add ProductId and ShopId to the junction table in the database.
        public void AddProductToShops(long id, List<int> shopIds)
        {
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "INSERT INTO ProductShop (productid, shopid) VALUES (@productid, @shopid)";

                foreach (var shopId in shopIds)
                {
                    using (MySqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Parameters.AddWithValue("@productid", id);
                        cmd.Parameters.AddWithValue("@shopid", shopId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        //Get all the Groups and info about them that exist in the database.
        public IEnumerable<Group> GetGroups()
        {
            List<Group> groups = new();

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT * FROM Category;";

                MySqlCommand cmd = new(query, connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(new Group()
                        {
                            Id = reader.GetInt64("groupid"),
                            ParentId = reader.GetInt64("parentid"),
                            GroupName = reader.GetString("groupname"),
                            Children = new List<Group>()
                        });
                    }
                }



            }
            return groups;
        }

        //Get the ID of the last row that was added to the database.
        public long LastInsertId()
        {
            long id = 0;
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT LAST_INSERT_ID()";
                MySqlCommand cmd = new(query, connection);

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    id = reader.GetInt64(0);
                }
            }
            return id;
        }

        //Get the Groups ID, name and its parent ID from the databse according to its ID.
        public Group GetGroup(long id)
        {
            Group group = null;
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Category WHERE groupid = " + id;
                MySqlCommand cmd = new(query, connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        group = new()
                        {
                            Id = reader.GetInt64("groupid"),
                            GroupName = reader.GetString("groupname"),
                            ParentId = reader.GetInt64("parentid")
                        };
                    }
                }
            }
            return group;
        }

        //Get the shops ID and name from the databse according to its ID.
        public Shop GetShop(long id)
        {
            Shop shop = null;
            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT * FROM Shop WHERE shopid = " + id;
                MySqlCommand cmd = new(query, connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        shop = new()
                        {
                            Id = reader.GetInt64("shopid"),
                            Name = reader.GetString("shopname")
                        };
                    }
                }
            }
            return shop;
        }

        public IEnumerable<Product> GetProductsByShopId(long id)
        {
            List<Product> products = new();

            using (MySqlConnection connection = GetConnection())
            {
                connection.Open();

                string query = "SELECT p.*, Shop.shopname, c.groupname FROM ProductShop as ps JOIN Product as p ON p.productid = ps.productid " +
                    "JOIN Shop ON Shop.shopid = ps.shopid JOIN Category as c ON c.groupid = p.groupid WHERE Shop.shopid = " + id;

                MySqlCommand cmd = new(query, connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (products.Any(product => product.ProductId == reader.GetInt64("productid")))
                        {
                            var product = products.FirstOrDefault(product => product.ProductId == reader.GetInt64("productid"));
                            product.Shops.Add(reader.GetString("shopname"));
                        }
                        else
                        {
                            products.Add(new Product()
                            {
                                ProductId = reader.GetInt64("productid"),
                                Name = reader.GetString("productname"),
                                GroupId = reader.GetInt64("groupid"),
                                GroupName = reader.GetString("groupname"),
                                CreatedDate = reader.GetDateTime("createddate"),
                                Price = reader.GetDecimal("price"),
                                VatPrice = reader.GetDecimal("vatprice"),
                                VatPercentage = reader.GetInt32("vatpercentage"),
                                Shops = new List<string>() { reader.GetString("shopname") }
                            });
                        }
                    }
                }

            }
            return products;
        }
    }
}
