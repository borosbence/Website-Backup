using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebBackup.WPF.Models;
using WebBackup.WPF.Repositories;
using WebBackup.WPF.Views;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsitesViewModel
    {
        public WebsitesViewModel(IGenericRepository<Website> repository)
        {
            _repository = repository;
            Task.Run(async () => await LoadData()).Wait();
        }

        private readonly IGenericRepository<Website> _repository;

        public ObservableCollection<Website> Websites { get; set; } = new ObservableCollection<Website>();

        private async Task LoadData()
        {
            Websites = new ObservableCollection<Website>(await _repository.GetAllAsync(x => x.FTPConnection, y => y.SQLConnection));
            foreach (var website in Websites)
            {
                website.Connections.AddIfNotNull(website.FTPConnection);
                website.Connections.AddIfNotNull(website.SQLConnection);
            }
        }

        [ICommand]
        private void NewWebsite()
        {
            // TODO: MVVM approach
            var window = new WebsiteFormWindow();
            window.ShowDialog();
        }
    }
}
