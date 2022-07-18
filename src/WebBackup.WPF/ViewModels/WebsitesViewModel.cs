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
    public partial class WebsitesViewModel : BaseCommandsViewModel, IRecipient<WebsiteRequestMessage>
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
            Messenger.Register<WebsiteRequestMessage>(this, (r, m) => Receive(m));
        }

        private bool newItem;

        private void Receive(WebItemChangedMessage m)
        {
            IEntity? webitem = m.Value.WebItem;
            // TODO: new, after edit error
            if (webitem == null)
            {
                // newItem = true;
                return;
            }

            Type selectedType = webitem.GetType();

            // TODO: service layer
            switch (m.Value.Event)
            {
                case Event.Select:
                    SelectedWebItem = webitem;
                    break;
                case Event.Add:
                    if (selectedType == typeof(Website))
                    {
                        Website website = (Website)webitem;
                        Websites.Add(website);
                    }
                    // TODO: add ftp connection 
                    break;
                case Event.Refresh:
                    Websites.Refresh();
                    break;
                case Event.Remove:
                    if (selectedType == typeof(Website))
                    {
                        Website website = (Website)webitem;
                        Websites.Remove(website);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Receive(WebsiteRequestMessage m)
        {
            //if (newItem)
            //{
            //    m.Reply(null);
            //    newItem = false;
            //    return;
            //}
            m.Reply(SelectedWebItem as Website);
        }
    }

    public class WebItemChangedMessage : ValueChangedMessage<WebItemMessage>
    {
        public WebItemChangedMessage(WebItemMessage value) : base(value)
        {
        }
    }

    public class WebsiteRequestMessage : RequestMessage<Website?>
    {
    }
}
