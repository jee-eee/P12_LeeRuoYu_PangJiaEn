//==========================================================
// Student Number : S10273008B
// Student Name : Lee Ruo Yu
// Partner Name : Pang Jia En
//==========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Menu
    {
        public Menu()
        {
            foodItems = new List<FoodItem>();
        }

        public Menu(string menuId, string menuName)
        {
            this.menuId = menuId;
            this.menuName = menuName;
            foodItems = new List<FoodItem>();
        }

        public string menuId { get; set; }
        public string menuName { get; set; }

        public List<FoodItem> foodItems { get; set; }

        public void AddFoodItem(FoodItem item)
        {
            foodItems.Add(item);
        }

        public bool RemoveFoodItem(FoodItem item)
        {
            return foodItems.Remove(item);
        }
        public void DisplayFoodItems()
        {
            foreach (FoodItem item in foodItems)
            {
                Console.WriteLine($" - {item.itemName}: {item.itemDesc} - ${item.itemPrice:0.00}");
            }
        }
        public override string ToString()
        {
            return menuName + " (" + foodItems.Count + " items)";
        }
    }
}