using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebBackup.WPF.Models;
using WebBackup.WPF.Repositories;

namespace WebBackup.WPF.ViewModels
{
    public class WebsitesViewModel
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
            Websites = new ObservableCollection<Website>(await _repository.GetAll(x => x.FTPConnection, y => y.SQLConnection));
            foreach (var website in Websites)
            {
                website.Connections.AddIfNotNull(website.FTPConnection);
                website.Connections.AddIfNotNull(website.SQLConnection);
            }
        }
    }
}
