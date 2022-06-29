using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.Core
{
    /// <summary>
    /// Base class for Connections.
    /// </summary>
    [NotMapped]
    public abstract class Connection : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string HostName { get; set; } = string.Empty;

        [Required]
        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Website")]
        public int WebsiteId { get; set; }
        public Website? Website { get; set; }
    }
}
