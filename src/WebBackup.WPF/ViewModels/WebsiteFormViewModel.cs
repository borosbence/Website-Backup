using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebBackup.Core;
using WebBackup.Infrastructure.Repositories;
using WebBackup.WPF.Services;

namespace WebBackup.WPF.ViewModels
{
    [ObservableRecipient]
    public partial class WebsiteFormViewModel : ObservableValidator
    {
        private readonly IWebsiteRepository _websiteRepository;
        private readonly IWindowService _windowService;
        private static IStringResourceService _stringResource;

        public WebsiteFormViewModel(IWebsiteRepository repository, IWindowService windowService, IStringResourceService stringResource)
        {
            Website? selected = WeakReferenceMessenger.Default.Send<WebsiteRequestMessage>().Response;
            _websiteRepository = repository;
            _windowService = windowService;
            _stringResource = stringResource;
            if (selected != null)
            {
                _website = selected;
                Title = _stringResource.EditWebsite;
            }
            else
            {
                Title = _stringResource.NewWebsite;
            }

            SaveCommand = new AsyncRelayCommand<object>(SaveAsync, CanSave);
            firstOpen = true;
        }

        /// <summary>
        /// Title of the Dialog window.
        /// </summary>
        [ObservableProperty]
        private string? title;
        /// <summary>
        /// This is the first opening of the dialog.
        /// </summary>
        private bool firstOpen;

        private readonly Website _website = new();

        private int Id => _website.Id;

        [Required(AllowEmptyStrings = false)]
        public string Name
        {
            get => _website.Name;
            set {
                SetProperty(_website.Name, value, _website, (w, name) => w.Name = name, true);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        [CustomValidation(typeof(WebsiteFormViewModel), nameof(ValidateUrl))]
        public string? Url
        {
            get => _website.Url;
            set
            {
                SetProperty(_website.Name, value, _website, (w, url) => w.Url = url, true);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        public IAsyncRelayCommand<object> SaveCommand { get; }

        // [ICommand(CanExecute = nameof(CanSave))]
        private async Task SaveAsync(object window)
        {
            var website = _website;
            bool exists = await _websiteRepository.ExistsAsync(website.Id);
            if (exists)
            {
                await _websiteRepository.UpdateAsync(website);
            }
            else
            {
                await _websiteRepository.InsertAsync(website);
                website.Id = website.Id;
            }
            // Notify collection
            WeakReferenceMessenger.Default.Send(new WebItemChangedMessage(website));
            _windowService.Close(window);
        }

        /// <summary>
        /// Validate the form and enable the Save button. On the first open, the input fields errors not shown.
        /// </summary>
        /// <returns>Form is valid.</returns>
        public bool CanSave(object window)
        {
            if (firstOpen && Id == 0)
            {
                return firstOpen = false;
            }
            ValidateAllProperties();
            return !HasErrors;
        }

        public static ValidationResult? ValidateUrl(string? url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                bool isUri = Uri.IsWellFormedUriString(url, UriKind.Absolute);
                if (!isUri)
                {
                    return new(_stringResource.InvalidURL);
                }
            }
            return ValidationResult.Success;
        }
    }
}
