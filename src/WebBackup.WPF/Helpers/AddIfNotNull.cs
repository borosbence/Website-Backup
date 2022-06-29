using System.Collections.Generic;

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
    }
}
