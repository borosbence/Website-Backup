//using CommunityToolkit.Mvvm.ComponentModel;
//using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.Core
{
    /// <summary>
    /// Base class for Connections.
    /// </summary>
    [NotMapped]
    //[Index(nameof(WebsiteId), IsUnique = true)]
    public abstract partial class Connection : /*ObservableValidator,*/ IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        //[ObservableProperty]
        public string HostName { get; set; } = string.Empty;

        [Required]
        //[ObservableProperty]
        public string Username { get; set; } = string.Empty;

        [Required]
        //[ObservableProperty]
        public string Password { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Website")]
        public int WebsiteId { get; set; }
        public Website? Website { get; set; }
    }
}
