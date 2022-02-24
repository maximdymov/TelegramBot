using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    class Purchase
    {
        public string Name { get;}

        private double price;
        public double Price 
        {   get
            {
                return price;
            }
            set
            {
                if (value >= 0) price = value;
            }
        }

        public string Category { get; }

        public Purchase(string name, double price, string categoryName)
        {
            Name = name;
            Price = price;
            Category = categoryName;
            CategoryStorage.AddCategory(Category);
        }
    }
}
