//using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.Core
{
    [Table("Sql_Connections")]
    public partial class SQLConnection : Connection
    {
        // TODO: db type

        [Required]
        //[ObservableProperty]
        public string DatabaseName { get; set; } = string.Empty;
    }
}
