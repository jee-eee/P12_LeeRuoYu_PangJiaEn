using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;


//Start of program

//Main Menu
void DisplayMenu()
{
    Console.WriteLine("=====Gruberoo Food Delivery System=====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all order");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Process an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("0. Exit");
}

void RunMenu()
{
    int input = -1;

    while (input != 0)
    {
        DisplayMenu();
        Console.Write("Enter your choice: ");
        string? choiceText = Console.ReadLine();
        if (!int.TryParse(choiceText, out input))
        {
            Console.WriteLine("Invalid choice. Please enter a number.");
            Console.WriteLine();
            continue;
        }
        Console.WriteLine();
        if (input == 1)
        {
            ListRestaurants();
        }
        else if (input == 2)
        {
            ListOrder();
        }
        else if (input == 3)
        {
            CreateOrder();
        }

        else if (input == 4)
        {
            ProcessOrder();
        }

        else if (input == 5)
        {
            ModifyOrder();
        }

        else if (input == 6)
        {
            DeleteOrder();
        }

        else if (input == 0)
        {
            break;
        }

        else
        {
            Console.WriteLine("Invalid choice. Please try again.");
        }
    }
}
//Q1 
//Student Name:Lee Ruo Yu
//Student Number: S10273008B
string DataPath(string fileName) => Path.Combine(AppContext.BaseDirectory, fileName);
List<Restaurant> restaurants = LoadRestaurants(DataPath("restaurants.csv"));
LoadFoodItems(DataPath("fooditems.csv"), restaurants);
List<Restaurant> LoadRestaurants(string filePath)
{
    List<Restaurant> restaurants = new List<Restaurant>();
    if (!File.Exists(filePath))
    {
        Console.WriteLine($"[Q1] Missing file: {filePath}");
        return restaurants; // return empty list instead of crashing
    }

    var lines = File.ReadAllLines(filePath);

    foreach (var line in lines.Skip(1)) // dont include header
    {
        var fields = line.Split(',');

        if (fields.Length >= 3) //id name email header
        {
            string restaurantId = fields[0].Trim();
            string restaurantName = fields[1].Trim();
            string restaurantEmail = fields[2].Trim();

            Restaurant r = new Restaurant(restaurantId, restaurantName, restaurantEmail);

            Menu defaultMenu = new Menu("M" + restaurantId, "Main Menu");
            r.AddMenu(defaultMenu);

            restaurants.Add(r);
        }
    }

    return restaurants;
}

void LoadFoodItems(string filePath, List<Restaurant> restaurants)
{
    if (!File.Exists(filePath))
    {
        Console.WriteLine($"[Q1] Missing file: {filePath}");
        return;
    }
    var lines = File.ReadAllLines(filePath);

    foreach (var line in lines.Skip(1)) // skip header
    {
        var fields = line.Split(',');

        if (fields.Length >= 4) // id name desc and price
        {
            string restaurantId = fields[0].Trim();
            string itemName = fields[1].Trim();
            string itemDesc = fields[2].Trim();
            double itemPrice = double.Parse(fields[3].Trim());

            FoodItem foodItem = new FoodItem(itemName, itemDesc, itemPrice, "");

            // find and add food item to menu
            Restaurant? r = restaurants.FirstOrDefault(x => x.restaurantId == restaurantId);
            if (r != null)
            {
                if (r.menus.Count == 0)
                {
                    r.AddMenu(new Menu("M" + restaurantId, "Main Menu"));
                }

                r.menus[0].AddFoodItem(foodItem);
            }
        }
    }
}

//Q2 
//Student Number:S10269305E
//Student Name:Pang Jia En
List<Customer> customers = LoadCustomers("customers.csv");
List<Order> orders = LoadOrders("orders.csv", customers, restaurants);
Stack<Order> refundStack = new Stack<Order>();

try
{
    RunMenu();
}
catch (Exception ex)
{
    Console.WriteLine("Program crashed:");
    Console.WriteLine(ex);
}

Console.WriteLine("\nPress ENTER to exit...");
Console.ReadLine();

List<Customer> LoadCustomers(string filePath)
{
    List<Customer> customers = new List<Customer>();
    if (!File.Exists(filePath))
    {
        Console.WriteLine($"[Q2] Missing file: {filePath}");
        return customers;
    }
    var lines = File.ReadAllLines(filePath);
    foreach (var line in lines.Skip(1)) // Skip header line
    {
        var fields = line.Split(',');
        if (fields.Length < 2) continue;

        string name = fields[0].Trim();
        string email = fields[1].Trim();

        Customer customer = new Customer(email, name);
        customers.Add(customer);
    }
    return customers;
}

List<Order> LoadOrders(string filePath, List<Customer> customers, List<Restaurant> restaurants)
{
    List<Order> orders = new List<Order>();
    if (!File.Exists(filePath))
    {
        Console.WriteLine($"[Q2] Missing file: {filePath}");
        return orders;
    }
    var lines = File.ReadAllLines(filePath);

    foreach (var line in lines.Skip(1))
    {
        var fields = line.Split(',');
        if (fields.Length < 6) continue;
        int orderId = int.Parse(fields[0].Trim());
        string customerEmail = fields[1].Trim();
        string restaurantId = fields[2].Trim();
        DateTime orderDateTime = DateTime.Parse(fields[3].Trim());
        DateTime deliveryDateTime = DateTime.Parse(fields[4].Trim());
        string deliveryAddress = fields[5].Trim();

        Order order = new Order(orderId, orderDateTime, deliveryAddress, deliveryDateTime);
        orders.Add(order);

        Customer? customer = customers.Find(c => c.EmailAddress == customerEmail);
        if (customer != null)
        {
            customer.AddOrder(order);
        }
        Restaurant? restaurant = restaurants.Find(r => r.restaurantId == restaurantId);
        if (restaurant != null)
        {
            restaurant.orders.Add(order);
        }
    }
    return orders;
}

//Q3 
//Student Number:S10269305E
//Student Name:Pang Jia En
void ListRestaurants()
{
    Console.WriteLine("All Restaurants and Menu Items");
    Console.WriteLine("==============================");

    foreach (Restaurant r in restaurants)
    {
        Console.WriteLine($"Restaurant: {r.restaurantName} ({r.restaurantId})");

        if (r.menus.Count > 0)
        {
            foreach (Menu m in r.menus)
            {
                Console.WriteLine($" Menu: {m.menuName}");
                foreach (FoodItem fi in m.foodItems)
                {
                    Console.WriteLine($"  - {fi.itemName}: {fi.itemDesc} - ${fi.itemPrice:0.00}");
                }
            }
        }
        else
        {
            Console.WriteLine(" (No menus)");
        }
    }
    Console.WriteLine();
}
//Q4 
//Student Name:Lee Ruo Yu
//Student Number: S10273008B
void ListOrder()
{
    Console.WriteLine("All Orders");
    Console.WriteLine("==========");

    if (orders.Count == 0)
    {
        Console.WriteLine("No orders found.");
        Console.WriteLine();
        return;
    }

    // OrderId -> (custEmail, restId, total, status)
    Dictionary<int, (string custEmail, string restId, double total, string status)> orderInfo
        = new Dictionary<int, (string, string, double, string)>();

    if (File.Exists("orders.csv"))
    {
        var lines = File.ReadAllLines("orders.csv");

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            // CSV split (supports quotes)
            List<string> fields = new List<string>();
            bool inQuotes = false;
            string cur = "";

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];

                if (ch == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        cur += '"';
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    fields.Add(cur);
                    cur = "";
                }
                else
                {
                    cur += ch;
                }
            }
            fields.Add(cur);

            if (fields.Count < 3) continue;
            if (!int.TryParse(fields[0].Trim(), out int id)) continue;

            string custEmail = fields[1].Trim();
            string restId = fields[2].Trim();

            // ---- status detect ----
            string status = "Unknown";
            string[] known = { "Pending", "Cancelled", "Preparing", "Delivered", "Rejected" };
            foreach (string f in fields)
            {
                string s = f.Trim();
                if (known.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)))
                {
                    status = char.ToUpper(s[0]) + s.Substring(1).ToLower();
                    break;
                }
            }

            // ---- total detect ----
            // total should be a decimal number (usually near end)
            double total = 0;
            for (int k = fields.Count - 1; k >= 0; k--)
            {
                string t = fields[k].Trim().TrimStart('$');
                if (double.TryParse(t, out double val))
                {
                    total = val;
                    break;
                }
            }

            orderInfo[id] = (custEmail, restId, total, status);
        }
    }

    Console.WriteLine("Order ID  Customer Email                  Restaurant ID  Delivery Date/Time        Address                   Total     Status");
    Console.WriteLine("--------  ------------------------------  ------------   --------------------      ------------------------  --------  --------");

    foreach (Order o in orders)
    {
        string custEmail = "Unknown";
        string restId = "Unknown";
        double total = o.OrderTotal;      // fallback
        string status = o.OrderStatus;    // fallback

        if (orderInfo.ContainsKey(o.OrderId))
        {
            custEmail = orderInfo[o.OrderId].custEmail;
            restId = orderInfo[o.OrderId].restId;
            total = orderInfo[o.OrderId].total;
            status = orderInfo[o.OrderId].status;

            // also update object so other menus show correct values
            o.OrderTotal = total;
            o.OrderStatus = status;
        }

        Console.WriteLine($"{o.OrderId,-8}  {custEmail,-30}  {restId,-12}  {o.DeliveryDateTime:dd/MM/yyyy HH:mm}  {o.DeliveryAddress,-28}  {($"${total:0.00}"),10}  {status,-10}");
    }

    Console.WriteLine();
}

//Q5 
//Student Number:S10269305E
//Student Name:Pang Jia En
void CreateOrder()
{
    Console.WriteLine("Create New Order");
    Console.WriteLine("================");
    Console.Write("Enter Customer Email: ");
    string customerEmail = Console.ReadLine() ?? "";
    Console.Write("Enter Restaurant ID: ");
    string restaurantId = (Console.ReadLine() ?? "").Trim().ToUpper();
    Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
    DateTime deliveryData = DateTime.Parse(Console.ReadLine()!);
    Console.Write("Enter Delivery Time (hh:mm): ");
    DateTime deliveryTime = DateTime.Parse(Console.ReadLine()!);
    Console.Write("Enter Delivery Address: ");
    string deliveryAddress = Console.ReadLine() ?? "";

    Restaurant? selectedRestaurant = restaurants.Find(r => r.restaurantId == restaurantId);
    if (selectedRestaurant == null)
    {
        Console.WriteLine("Invalid Restaurant ID.");
        return;
    }

    List<FoodItem> availableItems = new List<FoodItem>();
    int itemNumber = 1;

    Console.WriteLine("Available Food Items: ");
    foreach (Menu menu in selectedRestaurant.menus)
    {
        foreach (FoodItem item in menu.foodItems)
        {
            Console.WriteLine($"{itemNumber}. {item.itemName} - ${item.itemPrice:0.00}");
            availableItems.Add(item);
            itemNumber++;
        }
    }

    double total = 0.0;
    while (true)
    {
        Console.Write("Enter item number (0 to finish): ");
        if (!int.TryParse(Console.ReadLine(), out int choice))
        {
            Console.WriteLine("Invalid input. Please enter a number.");
            continue;
        }

        if (choice == 0) break;

        if (choice < 1 || choice > availableItems.Count)
        {
            Console.WriteLine("Invalid item number.");
            continue;
        }

        Console.Write("Enter quantity: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            Console.WriteLine("Invalid quantity.");
            continue;
        }

        FoodItem selectedItem = availableItems[choice - 1];
        total += selectedItem.itemPrice * quantity;
    }


    string specialRequest = "";
    Console.Write("Add special request? [Y/N]: ");
    string specialChoice = (Console.ReadLine() ?? "").Trim();

    if (specialChoice.ToUpper() == "Y")
    {
        Console.Write("Enter special request: ");
        specialRequest = Console.ReadLine()!;
    }

    double deliveryfee = 5.00;
    Console.WriteLine();
    Console.WriteLine($"Order Total: ${total:0.00} + ${deliveryfee:0.00} (delivery) = ${(total + deliveryfee):0.00}");

    Console.Write("Proceed to payment? [Y/N]: ");
    string proceedPayment = (Console.ReadLine() ?? "").Trim();
    if (proceedPayment.ToUpper() != "Y")
    {
        Console.WriteLine("Order cancelled.");
        return;
    }

    Console.WriteLine("Payment method:");
    Console.Write("[CC Credit Card / [PP] Paypal / [CD] Cash on Delivery: ");
    string paymentMethod = (Console.ReadLine() ?? "").Trim().ToUpper();

    //create order
    int maxId = 1000;
    foreach (Order o in orders)
    {
        if (o.OrderId > maxId) maxId = o.OrderId;
    }
    int newOrderId = maxId + 1;
    Order newOrder = new Order(newOrderId, DateTime.Now, deliveryAddress, deliveryData.AddHours(deliveryTime.Hour).AddMinutes(deliveryTime.Minute));

    newOrder.OrderStatus = "Pending";
    newOrder.OrderPaymentMethod = paymentMethod;

    //add to customer
    Customer? customer = customers.Find(c => c.EmailAddress == customerEmail);
    if (customer != null)
    {
        customer.AddOrder(newOrder);
    }

    //add to restaurant
    selectedRestaurant.orders.Add(newOrder);

    string csvline = $"{newOrderId},{customerEmail},{restaurantId},{DateTime.Now},{deliveryData.AddHours(deliveryTime.Hour).AddMinutes(deliveryTime.Minute)},{deliveryAddress},{paymentMethod},Pending";
    File.AppendAllText("orders.csv", csvline + Environment.NewLine);

    Console.WriteLine($"Order {newOrderId} created successfully! Status:Preaparing ");
}

//Q6 
//Student Name:Lee Ruo Yu
//Student Number: S10273008B:
void ProcessOrder()
{
    Console.WriteLine("Process Order");
    Console.WriteLine("=============");

    Console.Write("Enter Restaurant ID: ");
    string restaurantId = (Console.ReadLine() ?? "").Trim().ToUpper();

    Restaurant? r = restaurants.Find(x => x.restaurantId.Equals(restaurantId, StringComparison.OrdinalIgnoreCase));
    if (r == null)
    {
        Console.WriteLine("Invalid Restaurant ID.");
        Console.WriteLine();
        return;
    }

    // OrderId -> (CustomerEmail, RestaurantId, Total, Status, ItemsString)
    Dictionary<int, (string custEmail, string restId, double total, string status, string items)> orderInfo
        = new Dictionary<int, (string, string, double, string, string)>();

    if (File.Exists("orders.csv"))
    {
        var lines = File.ReadAllLines("orders.csv");

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            // ===== inline CSV split that supports quotes =====
            List<string> fields = new List<string>();
            bool inQuotes = false;
            string cur = "";

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];

                if (ch == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        cur += '"';
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    fields.Add(cur);
                    cur = "";
                }
                else
                {
                    cur += ch;
                }
            }
            fields.Add(cur);
            // ===== end CSV split =====

            if (fields.Count < 3) continue;

            if (!int.TryParse(fields[0].Trim(), out int id)) continue;

            string custEmail = fields[1].Trim();
            string restId = fields[2].Trim();

            // Try to get Total/Status/Items (works for both old & new csv layouts)
            double total = 0;
            string status = "";
            string items = "";

            // If your orders.csv is the "new" format (TotalAmount at index 7, Status at 8, Items at 9)
            if (fields.Count >= 9)
            {
                // safest: try parse total from any field, prefer later fields
                for (int k = fields.Count - 1; k >= 0; k--)
                {
                    if (double.TryParse(fields[k].Trim(), out double t))
                    {
                        total = t;
                        break;
                    }
                }

                // status usually one of these
                string[] known = { "Pending", "Cancelled", "Preparing", "Delivered", "Rejected" };
                foreach (string f in fields)
                {
                    string s = f.Trim();
                    if (known.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)))
                    {
                        status = char.ToUpper(s[0]) + s.Substring(1).ToLower(); // normalise case
                        break;
                    }
                }

                // items usually last column
                if (fields.Count >= 10)
                    items = fields[fields.Count - 1].Trim().Trim('"');
            }

            orderInfo[id] = (custEmail, restId, total, status, items);
        }
    }

    // Filter orders for this restaurant based on CSV (so we get the correct restaurant mapping)
    List<Order> restOrders = new List<Order>();
    foreach (Order o in orders)
    {
        if (orderInfo.ContainsKey(o.OrderId) &&
            orderInfo[o.OrderId].restId.Equals(restaurantId, StringComparison.OrdinalIgnoreCase))
        {
            restOrders.Add(o);
        }
    }

    if (restOrders.Count == 0)
    {
        Console.WriteLine("No orders found for this restaurant.");
        Console.WriteLine();
        return;
    }

    restOrders = restOrders.OrderBy(x => x.DeliveryDateTime).ToList();

    foreach (Order o in restOrders)
    {
        // Get customer name
        string custEmail = orderInfo.ContainsKey(o.OrderId) ? orderInfo[o.OrderId].custEmail : "";
        string custName = "Unknown";
        if (!string.IsNullOrWhiteSpace(custEmail))
        {
            Customer? c = customers.FirstOrDefault(x =>
                x.EmailAddress.Equals(custEmail, StringComparison.OrdinalIgnoreCase));
            if (c != null) custName = c.CustomerName;
        }

        // Apply status & total from CSV so it doesn't show 0
        if (orderInfo.ContainsKey(o.OrderId))
        {
            o.OrderTotal = orderInfo[o.OrderId].total;
            if (!string.IsNullOrWhiteSpace(orderInfo[o.OrderId].status))
                o.OrderStatus = orderInfo[o.OrderId].status;

            // Build ordered items from CSV ONLY if list is empty
            if (o.OrderedFoodItems.Count == 0 && !string.IsNullOrWhiteSpace(orderInfo[o.OrderId].items))
            {
                string itemsStr = orderInfo[o.OrderId].items;

                // expected format: Chicken Rice,2|Beef Burger,1
                string[] parts = itemsStr.Split('|', StringSplitOptions.RemoveEmptyEntries);
                foreach (string raw in parts)
                {
                    string part = raw.Trim();
                    if (part.Length == 0) continue;

                    int commaIndex = part.LastIndexOf(',');
                    if (commaIndex <= 0) continue;

                    string itemName = part.Substring(0, commaIndex).Trim();
                    string qtyText = part.Substring(commaIndex + 1).Trim();
                    if (!int.TryParse(qtyText, out int qty)) qty = 0;

                    // Find item in restaurant menu (so name matches)
                    FoodItem? found = null;
                    foreach (var m in r.menus)
                    {
                        found = m.foodItems.FirstOrDefault(x =>
                            x.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                        if (found != null) break;
                    }
                    if (found == null) found = new FoodItem(itemName, "", 0, "");

                    o.OrderedFoodItems.Add(new OrderedFoodItem(found, qty));
                }
            }
        }

        // ===== DISPLAY (match pic 2) =====
        Console.WriteLine();
        Console.WriteLine($"Order {o.OrderId}:");
        Console.WriteLine($"Customer: {custName}");
        Console.WriteLine("Ordered Items:");

        if (o.OrderedFoodItems.Count == 0)
        {
            Console.WriteLine(" - (No items)");
        }
        else
        {
            int count = 1;
            foreach (OrderedFoodItem ofi in o.OrderedFoodItems)
            {
                Console.WriteLine($"{count}. {ofi.FoodItem.itemName} - {ofi.QtyOrdered}");
                count++;
            }
        }

        Console.WriteLine($"Delivery date/time: {o.DeliveryDateTime:dd/MM/yyyy  HH:mm}");
        Console.WriteLine($"Total Amount: ${o.OrderTotal:0.00}");
        Console.WriteLine($"Order Status: {o.OrderStatus}");
        Console.WriteLine();

        Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
        string action = (Console.ReadLine() ?? "").Trim().ToUpper();

        if (action == "C")
        {
            if (o.OrderStatus == "Pending")
            {
                o.OrderStatus = "Preparing";
                Console.WriteLine($"\nOrder {o.OrderId} confirmed. Status: Preparing");
            }
            else
            {
                Console.WriteLine("\nCannot confirm. Only Pending orders can be confirmed.");
            }
        }
        else if (action == "R")
        {
            if (o.OrderStatus == "Pending")
            {
                o.OrderStatus = "Rejected";
                refundStack.Push(o);
                Console.WriteLine($"\nOrder {o.OrderId} rejected. Status: Rejected");
            }
            else
            {
                Console.WriteLine("\nCannot reject. Only Pending orders can be rejected.");
            }
        }
        else if (action == "S")
        {
            if (o.OrderStatus == "Cancelled")
            {
                Console.WriteLine($"\nOrder {o.OrderId} skipped.");
            }
            else
            {
                Console.WriteLine("\nCannot skip. Only Cancelled orders can be skipped.");
            }
        }
        else if (action == "D")
        {
            if (o.OrderStatus == "Preparing")
            {
                o.OrderStatus = "Delivered";
                Console.WriteLine($"\nOrder {o.OrderId} delivered. Status: Delivered");
            }
            else
            {
                Console.WriteLine("\nCannot deliver. Only Preparing orders can be delivered.");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid action. Moving to next order...");
        }
    }

    Console.WriteLine();
}


//Q7 
//Student Number:S10269305E
//Student Name:Pang Jia En
void ModifyOrder()
{
    Console.WriteLine("Modify Order");
    Console.WriteLine("============");
    Console.Write("Enter Customer Email: ");
    string custEmail = (Console.ReadLine() ?? "").Trim();

    Customer? cust = customers.Find(c => c.EmailAddress.Equals(custEmail, StringComparison.OrdinalIgnoreCase));
    if (cust == null)
    {
        Console.WriteLine("Customer not found.");
        return;
    }

    // Build from orders.csv:
    // OrderId -> (email, status, items)
    Dictionary<int, (string email, string status, string items)> csvMap
        = new Dictionary<int, (string, string, string)>();

    if (File.Exists("orders.csv"))
    {
        var lines = File.ReadAllLines("orders.csv");

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            // CSV split (supports quotes)
            List<string> fields = new List<string>();
            bool inQuotes = false;
            string cur = "";

            for (int i = 0; i < line.Length; i++)
            {
                char ch = line[i];

                if (ch == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        cur += '"';
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (ch == ',' && !inQuotes)
                {
                    fields.Add(cur);
                    cur = "";
                }
                else
                {
                    cur += ch;
                }
            }
            fields.Add(cur);

            if (fields.Count < 3) continue;

            if (!int.TryParse(fields[0].Trim(), out int id)) continue;

            string email = fields[1].Trim();

            // detect status from any column (because your orders.csv layout may differ)
            string status = "";
            string[] known = { "Pending", "Cancelled", "Preparing", "Delivered", "Rejected" };
            foreach (string f in fields)
            {
                string s = f.Trim();
                if (known.Any(x => x.Equals(s, StringComparison.OrdinalIgnoreCase)))
                {
                    status = char.ToUpper(s[0]) + s.Substring(1).ToLower();
                    break;
                }
            }

            // items normally last column IF your file stores it
            // only treat it as items if it looks like your format (has | and a comma for qty)
            string items = "";
            if (fields.Count >= 7)
            {
                string last = fields[fields.Count - 1].Trim().Trim('"');
                if (last.Contains("|") && last.Contains(","))
                    items = last;
            }

            csvMap[id] = (email, status, items);
        }
    }

    // Pending Orders list should be based on CSV email + Pending
    List<int> pendingIds = new List<int>();

    foreach (var kvp in csvMap)
    {
        int id = kvp.Key;
        var info = kvp.Value;

        if (!info.email.Equals(custEmail, StringComparison.OrdinalIgnoreCase)) continue;
        if (!info.status.Equals("Pending", StringComparison.OrdinalIgnoreCase)) continue;

        // to match sample (remove 36/37 etc)
        if (id < 1000) continue;

        pendingIds.Add(id);
    }

    pendingIds = pendingIds.OrderBy(x => x).ToList();

    Console.WriteLine("Pending Orders:");
    foreach (int id in pendingIds)
    {
        Console.WriteLine(id); // exactly like sample (no "Order ID:")
    }

    if (pendingIds.Count == 0)
    {
        Console.WriteLine("No pending orders.");
        return;
    }

    int orderId;
    while (true)
    {
        Console.Write("Enter Order ID: ");
        if (!int.TryParse(Console.ReadLine(), out orderId))
        {
            Console.WriteLine("Invalid Order ID.");
            continue;
        }

        if (!pendingIds.Contains(orderId))
        {
            Console.WriteLine("Order ID not in pending list.");
            continue;
        }

        break;
    }

    // Find the Order object
    Order? selectedOrder = orders.Find(o => o.OrderId == orderId);
    if (selectedOrder == null)
    {
        Console.WriteLine("Order not found in system.");
        return;
    }

    // Fill items from CSV into OrderedFoodItems if empty
    if (selectedOrder.OrderedFoodItems.Count == 0 &&
        csvMap.ContainsKey(orderId) &&
        !string.IsNullOrWhiteSpace(csvMap[orderId].items))
    {
        string itemsStr = csvMap[orderId].items;

        // format: Chicken Rice,2|Beef Burger,1
        string[] parts = itemsStr.Split('|', StringSplitOptions.RemoveEmptyEntries);

        foreach (string raw in parts)
        {
            string part = raw.Trim();
            if (part.Length == 0) continue;

            int commaIndex = part.LastIndexOf(',');
            if (commaIndex <= 0) continue;

            string itemName = part.Substring(0, commaIndex).Trim();
            string qtyText = part.Substring(commaIndex + 1).Trim();
            if (!int.TryParse(qtyText, out int qty)) qty = 0;

            // find matching FoodItem in any menu (so name is correct)
            FoodItem? found = null;
            foreach (var rr in restaurants)
            {
                foreach (var m in rr.menus)
                {
                    found = m.foodItems.FirstOrDefault(x =>
                        x.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                    if (found != null) break;
                }
                if (found != null) break;
            }
            if (found == null) found = new FoodItem(itemName, "", 0, "");

            selectedOrder.OrderedFoodItems.Add(new OrderedFoodItem(found, qty));
        }
    }

    // ===== Display like sample =====
    Console.WriteLine("\nOrder Items:");
    if (selectedOrder.OrderedFoodItems.Count == 0)
    {
        Console.WriteLine(" - (No items)");
    }
    else
    {
        int count = 1;
        foreach (OrderedFoodItem ofi in selectedOrder.OrderedFoodItems)
        {
            Console.WriteLine($"{count}. {ofi.FoodItem.itemName} - {ofi.QtyOrdered}");
            count++;
        }
    }

    Console.WriteLine("Address:");
    Console.WriteLine(selectedOrder.DeliveryAddress);

    Console.WriteLine("Delivery Date/Time:");
    Console.WriteLine($"{selectedOrder.DeliveryDateTime:dd/MM/yyyy, HH:mm}");

    int choice;
    while (true)
    {
        Console.Write("\nModify: [1] Items [2] Address [3] Delivery Time: ");
        if (int.TryParse(Console.ReadLine(), out choice) && (choice == 1 || choice == 2 || choice == 3))
            break;
        Console.WriteLine("Invalid choice.");
    }

    if (choice == 1)
    {
        Console.Write("Enter new Items: ");
        string item = Console.ReadLine() ?? "";
        Console.WriteLine($"\nOrder {orderId} updated. New Items: {item}");
    }
    else if (choice == 2)
    {
        Console.Write("Enter new Address: ");
        string newAddress = Console.ReadLine() ?? "";
        selectedOrder.DeliveryAddress = newAddress;

        Console.WriteLine($"\nOrder {orderId} updated. New Address: {newAddress}");
    }
    else // choice == 3
    {
        TimeSpan newTime;
        while (true)
        {
            Console.Write("Enter new Delivery Time (hh:mm): ");
            if (TimeSpan.TryParse(Console.ReadLine(), out newTime)) break;
            Console.WriteLine("Invalid time. Please enter hh:mm.");
        }

        selectedOrder.DeliveryDateTime = selectedOrder.DeliveryDateTime.Date + newTime;
        Console.WriteLine($"\nOrder {orderId} updated. New Delivery Time: {selectedOrder.DeliveryDateTime:HH:mm}");
    }
}

//Q8 
//Student Name:Lee Ruo Yu
//Student Number: S10273008B
void DeleteOrder()
{
    Console.WriteLine("Delete Order");
    Console.WriteLine("============");

    Console.Write("Enter Customer Email: ");
    string custEmail = Console.ReadLine();

    // Build lookup from orders.csv: OrderId -> (customerEmail, restaurantId)
    Dictionary<int, (string custEmail, string restId)> orderInfo = new Dictionary<int, (string, string)>();
    if (File.Exists("orders.csv"))
    {
        var lines = File.ReadAllLines("orders.csv");
        foreach (var line in lines.Skip(1))
        {
            var fields = line.Split(',');
            if (fields.Length >= 3)
            {
                int id;
                if (int.TryParse(fields[0].Trim(), out id))
                {
                    string email = fields[1].Trim();
                    string restId = fields[2].Trim();
                    orderInfo[id] = (email, restId);
                }
            }
        }
    }

    // Find pending orders for this customer (by matching email in CSV)
    List<Order> pendingOrders = new List<Order>();

    foreach (Order o in orders)
    {
        if (orderInfo.ContainsKey(o.OrderId))
        {
            if (orderInfo[o.OrderId].custEmail == custEmail && o.OrderStatus == "Pending")
            {
                pendingOrders.Add(o);
            }
        }
    }

    if (pendingOrders.Count == 0)
    {
        Console.WriteLine("No pending orders found for this customer.");
        Console.WriteLine();
        return;
    }

    Console.WriteLine("\nPending Orders:");
    foreach (Order o in pendingOrders)
    {
        Console.WriteLine($"Order ID: {o.OrderId}");
    }

    Console.Write("\nEnter Order ID to delete: ");
    int orderId = Convert.ToInt32(Console.ReadLine());

    Order target = pendingOrders.Find(o => o.OrderId == orderId);
    if (target == null)
    {
        Console.WriteLine("Invalid Order ID (not found in pending list).");
        Console.WriteLine();
        return;
    }

    Console.WriteLine($"\nOrder ID: {target.OrderId}");
    Console.WriteLine("Ordered Items:");

    if (target.OrderedFoodItems.Count == 0)
    {
        Console.WriteLine(" - (No items)");
    }
    else
    {
        int count = 1;
        foreach (OrderedFoodItem ofi in target.OrderedFoodItems)
        {
            Console.WriteLine($"{count}. {ofi.FoodItem.itemName} - {ofi.QtyOrdered}");
            count++;
        }
    }

    Console.WriteLine($"Delivery Date/Time: {target.DeliveryDateTime:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Delivery Address: {target.DeliveryAddress}");
    Console.WriteLine($"Total Amount: ${target.OrderTotal:0.00}");
    Console.WriteLine($"Status: {target.OrderStatus}");

    Console.Write("\nConfirm deletion? [Y/N]: ");
    string confirm = Console.ReadLine().ToUpper();

    if (confirm == "Y")
    {
        target.OrderStatus = "Cancelled";
        refundStack.Push(target);
        Console.WriteLine($"Order {target.OrderId} cancelled. Refund processed: ${target.OrderTotal:0.00}");
    }
    else
    {
        Console.WriteLine("Deletion cancelled.");
    }

    Console.WriteLine();
}