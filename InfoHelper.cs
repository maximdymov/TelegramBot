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
                purchaseList.Append(purchases[i].Category);
                purchaseList.Append('\n');
            }

            return purchaseList.ToString();
        }

        public static string ShowAllCategories()
        {
            var categories = CategoryStorage.GetAllCategories().ToList();
            var categoryList = new StringBuilder();

            if (categories.Count == 0) return "Нет категорий";

            for (int i = 0; i < categories.Count; i++)
            {
                categoryList.Append($"{i + 1}). ");
                categoryList.Append(categories[i]);
                categoryList.Append('\n');
            }

            return categoryList.ToString();

        }

        private static List<Purchase> GetPurchasesInCategory(string category)
        {
            var list = new List<Purchase>();
            for (int i = 0; i < PurchaseStorage.GetAllPurchases().Count; i++)
            {
                if (PurchaseStorage.GetAllPurchases()[i].Category == category)
                {
                    list.Add(PurchaseStorage.GetAllPurchases()[i]);
                }
            }
            return list;
        }

        private static double GetCategorySpends(string category)
        {
            var list = GetPurchasesInCategory(category);
            var result = 0.0;

            foreach (var item in list)
            {
                result += item.Price;
            }
            return result;
        }

        //public static string ShowPurchasesInCategory(Category category)
        //{
        //    var list = GetPurchasesByCategory(category);
        //    var result = new StringBuilder();

        //    if (list.Count == 0) return $"Нет покупок в категории {category.Name}";

        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        result.Append($"{i + 1}). ");
        //        result.Append(list[i].Name);
        //        result.Append('\n');
        //    }

        //    return result.ToString();
        //}

        public static string ShowSpendsByCategories()
        {
            var result = new StringBuilder();
            foreach (var category in CategoryStorage.GetAllCategories())
            {
                var spend = GetCategorySpends(category);
                result.Append($"{category} : {spend} руб.\n");
            }
            return result.ToString();
        }

    }
}
