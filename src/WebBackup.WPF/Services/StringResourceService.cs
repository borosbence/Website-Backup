using System.Windows;

namespace WebBackup.WPF.Services
{
    public interface IStringResourceService
    {
        string? GetValue(string resourceKey);
    }
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
