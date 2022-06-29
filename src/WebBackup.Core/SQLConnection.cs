using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.Core
{
    [Table("Sql_Connections")]
    public class SQLConnection : Connection
    {
        // TODO: db type

        [Required]
        public string DatabaseName { get; set; } = string.Empty;
    }
}
