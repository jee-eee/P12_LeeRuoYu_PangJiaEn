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
    internal class Customer
    {
        public string EmailAddress { get; set; }

        public string CustomerName { get; set; }

        private List<Order> orders;

        public Customer()
        {
            orders = new List<Order>();
        }

        public Customer(string emailAddress, string customerName)
        {
            EmailAddress = emailAddress;
            CustomerName = customerName;
            orders = new List<Order>();
        }

        public void AddOrder(Order order)
        {
            orders.Add(order);
        }

        public void DisplayAllOrders()
        {
            foreach (Order order in orders)
            {
                Console.WriteLine(order);
            }
        }
        public bool RemoveOrder(Order order)
        {
            return orders.Remove(order);
        }

        public override string ToString()
        {
            return "Customer Name: " + CustomerName + " Email: " + EmailAddress;
        }
    }
}
