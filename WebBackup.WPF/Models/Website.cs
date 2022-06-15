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
        [StringLength(250)]
        [ObservableProperty]
        private string name = string.Empty;

        [DataType(DataType.Url)]
        [ObservableProperty]
        private string? url;
    }
}
