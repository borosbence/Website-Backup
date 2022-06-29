using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
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
            OnActivated();
        }

        public ObservableCollection<Website> Websites { get; set; } = new ObservableCollection<Website>();

        [ObservableProperty]
        private Website? selectedWebsite;

        /// <summary>
        /// Load all websites from the repository.
        /// </summary>
        private async Task LoadData()
        {
            Websites = new ObservableCollection<Website>(await _repository.GetAllAsync(x => x.FTPConnection, y => y.SQLConnection));
            foreach (var website in Websites)
            {
                website.Connections.AddIfNotNull(website.FTPConnection);
                website.Connections.AddIfNotNull(website.SQLConnection);
            }
        }

        /// <summary>
        /// Register WebsitesViewModel messenger, when receiving request from WebsiteFormWindow.
        /// </summary>
        protected override void OnActivated()
        {
            Messenger.Register<WebsitesViewModel, WebsiteRequestMessage>(this, (r, m) => r.Receive(m));
        }

        /// <summary>
        /// Send the selected Website data to the Form.
        /// </summary>
        public void Receive(WebsiteRequestMessage message)
        {
            message.Reply(new WebsiteForm(selectedWebsite.Id, selectedWebsite.Name, selectedWebsite.Url));
        }

        [ICommand]
        private void ShowWebsite(string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                selectedWebsite = new();
            }
            _windowService.ShowDialog<WebsiteFormWindow>();
            // Messenger.Send(new WebsiteChangedMessage(selectedWebsite));
        }
    }

    public class WebsiteChangedMessage : ValueChangedMessage<WebsiteForm>
    {
        public WebsiteChangedMessage(WebsiteForm value) : base(value)
        {
        }
    }

    public class WebsiteRequestMessage : RequestMessage<WebsiteForm>
    {

    }
}
