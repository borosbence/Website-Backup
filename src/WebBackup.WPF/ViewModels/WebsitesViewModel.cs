using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            Messenger.Register<WebItemChangedMessage>(this, (r, m) => InsertOrRefresh(m));
            Messenger.Register<WebItemRemovedMessage>(this, (r, m) => Remove(m));
            Messenger.Register<WebsiteRequestMessage>(this, (r, m) => Receive(m));
        }

        private void InsertOrRefresh(WebItemChangedMessage m)
        {
            if (m.Value == null)
            {
                return;
            }
            Type selectedType = m.Value.GetType();
            if (selectedType == typeof(Website))
            {
                Website website = (Website)m.Value;
                bool exists = Websites.Any(x => x.Id == website.Id);
                // Replace, update existing element
                if (exists)
                {
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
            else if (selectedType == typeof(FTPConnection))
            {
            }
            else if (selectedType == typeof(SQLConnection))
            {
            }
        }

        private void Remove(WebItemRemovedMessage m)
        {
            if (m.Value == null)
            {
                return;
            }
            Type selectedType = m.Value.GetType();
            if (selectedType == typeof(Website))
            {
                Website website = (Website)m.Value;
                Websites.Remove(website);
            }
        }

        public void Receive(WebsiteRequestMessage m)
        {
            m.Reply(selectedWebItem as Website);
        }

    }

    public class WebItemChangedMessage : ValueChangedMessage<IEntity>
    {
        public WebItemChangedMessage(IEntity value) : base(value)
        {
        }
    }
    public class WebItemRemovedMessage : ValueChangedMessage<IEntity>
    {
        public WebItemRemovedMessage(IEntity value) : base(value)
        {
        }
    }
    public class WebsiteRequestMessage : RequestMessage<Website?>
    {

    }
}
