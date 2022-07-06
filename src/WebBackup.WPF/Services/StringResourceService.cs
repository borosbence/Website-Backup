using System.Windows;
using WebBackup.Infrastructure.Services;

namespace WebBackup.WPF.Services
{
    public class StringResourceService : IStringResourceService
    {
        public string? GetValue(string resourceKey)
        {
            var resource = Application.Current.Resources[resourceKey];
            if (resource != null)
            {
                return resource.ToString();
            }
            return null;
        }
    }
}
