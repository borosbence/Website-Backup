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
    public partial class FTPConnection : ObservableValidator, IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ObservableProperty]
        private string hostname = string.Empty;

        [Required]
        [ObservableProperty]
        private string username = string.Empty;

        [Required]
        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private bool isSSLEnabled = false;

        [ObservableProperty]
        private bool isPassive = false;

        [ForeignKey("Website")]
        public int WebsiteId { get; set; }
        public Website Website { get; set; }
    }
}
