//==========================================================
// Student Number : S10269305E
// Student Name : Pang Jia En
// Partner Name : Lee Ruo Yu
//==========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class OrderedFoodItem
    {
        public FoodItem FoodItem { get; set; }

        public int QtyOrdered { get; set; }

        public double subTotal { get; set; }

        public OrderedFoodItem() { }

        public OrderedFoodItem(FoodItem foodItem, int qtyOrdered)
        {
            FoodItem = foodItem;
            QtyOrdered = qtyOrdered;
            subTotal = 0;
        }

        public double CalculateSubTotal()
        {
            subTotal = FoodItem.itemPrice * QtyOrdered;
            return subTotal;
        }

        public override string ToString()
        {
            return "Item: " + FoodItem.itemName + "  Qty: " + QtyOrdered + "  Price: $" + FoodItem.itemPrice + "  SubTotal: $" + subTotal;
        }
    }
}
