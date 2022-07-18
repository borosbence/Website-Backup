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
            switch (itemType)
            {
                // TODO: connections
                case "website":
                    var vm = new WebsiteFormViewModel(new Website(), _websiteRepository, _windowService, _stringResource);
                    _windowService.ShowDialog<WebsiteFormWindow, MainWindow>(vm);
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
            Type selectedType = webItem.GetType();
            if (selectedType == typeof(Website))
            {
                var website = (Website)webItem;
                var vm = new WebsiteFormViewModel(website, _websiteRepository, _windowService, _stringResource);
                _windowService.ShowDialog<WebsiteFormWindow, MainWindow>(vm);
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
                    // Notify Treeview
                    Messenger.Send(new WebItemChangedMessage(new WebItemMessage(webItem, Event.Remove)));
                }
            }
            // TODO: connections
            else if (selectedType == typeof(FTPConnection))
            {

            }
            else if (selectedType == typeof(SQLConnection))
            {

            }
        }
    }

    public class WebItemSelectedMessage : ValueChangedMessage<IEntity>
    {
        public WebItemSelectedMessage(IEntity value) : base(value)
        {
        }
    }
}
