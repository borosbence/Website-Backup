using System.Collections.Generic;

namespace WebBackup.WPF
{
    public static class MyExtensions
    {
        public static void AddIfNotNull<T>(this IList<T> list, T item)
        {
            if (item != null)
            {
                list.Add(item);
            }
        }
    }
}
