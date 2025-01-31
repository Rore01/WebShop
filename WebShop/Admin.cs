using WebShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Identity.Client;
using WebShop.Models;

namespace WebShop
{
    internal class Admin
    {
        // administer products

        //insert new products
        public static void addproduct(string name, decimal price, int quantity, int categoryid)
        {
            using (var db = new MyDbContext())
            {
                try
                {
                    var category = db.Categories.Find(categoryid);
                    if (category == null)
                    {
                        Console.WriteLine("Category not found.");
                        return;
                    }
                    var newproduct = new Product
                    {
                        Name = name,
                        Price = price,
                        Quantity = quantity,
                        CategoryId = categoryid
                    };
                    db.Products.Add(newproduct);
                    db.SaveChanges();
                    Console.WriteLine("Product " + name + " added successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error adding product: " + ex.InnerException.Message);
                }

            }
        }

        //remove products
        public static void removeproduct(int categoryid, int productid)
        {
            using (var db = new MyDbContext())
            {
                try
                {
                    var producttoremove = db.Products.Find(productid);
                    if (producttoremove == null)
                    {
                        Console.WriteLine("Product not found.");
                        return;
                    }

                    db.Products.Remove(producttoremove);
                    db.SaveChanges();
                    Console.WriteLine("Product " + producttoremove.Name + " removed successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error removing product: " + ex.InnerException.Message);
                }
            }
        }

        //update products
        public static void updateproduct(int productid, string? newname = null, decimal? newprice = null, int? newquantity = null)
        {
            using (var db = new MyDbContext())
            {
                try
                {
                    var product = db.Products.Find(productid);
                    if (product == null)
                    {
                        Console.WriteLine("Product not found.");
                        return;
                    }
                    if (!string.IsNullOrEmpty(newname))
                    {
                        product.Name = newname;
                    }
                    if (newprice.HasValue)
                    {
                        product.Price = newprice.Value;
                    }
                    if (newquantity.HasValue)
                    {
                        product.Quantity = newquantity.Value;
                    }
                    db.SaveChanges();
                    Console.WriteLine("Product updated successfully!");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating product: " + ex.InnerException.Message);
                }
            }
        }

        //administer categories

        //insert new categories
        public static void addcategory(string name)
        {
            using (var db = new MyDbContext())
            {

                var category = new Category
                {
                    Name = name
                };
                db.Categories.Add(category);
                db.SaveChanges();
                Console.WriteLine("Category " + category.Name + " added successfully!");

            }
        }

        //remove categories
        public static void removecategory(int categoryId)
        {
            using (var db = new MyDbContext())
            {
                try
                {
                    var category = db.Categories.Find(categoryId);
                    if (category == null)
                    {
                        Console.WriteLine("Category not found.");
                        return;
                    }
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    Console.WriteLine("Category" + category.Name + "removed successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error removing category: " + ex.InnerException.Message);
                }
            }
        }

        //update category name
        public static void updatecategory(int categoryId, string name)
        {
            using (var db = new MyDbContext())
            {
                var changecategory = db.Categories.Find(categoryId);
                if (changecategory != null)
                {
                    changecategory.Name = name;
                }
                db.SaveChanges();
                Console.WriteLine("Category " + changecategory.Name + " updated successfully!");

            }
        }

        //administer clients

        //update clients info
        public static void updateclient(string? firstname, string? lastname, string? address)
        {
            using (var db = new MyDbContext())
            {

                var newclient = db.Clients.FirstOrDefault(c => c.FirstName == firstname);

                if (newclient.FirstName == firstname && newclient.LastName == lastname || newclient.Address == address) // if rather clients name and last name is the same or only the address
                {
                    Console.Write("Enter new name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter new Lastname: ");
                    string lasttname = Console.ReadLine();
                    Console.Write("Enter new Address: ");
                    string adress = Console.ReadLine();

                    newclient.FirstName = name;
                    newclient.LastName = lasttname;
                    newclient.Address = adress;
                }
                db.SaveChanges();
                Console.WriteLine("Clients information updated.");



            }

        }

        //statistics
        //Using Dapper SQL
        // Most sold products 
        public static void mostsold()
        {
            string connectionString = "Server=tcp:roredb.database.windows.net,1433;Initial Catalog=webshop;Persist Security Info=False;User ID=roredb;Password=dbdbdb.123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();


                string sql = @"
                SELECT TOP 3 Name, Quantity, (50 - Quantity) AS Sold, CategoryId 
                FROM Products
                ORDER BY Quantity ASC";

                var products = connection.Query<Product>(sql).ToList();

                List<string> textRows = new List<string>();

                int idd = 0;
                foreach (var product in products)
                {
                    idd++;
                    string productRow = $"  " +
                                        $"[{idd}] {product.Name} " +
                                        $"Sold: {50 - product.Quantity}" +
                                        $"  ";
                    textRows.Add(productRow);
                }

                var window = new Window("Most sold products", 1, 1, textRows);
                window.Draw();
            }
        }

        // Most popular category
        public static void popularcategory()
        {
            string connectionString = "Server=tcp:roredb.database.windows.net,1433;Initial Catalog=webshop;Persist Security Info=False;User ID=roredb;Password=dbdbdb.123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT TOP 1
                    c.Name AS CategoryName,
                    (250 - (SUM (p.Quantity))) AS mostsoldcategory
                FROM
                    Products p
                JOIN
                    Categories c ON p.CategoryId = c.Id
                GROUP BY
                    c.Name
                ORDER BY
                    mostsoldcategory DESC;";

                var category = db.QueryFirstOrDefault<(string CategoryName, int TotalSold)>(query);

                if (category != default)
                {

                    var windowContent = new List<string>
                    {
                        $"",
                        $"  Category: {category.CategoryName}",
                        $""
                    };

                    var window = new Window("Popular category", 1, 7, windowContent);

                    window.Draw();
                }
                else
                {
                    Console.WriteLine("No data found.");
                }
            }
        }

        // Total of products in stock
        public static void totalinstock()
        {
            string connectionString = "Server=tcp:roredb.database.windows.net,1433;Initial Catalog=webshop;Persist Security Info=False;User ID=roredb;Password=dbdbdb.123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                string query = "SELECT SUM(p.Quantity) AS TotalProductsInStock FROM Products p;";

                var totalInStock = db.QueryFirstOrDefault<int>(query);

                var windowContent = new List<string>
                {
                    $"",
                    $"        {totalInStock} products",
                    $""
                };

                var window = new Window("Total Products In Stock", 1, 13, windowContent);

                window.Draw();
            }
        }

        // Total sold products
        public static void totalsold()
        {
            string connectionString = "Server=tcp:roredb.database.windows.net,1433;Initial Catalog=webshop;Persist Security Info=False;User ID=roredb;Password=dbdbdb.123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                string query = @"
                    SELECT (COUNT(DISTINCT p.CategoryId) * 250) - SUM(p.Quantity) FROM Products p;";

                var totalSold = db.QueryFirstOrDefault<int>(query);


                var windowContent = new List<string>
                {
                    $"",
                    $"     {totalSold} products",
                    $""
                };

                var window = new Window("Total Sold Products", 26, 7, windowContent);

                window.Draw();
            }




        }

        // Products out of stock
        public static void outofstock()
        {
            string connectionString = "Server=tcp:roredb.database.windows.net,1433;Initial Catalog=webshop;Persist Security Info=False;User ID=roredb;Password=dbdbdb.123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (IDbConnection db = new SqlConnection(connectionString))
            {

                string query = @"
                    SELECT p.Name, p.Quantity
                    FROM Products p
                    WHERE p.Quantity = 0;";

                var outOfStockProducts = db.Query(query);
                if (outOfStockProducts.Any())
                {
                    var windowContent = new List<string> { "" };
                    foreach (var product in outOfStockProducts)
                    {
                        windowContent.Add($"{product.Name} - Quantity: {product.Quantity}");
                    }

                    var window = new Window("Out of Stock Products", 22, 13, windowContent);
                    window.Draw();
                }
                else
                {
                    var empty = new List<string> { "", "           None", "" };

                    var windoww = new Window("Out of Stock Products", 1, 19, empty);
                    windoww.Draw();
                    Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
                }
            }
        }
    }
}
