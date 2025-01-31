using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;
using Microsoft.EntityFrameworkCore;

namespace WebShop
{
    internal class Seed
    {
        public static void seed()
        {
            using (var db = new MyDbContext())
            {
                //Categories
                var category1 = new Category { Name = "Mouse" };
                var category2 = new Category { Name = "Keyboard" };
                var category3 = new Category { Name = "Headset" };

                db.Categories.AddRange(category1, category2, category3);
                db.SaveChanges();

                //All the products:
                //Mouse
                var product1 = new Product { Name = "Logitech G PRO Wireless          ", Price = 1090, Quantity = 50, CategoryId = 1 };
                var product2 = new Product { Name = "HyperX Pulsefire Haste           ", Price = 649, Quantity = 50, CategoryId = 1 };
                var product3 = new Product { Name = "Viper V3 Pro Wireless            ", Price = 1639, Quantity = 50, CategoryId = 1 };
                var product4 = new Product { Name = "Corsair M75 RGB Wireless         ", Price = 1490, Quantity = 50, CategoryId = 1 };
                var product5 = new Product { Name = "Razer Basilisk V3                ", Price = 729, Quantity = 50, CategoryId = 1 };

                //Keyboard
                var product6 = new Product { Name = "Razer BlackWidow V4              ", Price = 1969, Quantity = 50, CategoryId = 2 };
                var product7 = new Product { Name = "SteelSeries Apex Pro mini        ", Price = 3129, Quantity = 50, CategoryId = 2 };
                var product8 = new Product { Name = "EPOMAKER x AULA F75              ", Price = 780, Quantity = 50, CategoryId = 2 };
                var product9 = new Product { Name = "HyperX Alloy Origins             ", Price = 1339, Quantity = 50, CategoryId = 2 };
                var product10 = new Product { Name = "Asus ROG Strix Scope II 96      ", Price = 1829, Quantity = 50, CategoryId = 2 };

                //Headset
                var product11 = new Product { Name = "HyperX Cloud III Wireless       ", Price = 1749, Quantity = 50, CategoryId = 3 };
                var product12 = new Product { Name = "Razer Kraken Pro V2             ", Price = 446, Quantity = 50, CategoryId = 3 };
                var product13 = new Product { Name = "Logitech G Pro X                ", Price = 1139, Quantity = 50, CategoryId = 3 };
                var product14 = new Product { Name = "Corsair HS55 Stereo             ", Price = 789, Quantity = 50, CategoryId = 3 };
                var product15 = new Product { Name = "SteelSeries Arctis Pro Wireless ", Price = 4099, Quantity = 50, CategoryId = 3 };


                db.Products.AddRange(product1, product2, product3, product4, product5,
                                    product6, product7, product8, product9, product10,
                                    product11, product12, product13, product14, product15);
                db.SaveChanges();

                var client1 = new Client { FirstName = "John", LastName = "Doe", Address = "123 Main St" };
                var client2 = new Client { FirstName = "Jimy", LastName = "Doe", Address = "122 Main St" };
                var client3 = new Client { FirstName = "Samm", LastName = "Doe", Address = "133 Main St" };
                db.Clients.AddRange(client1, client2, client3);
                db.SaveChanges();

                while (true)
                {
                    foreach (var categories in db.Categories.Include(c => c.Products).ToList())
                    {
                        Console.WriteLine(categories.Id + " " + categories.Name);
                        foreach (var products in categories.Products)
                        {
                            Console.WriteLine("\t" + "[" + products.Id + "] " + "Name: " + products.Name);
                            Console.WriteLine("\t" + "Price: " + products.Price + "kr");
                            Console.WriteLine("\t" + "Quantity: " + products.Quantity);
                            Console.WriteLine();
                        }
                    }
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}
