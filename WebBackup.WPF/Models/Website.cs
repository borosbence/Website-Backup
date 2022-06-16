using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
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

        [Url]
        [ObservableProperty]
        private string? url;

        #region Navigation properties

        public FTPConnection? FTPConnection { get; set; }

        public SQLConnection? SQLConnection { get; set; }

        [NotMapped]
        public ObservableCollection<Connection> Connections { get; set; } = new ObservableCollection<Connection>();

        #endregion
    }
}
