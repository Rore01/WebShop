using WebShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace WebShop
{
    internal class Purchase
    {
        public static void ordering()
        {
            using (var db = new MyDbContext())
            {
                Console.Clear();
                Console.WriteLine();
                // display for totalprice
                List<decimal?> quantities = db.Orders // finding and selecting prices od the products in order (totalprice meaning price)
                                        .Select(x => x.TotalPrice)
                                        .ToList();
                if (quantities != null)
                {


                    decimal total = quantities.Sum(q => q ?? 0); // summing up the prices of products in "total" and using ternary operator to treat null values as 0

                    var rows = new List<string>
                    {
                        "",
                        $"  Total Price : {total}kr  ",
                        ""
                    };
                    var totalprice = new Window("", 13, 2, rows);
                    totalprice.Draw();
                    Console.WriteLine();


                    bool continueRunning = true;
                    //looping the unpurchased products in a list
                    while (continueRunning)
                    {
                        // fetch and display orders
                        List<string> orders = db.Orders
                                                .Where(x => x.Purchased == false)
                                                .Select(x => " " + x.Name + "     " + x.Quantity + "     " + x.TotalPrice + "kr   ")
                                                .ToList();
                        if (orders.All(order => string.IsNullOrWhiteSpace(order)))  // If all orders are empty or only have white spaces
                        {
                            orders.Clear();  // clear up the list if it contains invalid data
                            Console.Write("Press any key to return: ");
                            break; // print empty message
                        }
                        else
                        {
                            var n = new Window("Order list", 1, 9, orders);
                            n.Draw();  // Only draw the window if there is valid data
                            var inputframerows = new List<string>
                            {
                                "[X] to check out",
                                "[Q] to quit",
                                "[C] to clear the list"
                            };
                            var inputframe = new Window("", 1, 18, inputframerows);
                            inputframe.Draw();
                            Console.SetCursorPosition(0, Lowest.LowestPosition + 1);
                            Console.Write("");
                            string checkinout = Console.ReadLine();

                            // If user presses X, go to checkout process
                            if (checkinout.Equals("X", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.Clear();
                                Console.WriteLine("Proceeding to checkout...");
                                Console.WriteLine("");

                                // asking for customer details
                                Console.Write("Enter your name: ");
                                string checkname = Console.ReadLine();
                                Console.Write("Enter your lastname: ");
                                string checklast = Console.ReadLine();
                                Console.Write("Enter your address: ");
                                string checkaddress = Console.ReadLine();

                                // checking if client exists in DB
                                var clientcheck = db.Clients.FirstOrDefault(c => c.FirstName == checkname && c.LastName == checklast && c.Address == checkaddress);

                                if (clientcheck != null)
                                {
                                    // existing client, proceed with payment method
                                    Console.Clear();
                                    List<string> paymethodlist = new List<string>();
                                    {
                                        paymethodlist.Add("[1] CreditCard");
                                        paymethodlist.Add("[2] Swish");
                                        paymethodlist.Add("[3] Paypal");
                                        paymethodlist.Add("[4] Faktura");
                                        paymethodlist.Add("[5] Return to main menu");
                                    }
                                    var pay1frame = new Window("Payment Method", 1, 1, paymethodlist);
                                    pay1frame.Draw();
                                    Console.Write("Input: ");
                                    int pay1;
                                    bool validInput1 = int.TryParse(Console.ReadLine(), out pay1);
                                    //since we dont do actual payment the input dsnt really matter as long as its in range(1-4) 
                                    if (pay1 == 5)
                                    {
                                        break;
                                    }
                                    if (validInput1 && pay1 >= 1 && pay1 <= 4)
                                    {
                                        //process the payment and update orders
                                        var accepted = db.Orders
                                                        .Where(x => x.Quantity > 0) //i decided to find the products in the list from their quantity (if there is any quantity of product = my order list)
                                                        .ToList();
                                        foreach (var order in accepted)
                                        {
                                            order.Purchased = true; // set the Purchased property to true
                                        }
                                        db.SaveChanges();

                                        // find orders with purchased status = true
                                        var ordersToRemove = db.Orders
                                                                .Include(o => o.Products)
                                                                .Where(m => m.Purchased == true)
                                                                .ToList();

                                        //a loop to clear the order list after purchased = true, and reducing the products quantity by 1 per quantity(loop)
                                        foreach (var order in ordersToRemove)
                                        {
                                            // find products matching with the ordered products
                                            var product = db.Products.FirstOrDefault(p => p.Id == order.ProductId);
                                            // if not empty
                                            if (product != null)
                                            {
                                                // Update the quantity of the product

                                                // Reduce product quantity by the order quantity
                                                product.Quantity -= 1;

                                                // prevent negative quantities, setting 0 as lowest
                                                if (product.Quantity < 0)
                                                {
                                                    product.Quantity = 0;
                                                }
                                                db.Products.Update(product);

                                            }
                                            order.Name = null;
                                            order.Quantity = 0;
                                            order.ProductId = null;
                                            order.TotalPrice = null;
                                        }
                                        db.SaveChanges();
                                    }
                                }

                                else
                                {
                                    // if no client found, creat new one
                                    Console.Clear();
                                    Console.WriteLine("");
                                    Console.WriteLine("New client");
                                    Console.WriteLine("");
                                    var newclient = new Models.Client
                                    {
                                        FirstName = checkname,
                                        LastName = checklast,
                                        Address = checkaddress
                                    };
                                    db.Clients.Add(newclient);
                                    db.SaveChanges();

                                    // returning back to payment method
                                    List<string> paymethodlist2 = new List<string>();
                                    {
                                        paymethodlist2.Add("[1] CreditCard");
                                        paymethodlist2.Add("[2] Swish");
                                        paymethodlist2.Add("[3] Paypal");
                                        paymethodlist2.Add("[4] Faktura");
                                        paymethodlist2.Add("[5] Return to main menu");
                                    }
                                    var pay1frame2 = new Window("Payment Method", 1, 3, paymethodlist2);
                                    pay1frame2.Draw();
                                    Console.Write("Input: ");

                                    int pay2;
                                    bool validInput = int.TryParse(Console.ReadLine(), out pay2);

                                    if (pay2 == 5)
                                    {
                                        break;
                                    }
                                    //repeating same proccess
                                    if (validInput && pay2 >= 1 && pay2 <= 4)
                                    {
                                        var accepted = db.Orders
                                                    .Where(x => x.Quantity > 0)
                                                    .ToList();
                                        foreach (var order in accepted)
                                        {
                                            order.Purchased = true;
                                        }
                                        db.SaveChanges();

                                        var ordersToRemove = db.Orders
                                                                .Include(o => o.Products)
                                                                .Where(m => m.Purchased == true)
                                                                .ToList();

                                        foreach (var order in ordersToRemove)
                                        {

                                            var product = db.Products.FirstOrDefault(p => p.Id == order.ProductId);

                                            if (product != null)
                                            {

                                                product.Quantity -= 1;

                                                if (product.Quantity < 0)
                                                {
                                                    product.Quantity = 0;
                                                }

                                                db.Products.Update(product);


                                            }
                                            order.Name = null;
                                            order.Quantity = 0;
                                            order.ProductId = null;
                                            order.TotalPrice = null;

                                            db.SaveChanges();
                                        }
                                    }
                                }

                            }

                            // If user presses Q, quit the whole process
                            if (checkinout.Equals("Q", StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }

                            // if user presses C, wipes out the order list
                            if (checkinout.Equals("C", StringComparison.OrdinalIgnoreCase))
                            {
                                string connectionString = "Server=tcp:roredb.database.windows.net,1433;Initial Catalog=webshop;Persist Security Info=False;User ID=roredb;Password=dbdbdb.123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                                using (IDbConnection dbb = new SqlConnection(connectionString))
                                {
                                    // i was able to clear out all the orders but not the rows which were created from orders in the lists
                                    // so i asked chatgpt and after several ways of approaching this problem, this was the way to make it work
                                    // here i used dapper to connect to db and update the orderid which was a foreign key, then another query where it removes 
                                    // the products which wasnt purchased
                                    dbb.Open();
                                    // so i start a transaction to to perform both of the queries together
                                    using (var transaction = dbb.BeginTransaction())
                                    {
                                        try
                                        {
                                            // update the related products first
                                            string deleteProductsQuery = @"
                                            UPDATE Products
                                            SET OrderId = NULL
                                            WHERE OrderId IN (SELECT OrderId FROM Orders WHERE Purchased = 0);";

                                            dbb.Execute(deleteProductsQuery, transaction: transaction);

                                            // Now delete the orders
                                            string deleteOrdersQuery = @"
                                            DELETE FROM Orders
                                            WHERE Purchased = 0;";

                                            int rowsAffected = dbb.Execute(deleteOrdersQuery, transaction: transaction);

                                            // Commit the transaction
                                            transaction.Commit();
                                        }
                                        catch (Exception ex)
                                        {
                                            transaction.Rollback();
                                            Console.WriteLine($"An error occurred: {ex.Message}");
                                        }
                                    }
                                }
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
