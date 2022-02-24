using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    static class PurchaseStorage
    {
        private static List<Purchase> purchases = new List<Purchase>();

        public static void AddPurchase(Purchase purchase)
        {
            purchases.Add(purchase);
        }

        public static List<Purchase> GetAllPurchases()
        {
            return purchases;
        }

        public static void RemovePurchase(Purchase purchase)
        {
            if (purchases.Count > 0) purchases.Remove(purchase);
            else throw new Exception("Список покупок пуст");
        }
    }
}
