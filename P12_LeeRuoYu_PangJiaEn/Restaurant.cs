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
    internal class Restaurant
    {
        public Restaurant()
        {
            menus = new List<Menu>();
            specialOffers = new List<SpecialOffer>();
            orders = new List<Order>();
        }

        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            this.restaurantId = restaurantId;
            this.restaurantName = restaurantName;
            this.restaurantEmail = restaurantEmail;

            menus = new List<Menu>();
            specialOffers = new List<SpecialOffer>();
            orders = new List<Order>();
        }

        public string restaurantId { get; set; }
        public string restaurantName { get; set; }
        public string restaurantEmail { get; set; }

        public List<Menu> menus { get; set; }
        public List<SpecialOffer> specialOffers { get; set; }
        public List<Order> orders { get; set; }

        public void AddMenu(Menu menu)
        {
            menus.Add(menu);
        }

        public bool RemoveMenu(Menu menu)
        {
            return menus.Remove(menu);
        }

        public void DisplayMenu()
        {
            foreach (Menu menu in menus)
                Console.WriteLine(menu);
        }

        public void DisplaySpecialOffers()
        {
            foreach (SpecialOffer offer in specialOffers)
                Console.WriteLine(offer);
        }

        public void DisplayOrders()
        {
            foreach (Order order in orders)
                Console.WriteLine(order);
        }

        public override string ToString()
        {
            return restaurantName + " (" + restaurantEmail + ")";
        }
    }
}
