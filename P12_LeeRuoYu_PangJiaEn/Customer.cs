//==========================================================
// Student Number : S10269305E
// Student Name : Pang Jia En
// Partner Name : Lee Ruo Yu
//==========================================================
using P12_Lee_RuoYu_PangJiaEn;
using System;
using System.Collections.Generic;

namespace P12_Lee_RuoYu_PangJiaEn
{
    internal class Customer
    {
        public string EmailAddress { get; set; }
        public string CustomerName { get; set; }

        private List<Order> orders;
        public IReadOnlyList<Order> Orders => orders;

        public List<string> FavouriteOrders { get; set; } = new List<string>();

        public Customer()
        {
            orders = new List<Order>();
            FavouriteOrders = new List<string>();
        }

        public Customer(string email, string name)
        {
            EmailAddress = email;
            CustomerName = name;
            orders = new List<Order>();
            FavouriteOrders = new List<string>();
        }


        public void AddOrder(Order order)
        {
            if (order == null) return;

            if (!orders.Contains(order))
                orders.Add(order);
        }

        public bool RemoveOrder(Order order)
        {
            return orders.Remove(order);
        }

        public void DisplayAllOrders()
        {
            foreach (Order o in orders)
            {
                Console.WriteLine($"Order ID: {o.OrderId} | Status: {o.OrderStatus}");
            }
        }
        public override string ToString()
        {
            return $"Customer Name: {CustomerName} | Email: {EmailAddress}";
        }
    }
}

