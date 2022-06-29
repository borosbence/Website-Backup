//using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.Core
{
    [Table("Ftp_Connections")]
    public partial class FTPConnection : Connection
    {
        //[ObservableProperty]
        public bool IsSSLEnabled { get; set; }

        //[ObservableProperty]
        public bool IsPassive { get; set; }
    }
}
