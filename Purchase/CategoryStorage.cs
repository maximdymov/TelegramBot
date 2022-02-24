using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot
{
    static class CategoryStorage
    {
        private static HashSet<Category> categories = new HashSet<Category>();

        public static bool AddCategory(Category category)
        {
            return categories.Add(category);
        }

        public static HashSet<Category> GetAllCategories()
        {
            return categories;
        }

        public static void RemoveCategory(Category category)
        {
            if (categories.Count > 0) categories.Remove(category);
            else throw new Exception("Список категорий пуст");
        }
    }
}
