using WebShop.Models;
using Microsoft.EntityFrameworkCore;

namespace WebShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Seed.seed(); //Here is all my seeding data in case if i need to reseed my database
            while (true)
            {
                Console.Clear();

                //welcome page in frame
                var welcomeText = new List<string>
                {
                    "Welcome to the ClickNShop!"
                };
                var welcomeWindow = new Window("", 1, 1, welcomeText);
                welcomeWindow.Draw();

                var options = new List<string>
                {
                    "1. Admin",
                    "2. Client"
                };
                var optionsWindow = new Window("", 1, 4, options);
                optionsWindow.Draw();

                Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
                Console.Write("Select an option (1 or 2): ");
                int inputs;

                // Validate the input
                if (!int.TryParse(Console.ReadLine(), out inputs) || (inputs >= 1 && inputs <= 2))

                    // Handles Admin option
                    if (inputs == 1)
                    {
                        Console.Clear();

                        // Admin menu options
                        var adminMenu = new List<string>
                    {
                         "1. Administer products",
                         "2. Administer categories",
                         "3. Administer clients",
                         "4. See Statistics",
                         "5. Return to main menu"
                    };
                        var adminWindow = new Window("Admin Menu", 1, 1, adminMenu);
                        adminWindow.Draw();

                        Console.SetCursorPosition(0, Lowest.LowestPosition + 1);
                        Console.Write("Select an admin option: ");
                        int admininput;
                        if (!int.TryParse(Console.ReadLine(), out admininput) || (admininput >= 1 && admininput <= 5))

                            //if "Administer products" is selected
                            if (admininput == 1)
                            {

                                while (true)
                                {
                                    Console.Clear();

                                    // Administer products menu
                                    var adminMenuOptions = new List<string>
                                {
                                    "1. Add a new product",
                                    "2. Remove a product",
                                    "3. Update a product",
                                    "4. Return to main menu"
                                };
                                    var adminMenuWindow = new Window("Administer Products", 1, 1, adminMenuOptions);
                                    adminMenuWindow.Draw();

                                    Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
                                    Console.Write("Select an option: ");
                                    if (!int.TryParse(Console.ReadLine(), out var adminInput) || (adminInput <= 1 && adminInput >= 4))
                                    {
                                        Console.WriteLine("Invalid input. Please try again.");
                                        Console.ReadKey();
                                        continue;
                                    }

                                    if (adminInput == 4) break; // return to main menu

                                    switch (adminInput)
                                    {
                                        case 1: // Add a new product
                                            Console.Clear();
                                            Console.WriteLine("Adding a new product");
                                            Console.Write("Enter product name: ");
                                            string name = Console.ReadLine();
                                            Console.Write("Enter price: ");
                                            if (!decimal.TryParse(Console.ReadLine(), out var price))
                                            {
                                                Console.WriteLine("Invalid price. Returning to menu...");
                                                Console.ReadKey();
                                                continue;
                                            }
                                            Console.Write("Enter quantity: ");
                                            if (!int.TryParse(Console.ReadLine(), out var quantity))
                                            {
                                                Console.WriteLine("Invalid quantity. Returning to menu...");
                                                Console.ReadKey();
                                                continue;
                                            }
                                            Console.Write("Enter category ID: ");
                                            if (!int.TryParse(Console.ReadLine(), out var categoryId))
                                            {
                                                Console.WriteLine("Invalid category ID. Returning to menu...");
                                                Console.ReadKey();
                                                continue;
                                            }

                                            Admin.addproduct(name, price, quantity, categoryId);
                                            Console.WriteLine("Press any key to return...");
                                            Console.ReadKey();
                                            break;

                                        case 2: // Remove a product
                                            Console.Clear();
                                            Console.WriteLine("Removing a product");
                                            Console.Write("Enter product ID: ");
                                            if (!int.TryParse(Console.ReadLine(), out var productId))
                                            {
                                                Console.WriteLine("Invalid product ID. Returning to menu...");
                                                Console.ReadKey();
                                                continue;
                                            }
                                            Console.Write("Enter category ID: ");
                                            if (!int.TryParse(Console.ReadLine(), out categoryId))
                                            {
                                                Console.WriteLine("Invalid category ID. Returning to menu...");
                                                Console.ReadKey();
                                                continue;
                                            }

                                            Admin.removeproduct(categoryId, productId);
                                            Console.WriteLine("Press any key to return...");
                                            Console.ReadKey();
                                            break;

                                        case 3: // Update a product
                                            Console.Clear();
                                            Console.WriteLine("Updating a product");
                                            Console.Write("Enter product ID: ");
                                            if (!int.TryParse(Console.ReadLine(), out productId))
                                            {
                                                Console.WriteLine("Invalid product ID. Returning to menu...");
                                                Console.ReadKey();
                                                continue;
                                            }
                                            Console.Write("Enter new name (or press Enter to skip): ");
                                            string newName = Console.ReadLine();
                                            Console.Write("Enter new price (or press Enter to skip): ");
                                            var priceInput = Console.ReadLine();
                                            decimal? newPrice = string.IsNullOrWhiteSpace(priceInput) ? (decimal?)null : decimal.Parse(priceInput);
                                            Console.Write("Enter new quantity (or press Enter to skip): ");
                                            var quantityInput = Console.ReadLine();
                                            int? newQuantity = string.IsNullOrWhiteSpace(quantityInput) ? (int?)null : int.Parse(quantityInput);

                                            Admin.updateproduct(productId, newName, newPrice, newQuantity);
                                            Console.WriteLine("Press any key to return...");
                                            Console.ReadKey();
                                            break;

                                        default:
                                            Console.WriteLine("Invalid option. Returning to menu...");
                                            Console.ReadKey();
                                            break;
                                    }
                                }


                            }

                        //if "Administer Categories" is selected
                        if (admininput == 2)
                        {
                            while (true)
                            {
                                Console.Clear();
                                var adminCategoryOptions = new List<string>
                            {
                                "1. Add a new category",
                                "2. Remove a category",
                                "3. Update a category",
                                "4. Return to main menu"
                            };
                                var adminCategoryWindow = new Window("Administer Categories", 1, 1, adminCategoryOptions);
                                adminCategoryWindow.Draw();

                                Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
                                Console.Write("Select an option: ");
                                if (!int.TryParse(Console.ReadLine(), out var categoryInput) || categoryInput < 1 || categoryInput > 4)
                                {
                                    Console.WriteLine("Invalid input. Please try again.");
                                    Console.ReadKey();
                                    continue;
                                }

                                if (categoryInput == 4) break; // Return to main menu

                                switch (categoryInput)
                                {
                                    case 1: // Add a new category
                                        Console.Clear();
                                        Console.WriteLine("Adding a new category");
                                        Console.Write("Enter category name: ");
                                        string categoryName = Console.ReadLine();

                                        if (string.IsNullOrWhiteSpace(categoryName))
                                        {
                                            Console.WriteLine("Invalid name. Returning to menu...");
                                            Console.ReadKey();
                                            continue;
                                        }

                                        Admin.addcategory(categoryName);
                                        Console.WriteLine("Category added successfully. Press any key to return...");
                                        Console.ReadKey();
                                        break;

                                    case 2: // Remove a category
                                        Console.Clear();
                                        Console.WriteLine("Removing a category");
                                        Console.Write("Enter category ID to remove: ");
                                        if (!int.TryParse(Console.ReadLine(), out var categoryId))
                                        {
                                            Console.WriteLine("Invalid category ID. Returning to menu...");
                                            Console.ReadKey();
                                            continue;
                                        }

                                        Admin.removecategory(categoryId);
                                        Console.WriteLine("Category removed successfully. Press any key to return...");
                                        Console.ReadKey();
                                        break;

                                    case 3: // Update a category
                                        Console.Clear();
                                        Console.WriteLine("Updating a category");
                                        Console.Write("Enter category ID to update: ");
                                        if (!int.TryParse(Console.ReadLine(), out categoryId))
                                        {
                                            Console.WriteLine("Invalid category ID. Returning to menu...");
                                            Console.ReadKey();
                                            continue;
                                        }
                                        Console.Write("Enter new name for the category: ");
                                        string newCategoryName = Console.ReadLine();

                                        if (string.IsNullOrWhiteSpace(newCategoryName))
                                        {
                                            Console.WriteLine("Invalid name. Returning to menu...");
                                            Console.ReadKey();
                                            continue;
                                        }

                                        Admin.updatecategory(categoryId, newCategoryName);
                                        Console.WriteLine("Category updated successfully. Press any key to return...");
                                        Console.ReadKey();
                                        break;

                                    default:
                                        Console.WriteLine("Invalid option. Returning to menu...");
                                        Console.ReadKey();
                                        break;
                                }
                            }
                        }

                        //if "Administer Clients" is selected
                        if (admininput == 3)
                        {
                            while (true)
                            {
                                Console.Clear();

                                // Administer Clients Menu
                                var adminClientOptions = new List<string>
                            {
                                "1. Update Client Information",
                                "2. Return to main menu"
                            };
                                var adminClientWindow = new Window("Administer Clients", 1, 1, adminClientOptions);
                                adminClientWindow.Draw();

                                Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
                                Console.Write("Select an option: ");
                                if (!int.TryParse(Console.ReadLine(), out var clientInput) || clientInput == 1 && clientInput == 2)
                                {
                                    Console.WriteLine("Invalid input. Please try again.");
                                    Console.ReadKey();
                                    continue;
                                }

                                if (clientInput == 2) break; // return to main menu

                                if (clientInput == 1) // update Client Information
                                {
                                    Console.Clear();
                                    var db = new MyDbContext();
                                    //using linq to find and select clients information for display
                                    List<string> clients = db.Clients
                                                            .Select(c => "[" + c.Id + "] Name: " + c.FirstName + " " + c.LastName + "          Address: " + c.Address + "      ")
                                                            .ToList();
                                    var client1 = new Window("Clients", 1, 1, clients);
                                    client1.Draw();

                                    Console.Write("Enter client's first name: ");
                                    string firstName = Console.ReadLine();

                                    Console.Write("Enter client's last name: ");
                                    string lastName = Console.ReadLine();

                                    Console.Write("Enter new address (or press Enter to skip): ");
                                    string address = Console.ReadLine();

                                    if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                                    {
                                        Console.WriteLine("First name and last name are required.");
                                        Console.ReadKey();
                                        continue;
                                    }

                                    Admin.updateclient(firstName, lastName, address);
                                    Console.WriteLine("Client information updated successfully! Press any key to return...");
                                    Console.ReadKey();
                                }
                            }
                        }

                        //if "See statistics" is selected
                        if (admininput == 4)
                        {
                            Console.Clear();
                            Admin.mostsold();
                            Admin.popularcategory();
                            Admin.totalsold();
                            Admin.totalinstock();
                            Admin.outofstock();
                            Console.Write("Press any key to return...");
                            Console.ReadKey();
                        }

                        //if "Return to main menu" is selected
                        if (admininput == 5)
                        {
                            continue;
                        }
                    }

                // Handles Client option
                if (inputs == 2)
                {
                    using (var db = new MyDbContext())
                    {
                        while (true)
                        {
                            Console.Clear();


                            var clientMenu = new List<string>
                            {
                                "1. Start",
                                "2. Categories",
                                "3. Order",
                                "4. Return to main menu"
                            };
                            var clientwindow = new Window("Client Menu", 1, 1, clientMenu);
                            clientwindow.Draw();

                            Console.Write("Select an client option: ");
                            int clientinput;

                            if (!int.TryParse(Console.ReadLine(), out clientinput) || (clientinput >= 1 && clientinput <= 4))

                                // if option "Start" selected
                                if (clientinput == 1)
                                {
                                    Console.Clear();
                                    var welcomeText2 = new List<string>
                                {
                                    "",
                                    "    Most popular product!!!    ",
                                    ""
                                };
                                    var welcomeWindow2 = new Window("", 7, 1, welcomeText2);
                                    welcomeWindow2.Draw();

                                    //popular product 1
                                    var productWithLeastQuantity1 = db.Products //find the product with least quantity (least quantity = most sold product which makes it popular) from first category
                                                                     .Where(p => p.CategoryId == 1)
                                                                     .OrderBy(p => p.Quantity)
                                                                     .FirstOrDefault();

                                    List<string> salerows1 = new List<string>();

                                    if (productWithLeastQuantity1 != null)
                                    {
                                        salerows1.Add("");
                                        salerows1.Add($" {productWithLeastQuantity1.Name} {productWithLeastQuantity1.Price}kr");
                                        salerows1.Add("");
                                    }

                                    var sale1 = new Window($"[1] Mouse", 1, 7, salerows1);
                                    sale1.Draw();

                                    //popular product 2
                                    var productWithLeastQuantity2 = db.Products //find the product with least quantity (least quantity = most sold product which makes it popular) from seccond category
                                                                     .Where(p => p.CategoryId == 2)
                                                                     .OrderBy(p => p.Quantity)
                                                                     .FirstOrDefault();

                                    List<string> salerows2 = new List<string>();

                                    if (productWithLeastQuantity2 != null)
                                    {
                                        salerows2.Add("");
                                        salerows2.Add($" {productWithLeastQuantity2.Name} {productWithLeastQuantity2.Price}kr");
                                        salerows2.Add("");
                                    }

                                    var sale2 = new Window($"[2] Keyboard", 1, 13, salerows2);
                                    sale2.Draw();

                                    //popular product 3
                                    var productWithLeastQuantity3 = db.Products //find the product with least quantity (least quantity = most sold product which makes it popular) from third category
                                                                     .Where(p => p.CategoryId == 3)
                                                                     .OrderBy(p => p.Quantity)
                                                                     .FirstOrDefault();

                                    List<string> salerows3 = new List<string>();

                                    if (productWithLeastQuantity3 != null)
                                    {
                                        salerows3.Add("");
                                        salerows3.Add($" {productWithLeastQuantity3.Name} {productWithLeastQuantity3.Price}kr");
                                        salerows3.Add("");
                                    }

                                    var sale3 = new Window($"[3] Headset", 1, 19, salerows3);
                                    sale3.Draw();


                                    Console.SetCursorPosition(0, Lowest.LowestPosition + 3);
                                    Console.Write("Select Product: ");
                                    int saleinput;

                                    if (!int.TryParse(Console.ReadLine(), out saleinput) || (saleinput <= 1 && saleinput >= 3))
                                    {
                                        break;
                                    }


                                    //if product [1] is selected
                                    if (saleinput == 1)
                                    {
                                        Console.Clear();
                                        // make an order of prododuct 1
                                        var s1 = new Order
                                        {
                                            ProductId = productWithLeastQuantity1.Id, // Match the selected products id 
                                            Name = productWithLeastQuantity1.Name,
                                            Quantity = 1,
                                            TotalPrice = productWithLeastQuantity1.Price,
                                            OrderDate = DateTime.Now,
                                            Products = new List<Product> { productWithLeastQuantity1 }
                                        };
                                        db.Orders.Add(s1);
                                        db.SaveChanges();
                                        Console.WriteLine("Product successfully added!");
                                        Console.ReadKey();
                                        continue;
                                    }

                                    //if product [2] is selected
                                    if (saleinput == 2)
                                    {
                                        Console.Clear();
                                        // make an order of prododuct 2
                                        var s2 = new Order
                                        {
                                            ProductId = productWithLeastQuantity2.Id, // Match the selected products id
                                            Name = productWithLeastQuantity2.Name,
                                            Quantity = 1,
                                            TotalPrice = productWithLeastQuantity2.Price,
                                            OrderDate = DateTime.Now,
                                            Products = new List<Product> { productWithLeastQuantity2 }
                                        };
                                        db.Orders.Add(s2);
                                        db.SaveChanges();
                                        Console.WriteLine("Product successfully added!");
                                        Console.ReadKey();
                                        continue;
                                    }

                                    //if product [3] is selected
                                    if (saleinput == 3)
                                    {
                                        Console.Clear();
                                        // make an order of prododuct 3
                                        var s3 = new Order
                                        {
                                            ProductId = productWithLeastQuantity3.Id, // Match the selected products id
                                            Name = productWithLeastQuantity3.Name,
                                            Quantity = 1,
                                            TotalPrice = productWithLeastQuantity3.Price,
                                            OrderDate = DateTime.Now,
                                            Products = new List<Product> { productWithLeastQuantity3 }
                                        };
                                        db.Orders.Add(s3);
                                        db.SaveChanges();
                                        Console.WriteLine("Product successfully added!");
                                        Console.ReadKey();
                                        continue;
                                    }


                                }

                            // if option "Categories" selected 
                            if (clientinput == 2)
                            {
                                Console.Clear();
                                List<string> categoryRows = db.Categories
                                                                .Select(c => $"[{c.Id}] {c.Name}") //select the categorys id and name for display
                                                                .ToList();

                                var categoryWindow = new Window("Categories", 1, 1, categoryRows);
                                categoryWindow.Draw();

                                Console.SetCursorPosition(0, Lowest.LowestPosition + 1);
                                Console.Write("Enter a category ID: ");
                                int input;

                                // validate input
                                if (!int.TryParse(Console.ReadLine(), out input))
                                {
                                    continue;
                                }

                                if (db.Categories.Any(c => c.Id == input)) //if the input matches with any of the category from the categories list
                                {
                                    Console.Clear();

                                    // get the selected category name
                                    var selectedCategory = db.Categories.FirstOrDefault(c => c.Id == input);

                                    // get prodcts in the selected category
                                    var productsx = db.Products.ToList();
                                    var products = productsx
                                                    .Where(p => p.CategoryId == input)
                                                    .ToList();

                                    List<string> productRows = new List<string>();

                                    //display the products inside the selected category if theres any
                                    if (products.Any())
                                    {
                                        productRows.Add($"");
                                        foreach (var product in products)
                                        {
                                            productRows.Add($"[{product.Id}] Name: {product.Name}");
                                            productRows.Add($"    Price: {product.Price}kr");
                                            productRows.Add("");
                                        }


                                    }
                                    else
                                    {
                                        productRows.Add($"No products found in {selectedCategory.Name}.");
                                    }

                                    var productWindow = new Window($"{selectedCategory.Name}", 1, 1, productRows);
                                    productWindow.Draw();

                                    Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
                                    Console.Write("Select product by ID: ");
                                    int productinput;

                                    if (!int.TryParse(Console.ReadLine(), out productinput))
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Press any key to try again...");
                                        Console.ReadKey();
                                        continue;
                                    }

                                    //find the right prodct matching the input
                                    var orderedproduct = db.Products.FirstOrDefault(c => c.Id == productinput);

                                    if (orderedproduct != null)
                                    {
                                        Console.Clear();

                                        var neworder = new Order
                                        {
                                            ProductId = productinput,
                                            Name = orderedproduct.Name,
                                            Quantity = 1,
                                            TotalPrice = orderedproduct.Price,//orderedproduct.Price,
                                            OrderDate = DateTime.Now,
                                            Products = new List<Product> { orderedproduct }
                                        };
                                        db.Orders.Add(neworder);
                                        db.SaveChanges();
                                    }
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.Clear();
                                    var errorWindow = new Window("Error", 2, 2, new List<string> { "Invalid category ID." });
                                    errorWindow.Draw();
                                    Console.SetCursorPosition(0, Lowest.LowestPosition + 2);
                                    Console.WriteLine("Press any key to try again...");
                                    Console.ReadKey();
                                }
                            }

                            // if option "Order" selected
                            if (clientinput == 3)
                            {
                                Purchase.ordering();
                            }

                            // if option "Retunr to main menu" selected
                            if (clientinput == 4)
                            {
                                break;
                            }
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                }
            }
        }
    }
}
