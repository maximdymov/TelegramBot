using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    public static class CategoryStorage
    {
        private static HashSet<string> categories = new HashSet<string>();

        public static bool AddCategory(string category)
        {
            return categories.Add(category);
        }

        public static HashSet<string> GetAllCategories()
        {
            return categories;
        }

        public static void RemoveCategory(string category)
        {
            if (categories.Count > 0) categories.Remove(category);
        }
    }
}
