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
    internal class FoodItem
    {
        public FoodItem() { }

        public FoodItem(string itemName, string itemDesc, double itemPrice, string customise)
        {
            this.itemName = itemName;
            this.itemDesc = itemDesc;
            this.itemPrice = itemPrice;
            this.customise = customise;
        }

        public string itemName { get; set; }
        public string itemDesc { get; set; }
        public double itemPrice { get; set; }
        public string customise { get; set; }

        public override string ToString()
        {
            return itemName + " - $" + itemPrice;
        }
    }
}
