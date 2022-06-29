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

        public WebsitesViewModel(IGenericRepository<Website> repository, IWindowService windowService)
        {
            _repository = repository;
            _windowService = windowService;
            LoadData();
            // Task.Run(async () => await LoadData()).Wait();
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
                selectedWebsite = new();
            }
            _windowService.ShowDialog<WebsiteFormWindow>();
        }

        [ICommand]
        private async Task DeleteAsync(Website selectedWebsite)
        {
            // TODO: localize
            bool confirmed = _windowService.ConfirmDelete("Delete Website", "Confirm Delete?");
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
            foreach (var dbRecord in dbList)
            {
                dbRecord.Connections.AddIfNotNull(dbRecord.FTPConnection);
                dbRecord.Connections.AddIfNotNull(dbRecord.SQLConnection);
                Websites.Add(dbRecord);
            }
        }

        /// <summary>
        /// Register WebsitesViewModel messenger, when receiving request from WebsiteFormWindow.
        /// </summary>
        protected override void OnActivated()
        {
            Messenger.Register<WebsitesViewModel, WebsiteRequestMessage>(this, (r, m) => r.Receive(m));
            Messenger.Register<WebsiteChangedMessage>(this, (r, m) => Receive(m));
        }

        /// <summary>
        /// Send the selected Website data to the Form.
        /// </summary>
        public void Receive(WebsiteRequestMessage message)
        {
            if (selectedWebsite != null)
            {
                message.Reply(new WebsiteForm(selectedWebsite.Id, selectedWebsite.Name, selectedWebsite.Url));
            }
        }

        /// <summary>
        /// Updates View when dialog closed.
        /// </summary>
        /// <param name="message"></param>
        private void Receive(WebsiteChangedMessage message)
        {
            var website = message.Value;
            var existing = Websites.FirstOrDefault(x => x.Id == website.Id);
            if (existing != null)
            {
                var index = Websites.IndexOf(existing);
                Websites[index] = website;
            }
            else
            {
                Websites.Add(website);
            }
        }
    }

    public class WebsiteChangedMessage : ValueChangedMessage<Website>
    {
        public WebsiteChangedMessage(Website value) : base(value)
        {
        }
    }

    public class WebsiteRequestMessage : RequestMessage<WebsiteForm>
    {

    }
}
