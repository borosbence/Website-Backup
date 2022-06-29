using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsiteForm : ObservableValidator
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [Required]
        [MinLength(1)]
        [ObservableProperty]
        private string name = string.Empty;

        [Url]
        [ObservableProperty]
        private string? url;
    }
}
