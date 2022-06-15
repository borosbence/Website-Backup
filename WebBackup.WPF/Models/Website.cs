using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.WPF.Models
{
    [Table("Websites")]
    public partial class Website : ObservableValidator, IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ObservableProperty]
        private string name = string.Empty;

        [DataType(DataType.Url)]
        [ObservableProperty]
        private string? url;

        
        [ForeignKey("FTPConnection")]
        public int FTPConnectionId { get; set; }
        public FTPConnection? FTPConnection { get; set; }
    }
}
