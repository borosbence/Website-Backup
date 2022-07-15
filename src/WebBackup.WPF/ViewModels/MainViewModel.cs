using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Threading.Tasks;
using WebBackup.Core;
using WebBackup.Core.Repositories;

namespace WebBackup.WPF.ViewModels
{
    public partial class MainViewModel : ObservableRecipient, IRecipient<WebsiteCountChangedMessage>
    {
        private readonly IGenericRepository<Website> _websiteRepository;

        public MainViewModel(IGenericRepository<Website> websiteRepository)
        {
            _websiteRepository = websiteRepository;
            // Task.Run(async () => await LoadData());
            Task.Run(LoadData);
            OnActivated();
        }

        [ObservableProperty]
        private int websiteCount;

        private async Task LoadData()
        {
            var websites = await _websiteRepository.GetAllAsync();
            WebsiteCount = websites.Count;
        }

        protected override void OnActivated()
        {
            Messenger.Register<WebsiteCountChangedMessage>(this, (r, m) => Receive(m));
        }

        public void Receive(WebsiteCountChangedMessage message)
        {
            WebsiteCount += message.Value;
        }
    }

    public class WebsiteCountChangedMessage : ValueChangedMessage<int>
    {
        public WebsiteCountChangedMessage(int value) : base(value)
        {
        }
    }
}
