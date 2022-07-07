using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
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

        private readonly object _lock = new object();

        public WebsitesViewModel(IGenericRepository<Website> repository, IWindowService windowService, IStringResourceService stringResource)
        {
            _repository = repository;
            _windowService = windowService;
            _stringResource = stringResource;
            // async load UI collection
            // Dispatcher.CurrentDispatcher.BeginInvoke(async() => await LoadData());
            BindingOperations.EnableCollectionSynchronization(Websites, _lock);
            Task.Run(async () => await LoadData());
            OnActivated();
        }

        public ObservableCollection<Website> Websites { get; set; } = new();

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
                // Notify Main Window status bar
                Messenger.Send(new WebsiteCountChangedMessage(-1));
            }
        }

        /// <summary>
        /// Load all websites from the repository.
        /// </summary>
        private async Task LoadData()
        {
            List<Website>? dbList = await _repository.GetAllAsync(x => x.FTPConnection, y => y.SQLConnection);
            // lock(_lock)
            Websites.Clear();
            foreach (Website website in dbList)
            {
                website.Connections.AddIfNotNull(website.FTPConnection);
                website.Connections.AddIfNotNull(website.SQLConnection);
                // Build website connections
                Websites.Add(website);
            }
        }

        protected override void OnActivated()
        {
            Messenger.Register<WebsiteChangedMessage>(this, (r, m) => Receive(m));
            Messenger.Register<WebsiteRequestMessage>(this, (r, m) => Receive(m));
        }

        private void Receive(WebsiteChangedMessage message)
        {
            Website website = message.Value;
            // Website existing = Websites.FirstOrDefault(x => x.Id == website.Id);
            bool exists = Websites.Any(x => x.Id == website.Id);
            // Replace, update existing element
            if (exists)
            {
                //int index = Websites.IndexOf(existing);
                // Websites[index] = website;
                // TODO: refresh new item???
                Websites.Refresh();
            }
            // Add new element
            else
            {
                Websites.Add(website);
                // Notify Main Window status bar
                Messenger.Send(new WebsiteCountChangedMessage(1));
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
