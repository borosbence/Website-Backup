using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBackup.WPF.Models
{
    [Table("Ftp_Connections")]
    public partial class FTPConnection : Connection
    {
        [ObservableProperty]
        private bool isSSLEnabled = false;

        [ObservableProperty]
        private bool isPassive = false;
    }
}
