using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Threading.Tasks;
using WebBackup.Core;
using WebBackup.Core.Repositories;
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
            Messenger.Register<WebItemChangedMessage>(this, (r, m) => UpdateSelected(m));
            Messenger.Register<WebsiteCountChangedMessage>(this, (r, m) => Receive(m));
        }

        private void UpdateSelected(WebItemChangedMessage m)
        {
            SelectedWebItem = m.Value;
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
