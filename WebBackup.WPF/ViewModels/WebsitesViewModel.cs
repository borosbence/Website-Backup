using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WebBackup.WPF.Models;
using WebBackup.WPF.Repositories;
using WebBackup.WPF.Views;

namespace WebBackup.WPF.ViewModels
{
    public partial class WebsitesViewModel : ObservableRecipient, IRecipient<WebsiteRequestMessage>
    {
        private readonly IGenericRepository<Website> _repository;
        public WebsitesViewModel(IGenericRepository<Website> repository)
        {
            _repository = repository;
            Task.Run(async () => await LoadData()).Wait();
            OnActivated();
        }

        public ObservableCollection<Website> Websites { get; set; } = new ObservableCollection<Website>();

        [ObservableProperty]
        private Website selectedWebsite;

        private async Task LoadData()
        {
            Websites = new ObservableCollection<Website>(await _repository.GetAllAsync(x => x.FTPConnection, y => y.SQLConnection));
            foreach (var website in Websites)
            {
                website.Connections.AddIfNotNull(website.FTPConnection);
                website.Connections.AddIfNotNull(website.SQLConnection);
            }
        }

        protected override void OnActivated()
        {
            Messenger.Register<WebsitesViewModel, WebsiteRequestMessage>(this, (r, m) => r.Receive(m));
        }

        public void Receive(WebsiteRequestMessage message)
        {
            message.Reply(selectedWebsite);
        }

        [ICommand]
        private void ShowWebsite(string param)
        {
            // TODO: MVVM approach
            if (!string.IsNullOrEmpty(param))
            {
                selectedWebsite = new();
            }
            var window = new WebsiteFormWindow();
            // Messenger.Send(new WebsiteChangedMessage(selectedWebsite));

            window.ShowDialog();
        }
    }

    public class WebsiteChangedMessage : ValueChangedMessage<Website>
    {
        public WebsiteChangedMessage(Website value) : base(value)
        {
        }
    }

    public sealed class WebsiteRequestMessage : RequestMessage<Website>
    {

    }
}
