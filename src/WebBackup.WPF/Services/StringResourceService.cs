using System.Windows;

namespace WebBackup.WPF.Services
{
    public interface IStringResourceService
    {
        string? GetValue(string resourceKey);
        string? ConfirmDelete { get; }
        string? DeleteWebsite { get; }
        string? EditWebsite { get; }
        string? NewWebsite { get; }
        string? InvalidURL { get; }
    }
    public class StringResourceService : IStringResourceService
    {
        public string? GetValue(string resourceKey) => Application.Current.TryFindResource(resourceKey).ToString();
        public string? ConfirmDelete => GetValue("ConfirmDelete");
        public string? DeleteWebsite => GetValue("DeleteWebsite");
        public string? EditWebsite => GetValue("EditWebsite");
        public string? NewWebsite => GetValue("NewWebsite");
        public string? InvalidURL => GetValue("InvalidURL");
    }
}
