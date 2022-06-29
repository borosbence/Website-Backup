//using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.Core
{
    [Table("Websites")]
    public partial class Website :/* ObservableValidator,*/ IEntity
    {
        [Key]
        public int Id { get; set; }

        // TODO: Localize error messages
        [Required]
        // [MinLength(1)]
        //[ObservableProperty]
        public string Name { get; set; } = string.Empty;

        // [Url]
        //[ObservableProperty]
        public string? Url { get; set; }

        #region Navigation properties
        public FTPConnection? FTPConnection { get; set; }

        public SQLConnection? SQLConnection { get; set; }

        //[NotMapped]
        // TODO: HashSet?
        public ICollection<Connection> Connections { get; set; } = new HashSet<Connection>();
        #endregion
    }
}
