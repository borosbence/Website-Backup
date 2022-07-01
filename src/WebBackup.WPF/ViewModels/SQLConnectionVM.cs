using WebBackup.Core;

namespace WebBackup.WPF.ViewModels
{
    public class SQLConnectionVM : Connection
    {
        public string DatabaseName { get; set; } = string.Empty;
    }
}
