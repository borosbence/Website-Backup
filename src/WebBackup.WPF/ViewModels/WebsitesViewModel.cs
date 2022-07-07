using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebBackup.Core;
using WebBackup.Core.Repositories;
using WebBackup.WPF.Services;
using WebBackup.WPF.Views;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsitesViewModel : ObservableRecipient, IRecipient<WebsiteRequestMessage>
    {
        private readonly IGenericRepository<Website> _repository;
        private readonly IWindowService _windowService;
        private readonly IStringResourceService _stringResource;

        public WebsitesViewModel(IGenericRepository<Website> repository, IWindowService windowService, IStringResourceService stringResource)
        {
            _repository = repository;
            _windowService = windowService;
            _stringResource = stringResource;
            Task.Run(async () => await LoadData()).Wait();
            OnActivated();
        }

        public ObservableCollection<Website> Websites { get; set; } = new ObservableCollection<Website>();

        [ObservableProperty]
        private Website? selectedWebsite;

        [ICommand]
        private void ShowWebsite(string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                selectedWebsite = null;
            }
            //var wfVM = new WebsiteFormViewModel(selectedWebsite, _repository, _windowService);
            //_windowService.ShowDialog<WebsiteFormWindow>(wfVM);
            _windowService.ShowDialog<WebsiteFormWindow, MainWindow>();
        }

        [ICommand]
        private async Task DeleteAsync(Website selectedWebsite)
        {
            bool confirmed = _windowService.ConfirmDelete(
                _stringResource.GetValue("DeleteWebsite"),
                _stringResource.GetValue("ConfirmDelete"));
            if (confirmed)
            {
                await _repository.DeleteAsync(selectedWebsite);
                Websites.Remove(selectedWebsite);
            }
        }

        /// <summary>
        /// Load all websites from the repository.
        /// </summary>
        private async Task LoadData()
        {
            Websites.Clear();
            var dbList = await _repository.GetAllAsync(x => x.FTPConnection, y => y.SQLConnection);
            // Build website connections
            dbList.ForEach(website => {
                website.Connections.AddIfNotNull(website.FTPConnection);
                website.Connections.AddIfNotNull(website.SQLConnection);
                Websites.Add(website);
            });
        }

        protected override void OnActivated()
        {
            Messenger.Register<WebsiteChangedMessage>(this, (r, m) => Receive(m));
            Messenger.Register<WebsiteRequestMessage>(this, (r, m) => Receive(m));
        }

        private void Receive(WebsiteChangedMessage message)
        {
            var websiteVM = message.Value;
            var existing = Websites.FirstOrDefault(x => x.Id == websiteVM.Id);
            // Replace, update existing element
            if (existing != null)
            {
                var index = Websites.IndexOf(existing);
                Websites[index] = websiteVM;
            }
            // Add new element
            else
            {
                Websites.Add(websiteVM);
            }
        }

        public void Receive(WebsiteRequestMessage message)
        {
            message.Reply(selectedWebsite);
        }

    }

    public class WebsiteChangedMessage : ValueChangedMessage<Website>
    {
        public WebsiteChangedMessage(Website value) : base(value)
        {
        }
    }
    public class WebsiteRequestMessage : RequestMessage<Website?>
    {

    }
}
