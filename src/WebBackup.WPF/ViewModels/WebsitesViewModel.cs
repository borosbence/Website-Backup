using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;
using WebBackup.Core;
using WebBackup.Infrastructure.Repositories;
using WebBackup.WPF.Services;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsitesViewModel : BaseCommandsViewModel
    {
        private readonly object _lock = new();

        public WebsitesViewModel(IWebsiteRepository websiteRepository, IWindowService windowService, IStringResourceService stringResource) 
            : base(websiteRepository, windowService, stringResource)
        {
            BindingOperations.EnableCollectionSynchronization(Websites, _lock);
        }

        public ObservableCollection<Website> Websites { get; set; } = new();

        /// <summary>
        /// Load all websites from the repository.
        /// </summary>
        protected override async Task LoadData()
        {
            List<Website>? dbList = await _websiteRepository.GetAllAsync(x => x.FTPConnection, y => y.SQLConnection);
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
            Messenger.Register<WebItemChangedMessage>(this, (r, m) => Receive(m));
        }

        private void Receive(WebItemChangedMessage message)
        {
            // TODO: service layer
            IEntity? webitem = message.Value.WebItem;
            if (webitem == null)
            {
                return;
            }

            Type selectedType = webitem.GetType();
            switch (message.Value.Event)
            {
                case Event.Add:
                    if (selectedType == typeof(Website))
                    {
                        Website website = (Website)webitem;
                        Websites.Add(website);
                    }
                    // TODO: add ftp connection 
                    break;
                
                case Event.Remove:
                    if (selectedType == typeof(Website))
                    {
                        Website website = (Website)webitem;
                        Websites.Remove(website);
                    }
                    break;
                case Event.Replace:
                    Websites.Refresh();
                    break;
                case Event.Select:
                    SelectedWebItem = webitem;
                    break;
                default:
                    break;
            }
        }
    }

    public class WebItemChangedMessage : ValueChangedMessage<WebItemMessage>
    {
        public WebItemChangedMessage(WebItemMessage value) : base(value)
        {
        }
    }
}
