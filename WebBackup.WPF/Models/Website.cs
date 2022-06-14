using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBackup.WPF.Models
{
    [Table("Websites")]
    public class Website : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Url)]
        public string? URL { get; set; }
    }
}
