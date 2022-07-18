using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Threading.Tasks;
using WebBackup.Infrastructure.Repositories;
using WebBackup.WPF.Services;

namespace WebBackup.WPF.ViewModels
{
    public partial class MainViewModel : BaseCommandsViewModel, IRecipient<WebsiteCountChangedMessage>
    {
        public MainViewModel(IWebsiteRepository websiteRepository, IWindowService windowService, IStringResourceService stringResource)
            : base(websiteRepository, windowService, stringResource)
        {
        }

        [ObservableProperty]
        private int websiteCount;

        protected override async Task LoadData()
        {
            WebsiteCount = await _websiteRepository.CountAsync();
        }

        protected override void OnActivated()
        {
            Messenger.Register<WebItemChangedMessage>(this, (r, m) => Receive(m));
            Messenger.Register<WebsiteCountChangedMessage>(this, (r, m) => Receive(m));
        }

        private void Receive(WebItemChangedMessage message)
        {
            this.SelectedWebItem = message.Value.WebItem;
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
