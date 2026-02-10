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
    internal class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDateTime { get; set; }

        public double OrderTotal { get; set; }

        public string OrderStatus { get; set; }

        public DateTime DeliveryDateTime { get; set; }

        public string DeliveryAddress { get; set; }

        public string OrderPaymentMethod { get; set; }

        public bool OrderPaid { get; set; }

        public List<OrderedFoodItem> OrderedFoodItems { get; set; }

        public Order()
        {
            OrderedFoodItems = new List<OrderedFoodItem>();
            OrderTotal = 0;
        }

        public Order(int orderId, DateTime orderDateTime, string deliveryAddress, DateTime deliveryDateTime)
        {
            OrderId = orderId;
            OrderDateTime = orderDateTime;
            DeliveryAddress = deliveryAddress;
            DeliveryDateTime = deliveryDateTime;
            OrderStatus = "Pending";
            OrderPaid = false;
            OrderedFoodItems = new List<OrderedFoodItem>();
        }

        public double CalculateOrderTotal()
        {
            OrderTotal = 0;

            foreach (OrderedFoodItem item in OrderedFoodItems)
            {
                OrderTotal += item.CalculateSubTotal();
            }

            return OrderTotal;
        }

        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            OrderedFoodItems.Add(item);
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            return OrderedFoodItems.Remove(item);
        }

        public void DisplayOrderedFoodItem()
        {
            foreach (OrderedFoodItem item in OrderedFoodItems)
            {
                Console.WriteLine(item);
            }
        }

        public override string ToString()
        {
            return "Order ID: " + OrderId + " Delivery Date/Time : " + DeliveryDateTime + " Address: " + DeliveryAddress + " Total: $" + OrderTotal + " Status: " + OrderStatus;
        }
    }
}
