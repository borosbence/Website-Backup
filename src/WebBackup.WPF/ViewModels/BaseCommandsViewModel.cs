using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Threading.Tasks;
using WebBackup.Core;
using WebBackup.Infrastructure.Repositories;
using WebBackup.WPF.Services;
using WebBackup.WPF.Views;

namespace WebBackup.WPF.ViewModels
{
    public abstract partial class BaseCommandsViewModel : ObservableRecipient
    {
        protected readonly IWebsiteRepository _websiteRepository;
        protected readonly IWindowService _windowService;
        protected readonly IStringResourceService _stringResource;

        protected BaseCommandsViewModel(IWebsiteRepository websiteRepository, IWindowService windowService, IStringResourceService stringResource)
        {
            _websiteRepository = websiteRepository;
            _windowService = windowService;
            _stringResource = stringResource;
            Task.Run(LoadData);
            OnActivated();
        }

        protected abstract Task LoadData();

        [ObservableProperty]
        private IEntity? selectedWebItem;

        [ICommand]
        protected void NewItem(string itemType)
        {
            if (string.IsNullOrWhiteSpace(itemType))
            {
                return;
            }
            // SelectedWebitem is null
            // Messenger.Send(new WebItemChangedMessage(new WebItemMessage(null, Event.Select)));
            switch (itemType)
            {
                // TODO: connections
                case "website":
                    _windowService.ShowDialog<WebsiteFormWindow, MainWindow>();
                    break;
                default:
                    break;
            }
        }

        [ICommand]
        protected void EditItem(IEntity webItem)
        {
            if (webItem == null)
            {
                return;
            }
            // selectedWebItem = webItem;
            Type selectedType = webItem.GetType();
            if (selectedType == typeof(Website))
            {
                _windowService.ShowDialog<WebsiteFormWindow, MainWindow>();
            }
            // TODO: connections
            else if (selectedType == typeof(FTPConnection))
            {
            }
            else if (selectedType == typeof(SQLConnection))
            {
            }
        }

        [ICommand]
        protected async Task DeleteAsync(IEntity webItem)
        {
            if (webItem == null)
            {
                return;
            }
            Type selectedType = webItem.GetType();
            if (selectedType == typeof(Website))
            {
                bool confirmed = _windowService.ConfirmDelete(
                _stringResource.DeleteWebsite,
                _stringResource.ConfirmDelete);
                if (confirmed)
                {
                    Website website = (Website)webItem;
                    await _websiteRepository.DeleteAsync(website);
                    // Notify Status bar
                    Messenger.Send(new WebsiteCountChangedMessage(-1));
                }
            }
            // TODO: connections
            else if (selectedType == typeof(FTPConnection))
            {

            }
            else if (selectedType == typeof(SQLConnection))
            {

            }
            // Notify Treeview
            Messenger.Send(new WebItemChangedMessage(new WebItemMessage(webItem, Event.Remove)));
            
        }
    }

    public class WebItemSelectedMessage : ValueChangedMessage<IEntity>
    {
        public WebItemSelectedMessage(IEntity value) : base(value)
        {
        }
    }
}
