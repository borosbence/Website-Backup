using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
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

        protected BaseCommandsViewModel(IWebsiteRepository repository, IWindowService windowService, IStringResourceService stringResource)
        {
            _websiteRepository = repository;
            _windowService = windowService;
            _stringResource = stringResource;
            Task.Run(LoadData);
            OnActivated();
        }

        protected abstract Task LoadData();

        [ObservableProperty]
        public IEntity? selectedWebItem;


        // TODO: messenger selected item
        [ICommand]
        protected void NewItem(string itemType)
        {
            if (string.IsNullOrWhiteSpace(itemType))
            {
                return;
            }
            SelectedWebItem = null;
            switch (itemType)
            {
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
            Type selectedType = webItem.GetType();
            if (selectedType == typeof(Website))
            {
                _windowService.ShowDialog<WebsiteFormWindow, MainWindow>();
            }
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
                    // Notify Treeview
                    WeakReferenceMessenger.Default.Send(new WebItemRemovedMessage(website));
                    // Notify Status bar
                    WeakReferenceMessenger.Default.Send(new WebsiteCountChangedMessage(-1));
                }
            }
            else if (selectedType == typeof(FTPConnection))
            {

            }
            else if (selectedType == typeof(SQLConnection))
            {

            }
        }
    }
}
