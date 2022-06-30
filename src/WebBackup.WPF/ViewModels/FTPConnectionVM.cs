using WebBackup.Core;

namespace WebBackup.WPF.ViewModels
{
    public class FTPConnectionVM : Connection
    {
        public bool IsSSLEnabled { get; set; }

        public bool IsPassive { get; set; }
    }
}
