using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBackup.WPF.Models;
using WebBackup.WPF.Repositories;

namespace WebBackup.WPF.ViewModels
{
    public class WebsitesViewModel
    {
        public WebsitesViewModel(WebsiteRepository repository)
        {
            _repository = repository;
            Task.Run(async () => await Initialize()).Wait();
        }

        private readonly WebsiteRepository _repository;

        public ObservableCollection<Website> Websites { get; set; } = new ObservableCollection<Website>();

        private async Task Initialize()
        {
            Websites = new ObservableCollection<Website>(await _repository.GetAll());
        }
    }
}
