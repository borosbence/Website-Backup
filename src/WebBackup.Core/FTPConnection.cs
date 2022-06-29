using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.Core
{
    [Table("Ftp_Connections")]
    public class FTPConnection : Connection
    {
        public bool IsSSLEnabled { get; set; }

        public bool IsPassive { get; set; }
    }
}
