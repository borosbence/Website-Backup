using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebBackup.Core;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsiteVM : ObservableValidator
    {
        [ObservableProperty]
        private int id;

        [Required]
        [MinLength(1)]
        [ObservableProperty]
        private string name = string.Empty;

        [Url]
        [ObservableProperty]
        private string? url;

        public FTPConnectionVM? FTPConnection { get; set; }

        public SQLConnectionVM? SQLConnection { get; set; }

        public ICollection<Connection> Connections { get; set; } = new HashSet<Connection>();
    }

    public class FTPConnectionVM : Connection
    {
        public bool IsSSLEnabled { get; set; }

        public bool IsPassive { get; set; }
    }

    public class SQLConnectionVM : Connection
    {
        public string DatabaseName { get; set; } = string.Empty;
    }
}
