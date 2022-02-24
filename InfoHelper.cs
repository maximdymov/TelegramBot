using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public class InfoHelper
    {
        public static string ShowAllPurchases()
        {
            var purchases = PurchaseStorage.GetAllPurchases();
            var purchaseList = new StringBuilder();

            if (purchases.Count == 0) return "Нет покупок";

            for (int i = 0; i < purchases.Count; i++)
            {
                purchaseList.Append($"{i + 1}). ");
                purchaseList.Append(purchases[i].Name);
                purchaseList.Append(", ");
                purchaseList.Append(purchases[i].Price);
                purchaseList.Append(" руб., Категория : ");
                purchaseList.Append(purchases[i].Category.Name);
                purchaseList.Append('\n');
            }

            return purchaseList.ToString();
        }


    }
}
