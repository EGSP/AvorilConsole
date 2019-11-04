using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AvorilConsole.Items;

namespace AvorilConsole.Items.Factory
{
    public static class ItemFactory
    {
        private static Dictionary<int, Item> ClonableItems;

        private static bool IsInitialized = false;

        public static void InitializeItems()
        {
            throw new System.NotImplementedException();
        }

        public static Item CreateItem(int _ID, int _Count)
        {
            if (IsInitialized == false)
                throw new System.Exception("ItemFactory has not initialized");

            // Прдемет, который будет клонирован
            var item = ClonableItems.GetValueOrDefault(_ID, null);

            if (item == null)
                throw new System.Exception("ItemFactory does not contain item with ID:"+_ID.ToString());

            var clone = item.Clone(_Count);

            return clone;
        }
    }
}
