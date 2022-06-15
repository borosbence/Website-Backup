using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.WPF.Models
{
    [Table("Sql_Connections")]
    public partial class SQLConnection : Connection
    {
        // TODO: db type

        [Required]
        [ObservableProperty]
        private string databasename = string.Empty;
    }
}
