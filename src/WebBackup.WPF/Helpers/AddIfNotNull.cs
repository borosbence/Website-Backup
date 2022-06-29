using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WebBackup.WPF
{
    public static class Helpers
    {
        public static void AddIfNotNull<T>(this ICollection<T> list, T? item)
        {
            if (item != null)
            {
                list.Add(item);
            }
        }

        public static void ReplaceItem<T>(this ObservableCollection<T> items, Func<T, bool> predicate, T newItem)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (predicate(items[i]))
                {
                    items[i] = newItem;
                    break;
                }
            }
        }
    }
}
