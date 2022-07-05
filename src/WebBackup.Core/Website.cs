using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.Core
{
    [Table("Websites")]
    public class Website : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Url { get; set; }

        #region Navigation properties
        public FTPConnection? FTPConnection { get; set; }

        public SQLConnection? SQLConnection { get; set; }

        public ICollection<Connection> Connections { get; set; } = new HashSet<Connection>();
        #endregion
    }
}
