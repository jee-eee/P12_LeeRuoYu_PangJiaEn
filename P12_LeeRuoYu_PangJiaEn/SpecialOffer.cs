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
    internal class SpecialOffer
    {
        public SpecialOffer() { }

        public SpecialOffer(string offerCode, string offerDesc, double discount)
        {
            this.offerCode = offerCode;
            this.offerDesc = offerDesc;
            this.discount = discount;
        }

        public string offerCode { get; set; }
        public string offerDesc { get; set; }
        public double discount { get; set; }

        public override string ToString()
        {
            return offerCode + " - " + discount + "% off";
        }
    }
}

