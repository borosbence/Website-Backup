using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsiteForm : ObservableValidator
    {
        public WebsiteForm(int id, string name, string? url)
        {
            this.id = id;
            this.name = name;
            this.url = url;
        }
        
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
