using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.WPF.Models
{
    /// <summary>
    /// Base class for Connections.
    /// </summary>
    [NotMapped]
    [Index(nameof(WebsiteId), IsUnique = true)]
    public abstract partial class Connection : ObservableValidator, IEntity
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

        [Required]
        [ForeignKey("Website")]
        public int WebsiteId { get; set; }
        public Website? Website { get; set; }
    }
}
